using ArenaShooter.Entities;
using ArenaShooter.Extensions;
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

        private void Start()
        {
            hitEffect = Instantiate(bodyTemplate.FirePrefab, transform).GetComponent<ParticleSystem>();
        }

        protected override void OnFire()
        {
            var hit = Utils.Raycast(new Ray(WeaponHolder.WeaponFirePosition, WeaponHolder.WeaponForward), Range, WeaponHolder.WeaponHitLayerMask, WeaponHolder.gameObject, QueryTriggerInteraction.Ignore);

            if (hit.HitAnything)
            {
                if (hit.NetworkHit)
                {
                    var takeDamageEvent         = TakeDamageEvent.Create(GlobalTargets.Everyone, ReliabilityModes.ReliableOrdered);
                    takeDamageEvent.Target      = hit.Hitbox.GetComponent<IEntity>().entity;
                    takeDamageEvent.DamageTaken = CalculateDamage(hit.Distance);
                    takeDamageEvent.Send();
                }

                var fireEvent = WeaponRaycastFireEffectEvent.Create(WeaponHolder.entity, EntityTargets.EveryoneExceptOwner);
                fireEvent.Shooter = WeaponHolder.entity;
                fireEvent.Point   = hit.HitPoint;
                fireEvent.Up      = hit.HitNormal;
                fireEvent.Send();

                PlayHitEffect(hit.HitPoint, hit.HitNormal);
            }
        }

        public void PlayHitEffect(Vector3 position, Vector3 up)
        {
            hitEffect.transform.position = position;
            hitEffect.transform.up       = up;
            hitEffect.Play(true);
        }

    }

}
