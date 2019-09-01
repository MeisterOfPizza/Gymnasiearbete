using ArenaShooter.Controllers;
using ArenaShooter.Entities;
using Bolt;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Combat
{

    class SupportWeapon : Weapon
    {

        #region Editor

        [Header("References")]
        [SerializeField] private LineRenderer lineRenderer; // TEST: Test data.

        #endregion

        #region Private variables

        private IEntity target;

        #endregion

        protected override void OnBeforeFireFirstTime()
        {
            target = PlayerEntityController.Singleton.GetClosestPlayer(WeaponHolder.WeaponFirePosition, Range);

            lineRenderer.gameObject.SetActive(target != null);
            UpdateLineRenderer();
        }

        protected override void OnFireIsOnCooldown()
        {
            if (IsPlayerHoldingFire)
            {
                UpdateLineRenderer();
            }
        }

        protected override void OnFire()
        {
            if (IsPlayerHoldingFire)
            {
                float distance = Vector3.Distance(WeaponHolder.WeaponFirePosition, target.entity.transform.position);
                if (distance < Range)
                {
                    var healEvent    = HealEvent.Create(GlobalTargets.Everyone, ReliabilityModes.ReliableOrdered);
                    healEvent.Target = target.entity;
                    healEvent.Heal   = CalculateDamage(distance);
                    healEvent.Send();

                    UpdateLineRenderer();
                }
                else
                {
                    target = null;
                    lineRenderer.gameObject.SetActive(false);
                }
            }
        }

        private void UpdateLineRenderer()
        {
            if (target != null)
            {
                lineRenderer.SetPosition(0, WeaponHolder.WeaponFirePosition);
                lineRenderer.SetPosition(1, target.entity.transform.position);
            }
        }

    }

}
