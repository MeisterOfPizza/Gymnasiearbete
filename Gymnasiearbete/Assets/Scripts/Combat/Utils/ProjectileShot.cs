using UnityEngine;

namespace ArenaShooter.Combat.Utils
{

    abstract class ProjectileShot : MonoBehaviour
    {

        #region Protected variables

        protected ProjectileWeapon weapon;

        #endregion

        public void Initialize(ProjectileWeapon weapon)
        {
            this.weapon = weapon;

            OnInitialized();
        }

        protected virtual void OnInitialized()
        {
            // Leave blank.
        }

        public abstract void FireProjectile(bool clientIsShooter);

    }

}
