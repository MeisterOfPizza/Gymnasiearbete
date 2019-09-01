using ArenaShooter.Controllers;
using ArenaShooter.Player;
using Bolt;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Combat
{

    class SupportWeapon : Weapon
    {

        #region Private variables

        private LineRenderer effectLineRenderer;

        #endregion

        #region Protected properties

        protected bool HasValidTarget
        {
            get
            {
                return target != null && target != WeaponHolder.entity;
            }
        }

        protected override bool WeaponCanFire
        {
            get
            {
                var _pcController = PlayerEntityController.Singleton.GetClosestPlayer((PlayerController)WeaponHolder, WeaponHolder.WeaponFirePosition, barrelTemplate.Range);
                var _target = _pcController == null ? null : _pcController.entity;

                return _target != null && _target != WeaponHolder.entity;
            }
        }

        #endregion

        #region Protected variables

        protected BoltEntity target;

        #endregion

        private void Start()
        {
            effectLineRenderer = Instantiate(bodyTemplate.FirePrefab, transform).GetComponent<LineRenderer>();
            effectLineRenderer.useWorldSpace = true;
        }

        protected override void WeaponUpdate()
        {
            // Check if the weapon does not belong to the local client, and if the weapon has received a target:
            if (!WeaponHolder.entity.IsControllerOrOwner && HasValidTarget)
            {
                // If so: update the graphics of the effect.
                UpdateGraphics();
            }
        }

        protected override void OnBeginFire()
        {
            var pcController = PlayerEntityController.Singleton.GetClosestPlayer((PlayerController)WeaponHolder, WeaponHolder.WeaponFirePosition, barrelTemplate.Range);
            target = pcController == null ? null : pcController.entity;

            if (HasValidTarget)
            {
                var beginFiring     = WeaponSupportBeginFireEffectEvent.Create(WeaponHolder.entity, EntityTargets.EveryoneExceptOwner);
                beginFiring.Begin   = true;
                beginFiring.Shooter = WeaponHolder.entity;
                beginFiring.Target  = target;
                beginFiring.Send();

                effectLineRenderer.gameObject.SetActive(true);
            }
            else
            {
                target = null;
            }
        }

        protected override void OnFireFrame()
        {
            if (HasValidTarget)
            {
                UpdateGraphics();
            }
        }

        protected override void OnEndFire()
        {
            var beginFiring     = WeaponSupportBeginFireEffectEvent.Create(WeaponHolder.entity, EntityTargets.EveryoneExceptOwner);
            beginFiring.Begin   = false;
            beginFiring.Shooter = WeaponHolder.entity;
            beginFiring.Send();

            target = null;
            effectLineRenderer.gameObject.SetActive(false);
        }

        protected override void OnFire()
        {
            if (HasValidTarget)
            {
                float distance = Vector3.Distance(WeaponHolder.WeaponFirePosition, target.transform.position);
                if (distance < Range)
                {
                    var healEvent    = HealEvent.Create(GlobalTargets.Everyone, ReliabilityModes.ReliableOrdered);
                    healEvent.Target = target;
                    healEvent.Heal   = Damage;
                    healEvent.Send();
                }
                else
                {
                    StopFiring();
                }
            }
        }

        private void UpdateGraphics()
        {
            effectLineRenderer.SetPosition(0, WeaponHolder.WeaponFirePosition);
            effectLineRenderer.SetPosition(1, target.transform.position);
        }

        public void BeginFiring(WeaponSupportBeginFireEffectEvent weaponSupportBeginFireEffectEvent)
        {
            target = weaponSupportBeginFireEffectEvent.Target;
            effectLineRenderer.gameObject.SetActive(weaponSupportBeginFireEffectEvent.Begin);
        }

    }

}
