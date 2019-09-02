using ArenaShooter.Entities;
using Bolt;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Combat
{

    sealed class RaycastWeapon : Weapon
    {

        #region Private variables

        private ParticleSystem hitEffect;

        #endregion

        protected override void OnInitialized()
        {
            hitEffect = Instantiate(BodyTemplate.FirePrefab, transform).GetComponent<ParticleSystem>();
        }

        protected override void OnFire()
        {
            var hit = Extensions.Utils.Raycast(new Ray(WeaponHolder.WeaponFirePosition, WeaponHolder.WeaponForward), BarrelTemplate.Range, WeaponHolder.WeaponHitLayerMask, WeaponHolder.gameObject, QueryTriggerInteraction.Ignore);

            if (hit.HitAnything)
            {
                if (hit.NetworkHit)
                {
                    var takeDamageEvent         = TakeDamageEvent.Create(GlobalTargets.Everyone, ReliabilityModes.ReliableOrdered);
                    takeDamageEvent.Target      = hit.Hitbox.GetComponent<IEntity>().entity;
                    takeDamageEvent.DamageTaken = CalculateDamage(hit.Distance);
                    takeDamageEvent.Send();
                }

                var fireEvent     = WeaponFireEffectEvent.Create(WeaponHolder.entity, EntityTargets.EveryoneExceptOwner);
                fireEvent.Shooter = WeaponHolder.entity;
                fireEvent.Point   = hit.HitPoint;
                fireEvent.Up      = hit.HitNormal;
                fireEvent.Send();

                OnEvent(fireEvent);
            }
        }

        public override void OnEvent(WeaponFireEffectEvent @event)
        {
            hitEffect.transform.position = @event.Point;
            hitEffect.transform.up       = @event.Up;
            hitEffect.Play(true);
        }

    }

}
