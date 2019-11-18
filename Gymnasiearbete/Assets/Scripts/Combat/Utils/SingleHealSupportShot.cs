using ArenaShooter.Controllers;
using ArenaShooter.Entities;
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
            effectLineRenderer.SetPosition(0, weapon.WeaponHolder.WeaponFirePosition);
            effectLineRenderer.SetPosition(1, target.BodyOriginPosition);
        }

    }

}
