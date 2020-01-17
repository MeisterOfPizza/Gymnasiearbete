using ArenaShooter.Controllers;
using ArenaShooter.Entities;
using ArenaShooter.Extensions;
using Bolt;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Combat.Utils
{

    sealed class SingleHealSupportShot : SupportShot
    {

        #region Editor

        [Header("References")]
        [SerializeField] private LineRenderer effectLineRenderer;

        [Header("Values")]
        [SerializeField] private int   effectLineCurveSegments = 25;
        [SerializeField] private float beamSpinSpeed  = 1f;
        [SerializeField] private float beamSpinRadius = 0.1f;

        #endregion

        #region Public properties

        public override bool WeaponCanFire
        {
            get
            {
                return EntityController.Singleton.GetClosestEntity(weapon.WeaponHolder.WeaponFirePosition, weapon.Stats.Range, weapon.WeaponHolder, weapon.Stats.SupportTargetEntityTeam) != null;
            }
        }

        public override bool HasValidTargets
        {
            get
            {
                return target != null && target != weapon.WeaponHolder;
            }
        }

        #endregion

        #region Private variables

        private IEntity target;

        #endregion

        protected override void OnInitialized()
        {
            base.OnInitialized();

            effectLineRenderer.positionCount = effectLineCurveSegments;
        }

        public override void OnBeginSupport()
        {
            var potentialTarget = EntityController.Singleton.GetClosestEntity(weapon.WeaponHolder.WeaponFirePosition, weapon.Stats.Range, weapon.WeaponHolder, weapon.Stats.SupportTargetEntityTeam);

            if (potentialTarget != null)
            {
                target = potentialTarget;

                var beginFiring     = WeaponSupportBeginFireEffectEvent.Create(weapon.WeaponHolder.entity, EntityTargets.EveryoneExceptOwner);
                beginFiring.Begin   = true;
                beginFiring.Shooter = weapon.WeaponHolder.entity;
                beginFiring.Target  = target.entity;
                beginFiring.Send();
                
                OnEvent(beginFiring);
            }
        }

        public override void OnEndSupport()
        {
            var beginFiring     = WeaponSupportBeginFireEffectEvent.Create(weapon.WeaponHolder.entity, EntityTargets.EveryoneExceptOwner);
            beginFiring.Begin   = false;
            beginFiring.Shooter = weapon.WeaponHolder.entity;
            beginFiring.Send();

            OnEvent(beginFiring);
        }

        public override void SupportTargets()
        {
            if (target != null && Vector3.Distance(weapon.WeaponHolder.WeaponFirePosition, target.BodyOriginPosition) <= weapon.Stats.Range)
            {
                var healEvent    = HealEvent.Create(GlobalTargets.Everyone, ReliabilityModes.ReliableOrdered);
                healEvent.Target = target.entity;
                healEvent.Heal   = weapon.Damage;
                healEvent.Send();
            }
            else
            {
                weapon.StopFiring();
            }
        }

        public override void OnEvent(WeaponSupportBeginFireEffectEvent @event)
        {
            target = @event.Target != null ? EntityController.Singleton.FindEntityWithBoltEntity(@event.Target) : null;

            effectLineRenderer.gameObject.SetActive(@event.Begin);
        }

        public override void UpdateSupportShot()
        {
            Vector2 start     = new Vector2(weapon.WeaponHolder.WeaponFirePosition.x, weapon.WeaponHolder.WeaponFirePosition.z);
            Vector2 end       = new Vector2(target.BodyOriginPosition.x, target.BodyOriginPosition.z);
            Vector2 delta     = end - start;
            Vector2 targetDir = delta.normalized;
            Vector2 aimDir    = new Vector2(weapon.WeaponHolder.WeaponForward.x, weapon.WeaponHolder.WeaponForward.z);

            BezierCurve effectCurve = new BezierCurve(start,
                                                      start + delta.magnitude * 0.50f * aimDir,
                                                      start + delta.magnitude * 0.75f * Vector2.Lerp(aimDir, targetDir, 0.75f),
                                                      end);

            for (int i = 0; i < effectLineCurveSegments; i++)
            {
                // Time variable for curve segment index:
                float t = i / (float)effectLineCurveSegments;

                // Current and next point for the line renderer:
                Vector2 point = effectCurve.GetPoint(t);
                Vector2 next  = effectCurve.GetPoint((i + 1) / (float)effectLineCurveSegments);
                Vector3 dir   = (new Vector3(next.x, 0f, next.y) - new Vector3(point.x, 0f, point.y)).normalized;

                // Spin offset:
                Vector3 offset = Quaternion.LookRotation(dir, Vector3.up) * new Vector3(Mathf.Cos(Time.time * beamSpinSpeed) * beamSpinRadius, Mathf.Sin(Time.time * beamSpinSpeed) * beamSpinRadius);

                // Curved multiplication to avoid offset near start and end:
                // t = 0.0 => offset = 0.0
                // t = 0.5 => offset = 1.0
                // t = 1.0 => offset = 0.0
                // offset *= -4(0.5 - t)^2 + 1 = (1 - t) * 4t
                offset *= (1f - t) * 4f * t;

                effectLineRenderer.SetPosition(i,
                                               new Vector3(point.x,
                                                           Mathf.Lerp(weapon.WeaponHolder.WeaponFirePosition.y,
                                                                      target.BodyOriginPosition.y,
                                                                      i / (float)effectLineCurveSegments),
                                                           point.y) + offset);
            }
        }

    }

}
