using ArenaShooter.Extensions;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Combat
{

    sealed class ProjectileWeapon : Weapon
    {

        #region Editor

        [Header("References")]
        [SerializeField] private GameObject projectile; // TODO: Replace with template's firePrefab.

        #endregion

        #region Private variables

        private GameObjectPool<Projectile> projectiles;

        #endregion

        private void Awake()
        {
            // TODO: Create projectiles.
            projectiles = new GameObjectPool<Projectile>(null, projectile, MaxAmmoStock + MaxAmmoClip);

            foreach (var projectile in projectiles.PooledItems)
            {
                projectile.Initialize(this, ProjectileHit);
            }
        }

        public override void Fire()
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
