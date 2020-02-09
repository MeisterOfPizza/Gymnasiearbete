using ArenaShooter.Controllers;
using ArenaShooter.Entities;
using ArenaShooter.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Combat.Utils
{

    sealed class TransferdElectricShot : ElectricShot
    {

        #region Editor

        [Header("Prefabs")]
        [SerializeField] private GameObject electricShotPrefab;

        [Header("Values")]
        [SerializeField] private float effectTime            = 1f;
        [SerializeField] private int   effectLineSegments    = 25;
        [SerializeField] private float effectMaxDeviation    = 0.1f;
        [SerializeField] private float effectUpdateFrequency = 0.01f;

        #endregion

        #region Private variables

        private GameObjectPool<LineRenderer> pool;

        #endregion

        protected override void OnInitialized()
        {
            pool = new GameObjectPool<LineRenderer>(transform, electricShotPrefab, weapon.Stats.MaxAmmoPerClip * 2);

            foreach (var item in pool.PooledItems)
            {
                item.positionCount = effectLineSegments;
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();

            pool.PoolAllItems();
        }

        private IEnumerator UpdateEffectLine(Tuple<LineRenderer, WeaponFireEffectEvent> lineEventPair)
        {
            float timeLeft   = effectTime;
            float updateTime = effectUpdateFrequency;

            Vector3 start = lineEventPair.Item2.Point;
            Vector3 end   = lineEventPair.Item2.Forward;

            while (timeLeft > 0f)
            {
                timeLeft   -= Time.deltaTime;
                updateTime -= Time.deltaTime;

                // Update the alpha to fade out:
                lineEventPair.Item1.startColor = new Color(lineEventPair.Item1.startColor.r, lineEventPair.Item1.startColor.g, lineEventPair.Item1.startColor.b, timeLeft / effectTime);
                lineEventPair.Item1.endColor   = new Color(lineEventPair.Item1.endColor.r, lineEventPair.Item1.endColor.g, lineEventPair.Item1.endColor.b, timeLeft / effectTime);

                // Update the position:
                if (updateTime <= 0f)
                {
                    updateTime = effectUpdateFrequency;

                    UpdateLineEffect(lineEventPair.Item1, start, end);
                }

                yield return new WaitForEndOfFrame();
            }

            pool.PoolItem(lineEventPair.Item1);
        }

        /// <summary>
        /// Returns the closest entities based on <see cref="Weapon.Stats.MaxDistance"/> and <see cref="Weapon.Stats.Range"/> as well as <see cref="Weapon.AmmoLeftInClip"/>.
        /// </summary>
        public override List<IEntity> GetTargets()
        {
            int           shotsLeft      = weapon.AmmoLeftInClip;
            Vector3       position       = weapon.WeaponHolder.WeaponFirePosition;
            List<IEntity> targets        = new List<IEntity>();
            List<IEntity> checkedTargets = new List<IEntity>();
            IEntity       currentEntity  = EntityController.Singleton.GetClosestEntity(position, weapon.Stats.MaxDistance, targets, weapon.Stats.TargetEntityTeam);

            while (shotsLeft > 0 && currentEntity != null)
            {
                var hit = Extensions.Utils.Raycast(new Ray(position, (currentEntity.BodyOriginPosition - position).normalized), weapon.Stats.Range, weapon.WeaponHolder.WeaponHitLayerMask, currentEntity.gameObject, QueryTriggerInteraction.Ignore);
                
                if (!hit.WorldHit)
                {
                    shotsLeft -= weapon.Stats.AmmoPerFire;

                    position = currentEntity.BodyOriginPosition;
                    targets.Add(currentEntity);
                }

                checkedTargets.Add(currentEntity);

                currentEntity = EntityController.Singleton.GetClosestEntity(position, weapon.Stats.Range, checkedTargets, weapon.Stats.TargetEntityTeam);
            }

            weapon.DepleteAmmo(weapon.AmmoLeftInClip - shotsLeft);

            return targets;
        }

        public override void OnEvent(WeaponFireEffectEvent @event)
        {
            var effect = pool.GetItem();

            if (effect != null)
            {
                UpdateLineEffect(effect, @event.Point, @event.Forward);
                StartCoroutine("UpdateEffectLine", new Tuple<LineRenderer, WeaponFireEffectEvent>(effect, @event));
            }
        }

        private void UpdateLineEffect(LineRenderer line, Vector3 start, Vector3 end)
        {
            for (int i = 0; i < effectLineSegments; i++)
            {
                Vector3 point = Vector3.Lerp(start, end, i / (float)effectLineSegments);
                Vector3 next  = Vector3.Lerp(start, end, (i + 1) / (float)effectLineSegments);

                // Create a random offset in the forward direction to act upon the x- and y-axis.
                // Multiply the offset with 0 (making it obsolete) if the current segment is at the start or the end.
                Vector3 offset = Quaternion.LookRotation((next - point).normalized, Vector3.up) * new Vector3(UnityEngine.Random.Range(-effectMaxDeviation, effectMaxDeviation),
                                                                                                              UnityEngine.Random.Range(-effectMaxDeviation, effectMaxDeviation),
                                                                                                              0f) * (i == 0 || i == effectLineSegments - 1 ? 0f : 1f);

                // Set the position and add the offset:
                line.SetPosition(i, point + offset);
            }
        }

    }

}
