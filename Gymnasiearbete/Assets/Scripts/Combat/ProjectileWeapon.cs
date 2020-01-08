using ArenaShooter.Combat.Utils;
using ArenaShooter.Controllers;
using ArenaShooter.Extensions;
using Bolt;

#pragma warning disable 0649

namespace ArenaShooter.Combat
{

    sealed class ProjectileWeapon : Weapon
    {

        #region Private variables

        private GameObjectPool<ProjectileShot> projectiles;

        #endregion

        protected override void OnInitialized()
        {
            projectiles = new GameObjectPool<ProjectileShot>(WeaponController.Singleton.ProjectileContainer, Stats.FirePrefab, Stats.MaxAmmoPerClip * 2);

            foreach (var projectile in projectiles.PooledItems)
            {
                projectile.Initialize(this);
            }
        }

        protected override void OnFire()
        {
            var fireEvent     = WeaponFireEffectEvent.Create(WeaponHolder.entity, EntityTargets.EveryoneExceptOwner);
            fireEvent.Shooter = WeaponHolder.entity;
            fireEvent.Point   = WeaponHolder.WeaponFirePosition;
            fireEvent.Forward = WeaponHolder.WeaponForward;
            fireEvent.Send();

            OnEvent(fireEvent);
        }

        public override void OnEvent(WeaponFireEffectEvent @event)
        {
            ProjectileShot projectile = projectiles.GetItem();

            if (projectile != null)
            {
                projectile.transform.position = @event.Point;
                projectile.transform.forward  = @event.Forward;
                projectile.FireProjectile(@event.Shooter.NetworkId.Equals(WeaponHolder.entity.NetworkId)); // Check if the shooter is the local client.
            }
        }

        /// <summary>
        /// Pools the provided projectile shot.
        /// </summary>
        public void ProjectileHit(ProjectileShot projectile)
        {
            projectiles.PoolItem(projectile);
        }

    }

}
