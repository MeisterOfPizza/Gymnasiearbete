using ArenaShooter.Controllers;
using ArenaShooter.Extensions;

#pragma warning disable 0649

namespace ArenaShooter.Combat
{

    sealed class ProjectileWeapon : Weapon
    {

        #region Private variables

        private GameObjectPool<Projectile> projectiles;

        #endregion

        private void Start()
        {
            projectiles = new GameObjectPool<Projectile>(WeaponController.Singleton.ProjectileContainer, bodyTemplate.FirePrefab, bodyTemplate.MaxAmmoStock + bodyTemplate.MaxAmmoPerClip);

            foreach (var projectile in projectiles.PooledItems)
            {
                projectile.Initialize(this, ProjectileHit);
            }
        }

        protected override void OnFire()
        {
            var fireEvent = WeaponProjectileFireEvent.Create(WeaponHolder.entity);
            fireEvent.Shooter  = WeaponHolder.entity;
            fireEvent.Position = WeaponHolder.WeaponFirePosition;
            fireEvent.Forward  = WeaponHolder.WeaponForward;
            fireEvent.Send();
        }
        
        public void FireProjectileEffect(WeaponProjectileFireEvent fireEvent)
        {
            Projectile projectile = projectiles.GetItem();

            if (projectile != null)
            {
                projectile.transform.position = fireEvent.Position;
                projectile.transform.forward  = fireEvent.Forward;
                projectile.FireProjectile(fireEvent.FromSelf);
            }
        }

        private void ProjectileHit(Projectile projectile)
        {
            projectiles.PoolItem(projectile);
        }

    }

}
