using ArenaShooter.Entities;
using UnityEngine;

namespace ArenaShooter.Combat
{

    /// <summary>
    /// Weapon object to be used by players ONLY.
    /// </summary>
    abstract class Weapon : MonoBehaviour
    {

        #region Public properties

        public int   Damage       { get; private set; } = 10; // TEST: Test data.
        public float Range        { get; private set; } = 10f; // TEST: Test data.
        public float MaxDistance  { get; private set; } = 100f; // TEST: Test data.
        public float Accuracy     { get; private set; }
        public float Mobility     { get; private set; }
        public float FireCooldown { get; private set; } = 0.1f; // TEST: Test data.
        public int   MaxAmmoStock { get; private set; } = 30; // TEST: Test data.
        public int   MaxAmmoClip  { get; private set; } = 5; // TEST: Test data.
        public int   AmmoPerFire  { get; private set; }

        public int   AmmoLeftInStock     { get; private set; } = 2;
        public int   AmmoLeftInClip      { get; private set; } = 2;
        public float CurrentFireCooldown { get; private set; }

        public IWeaponHolder WeaponHolder { get; private set; }

        public bool IsPlayerHoldingFire
        {
            get
            {
                return firedWeaponLastFrame;
            }
        }

        #endregion

        #region Private variables

        private bool firedWeaponThisFrame;
        private bool firedWeaponLastFrame;

        #endregion

        private void Update()
        {
            CurrentFireCooldown -= Time.deltaTime;
        }

        private void LateUpdate()
        {
            firedWeaponLastFrame = firedWeaponThisFrame;
            firedWeaponThisFrame = false;
        }

        public virtual void EquipWeapon(IWeaponHolder weaponHolder)
        {
            this.WeaponHolder = weaponHolder;
        }

        public virtual void UnequipWeapon()
        {

        }

        #region Calculations

        public virtual int CalculateDamage(float range)
        {
            return (int)(Damage * (1 - Mathf.Clamp01(range / Range)));
        }

        #endregion

        #region Reloading

        public void Reload()
        {
            int unsedAmmo = AmmoLeftInClip;
            AmmoLeftInClip = Mathf.Min(MaxAmmoClip, AmmoLeftInStock);
            AmmoLeftInStock = Mathf.Clamp(AmmoLeftInStock - MaxAmmoClip - unsedAmmo, 0, MaxAmmoStock);
        }

        #endregion

        #region Firing

        public void Fire()
        {
            if (!firedWeaponLastFrame)
            {
                OnBeforeFireFirstTime();
            }

            if (AmmoLeftInClip - AmmoPerFire > 0)
            {
                if (CurrentFireCooldown <= 0f)
                {
                    AmmoLeftInClip -= AmmoPerFire;

                    CurrentFireCooldown = 0;

                    firedWeaponThisFrame = true;

                    OnFire();
                }
                else
                {
                    OnFireIsOnCooldown();
                }
            }
            else
            {
                Reload();
            }
        }

        /// <summary>
        /// Is called whenever the weapon holder fires the weapon for the first time since they stopped firing.
        /// </summary>
        protected virtual void OnBeforeFireFirstTime()
        {
            // Leave blank.
        }

        /// <summary>
        /// Is called whenever the weapon holder tries to fire the weapon while it's still in cooldown.
        /// </summary>
        protected virtual void OnFireIsOnCooldown()
        {
            // Leave blank.
        }

        protected abstract void OnFire();

        #endregion

    }

}
