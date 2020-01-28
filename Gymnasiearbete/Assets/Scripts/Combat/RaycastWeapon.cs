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
            hitEffect = Instantiate(Stats.FirePrefab, transform).GetComponent<ParticleSystem>();
        }

        public override void EquipWeapon(IWeaponHolder weaponHolder)
        {
            base.EquipWeapon(weaponHolder);

            var collisionModule = hitEffect.collision;
            collisionModule.collidesWith = WeaponHolder.WeaponHitLayerMask | LayerMask.GetMask("Enemy VFX");
        }

        protected override void OnFire()
        {
            
            float offset = Random.Range(-1f, 1f) * (1-Stats.Accuracy) * Mathf.PI * Stats.MaxAngleOffset / 180f;

           
            float angle = 450f - Quaternion.LookRotation(WeaponHolder.WeaponForward).eulerAngles.y;//360 - y + 90
            Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.PI / 180f + offset), 0f, Mathf.Sin(angle * Mathf.PI / 180f + offset));
           
            var ray = new Ray(WeaponHolder.WeaponFirePosition,dir);
            var hit = Extensions.Utils.Raycast(ray, Stats.Range, WeaponHolder.WeaponHitLayerMask, WeaponHolder.gameObject, QueryTriggerInteraction.Ignore);

            
            if (hit.NetworkHit)
            {
                var takeDamageEvent         = TakeDamageEvent.Create(GlobalTargets.Everyone, ReliabilityModes.ReliableOrdered);
                takeDamageEvent.Target      = hit.Hitbox.GetComponent<IEntity>().entity;
                takeDamageEvent.Shooter     = WeaponHolder.entity;
                takeDamageEvent.DamageTaken = CalculateDamage(hit.Distance);
                takeDamageEvent.Send();
            }

            var fireEvent     = WeaponFireEffectEvent.Create(WeaponHolder.entity, EntityTargets.EveryoneExceptOwner);
            fireEvent.Shooter = WeaponHolder.entity;
            fireEvent.Point   = ray.origin;
            fireEvent.Forward = ray.direction;
            fireEvent.Send();

            OnEvent(fireEvent);
        }

        public override void OnEvent(WeaponFireEffectEvent @event)
        {
            hitEffect.Emit(new ParticleSystem.EmitParams() { position = @event.Point, velocity = @event.Forward * hitEffect.main.startSpeed.constant }, 1);
        }

    }

}
