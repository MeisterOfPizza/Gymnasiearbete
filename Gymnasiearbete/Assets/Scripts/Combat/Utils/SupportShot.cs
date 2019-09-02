using UnityEngine;

namespace ArenaShooter.Combat.Utils
{

    abstract class SupportShot : MonoBehaviour
    {

        #region Public properties

        public abstract bool WeaponCanFire   { get; }
        public abstract bool HasValidTargets { get; }

        #endregion

        #region Protected variables

        protected SupportWeapon weapon;

        #endregion

        public void Initialize(SupportWeapon weapon)
        {
            this.weapon = weapon;

            OnInitialized();
        }

        protected virtual void OnInitialized()
        {
            // Leave blank.
        }

        public abstract void OnBeginSupport();
        public abstract void OnEndSupport();
        public abstract void SupportTargets();
        public abstract void OnEvent(WeaponSupportBeginFireEffectEvent @event);
        public abstract void UpdateSupportShot();

    }

}
