using ArenaShooter.Controllers;
using ArenaShooter.Entities;
using ArenaShooter.Extensions;
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
        [SerializeField] private float effectTime = 1f;

        #endregion

        #region Private variables

        private GameObjectPool<LineRenderer> pool;

        #endregion

        protected override void OnInitialized()
        {
            pool = new GameObjectPool<LineRenderer>(transform, electricShotPrefab, weapon.Stats.MaxAmmoPerClip * 2);
        }

        private IEnumerator DespawnCooldown(LineRenderer effect)
        {
            float timeLeft = effectTime;

            while (timeLeft > 0f)
            {
                timeLeft -= Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            pool.PoolItem(effect);
        }

        /// <summary>
        /// Returns the closest entities based on <see cref="Weapon.MaxDistance"/> and <see cref="Weapon.Range"/> as well as <see cref="Weapon.AmmoLeftInClip"/>.
        /// </summary>
        public override List<IEntity> GetTargets()
        {
            int           shotsLeft     = weapon.AmmoLeftInClip;
            Vector3       position      = weapon.WeaponHolder.WeaponFirePosition;
            List<IEntity> targets       = new List<IEntity>();
            IEntity       currentEntity = EntityController.Singleton.GetClosestEntity(position, weapon.Stats.MaxDistance, targets, EntityTeam.Enemy);

            while (shotsLeft > 0 && currentEntity != null)
            {
                shotsLeft -= weapon.Stats.AmmoPerFire;

                position = currentEntity.BodyOriginPosition;
                targets.Add(currentEntity);

                currentEntity = EntityController.Singleton.GetClosestEntity(position, weapon.Stats.Range, targets, EntityTeam.Enemy);
            }

            weapon.DepleteAmmo(weapon.AmmoLeftInClip - shotsLeft);

            return targets;
        }

        public override void OnEvent(WeaponFireEffectEvent @event)
        {
            var effect = pool.GetItem();

            if (effect != null)
            {
                effect.SetPosition(0, @event.Point);
                effect.SetPosition(1, @event.Forward);
                StartCoroutine("DespawnCooldown", effect);
            }
        }

    }

}
