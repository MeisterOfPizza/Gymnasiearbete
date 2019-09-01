using ArenaShooter.Entities;
using ArenaShooter.Templates.Weapons;
using System;
using System.Collections;
using UnityEngine;

namespace ArenaShooter.Combat
{

    /// <summary>
    /// Weapon object to be used by players ONLY.
    /// </summary>
    abstract class Weapon : MonoBehaviour
    {

        #region Public properties

        // Callbacks //

        public Action<string> OnAmmoChangedCallback    { get; set; }
        public Action         OnReloadBegunCallback    { get; set; }
        public Action         OnReloadFinishedCallback { get; set; }
        public Action         OnReloadCanceledCallback { get; set; }

        // Pre-use calculations done to increase execution times //

        public int Damage { get; private set; }

        // Temporary properties //

        public int   AmmoLeftInStock     { get; private set; }
        public int   AmmoLeftInClip      { get; private set; }
        public float CurrentFireCooldown { get; private set; }

        // Helpers //

        public IWeaponHolder WeaponHolder { get; private set; }

        public float Range
        {
            get
            {
                return barrelTemplate.Range;
            }
        }

        public float MaxDistance
        {
            get
            {
                return barrelTemplate.MaxDistance;
            }
        }

        public WeaponPartTemplateOutputType OutputType
        {
            get
            {
                return stockTemplate.OutputType;
            }
        }

        #endregion

        #region Protected properties

        /// <summary>
        /// This virtual property can be used to check some statements to see if the weapon really can fire.
        /// </summary>
        protected virtual bool WeaponCanFire
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Protected variables

        protected StockTemplate  stockTemplate;
        protected BodyTemplate   bodyTemplate;
        protected BarrelTemplate barrelTemplate;

        #endregion

        #region Private variables

        private bool weaponIsFiring;
        private bool isReloading;

        #endregion

        #region Initializing

        public void Initialize(StockTemplate stockTemplate, BodyTemplate bodyTemplate, BarrelTemplate barrelTemplate)
        {
            this.stockTemplate  = stockTemplate;
            this.bodyTemplate   = bodyTemplate;
            this.barrelTemplate = barrelTemplate;

            this.Damage = (int)(bodyTemplate.Damage * barrelTemplate.DamageMultiplier);

            this.AmmoLeftInStock = bodyTemplate.MaxAmmoStock;
            this.AmmoLeftInClip  = bodyTemplate.MaxAmmoPerClip;
        }

        #endregion

        #region Updating

        private void Update()
        {
            CurrentFireCooldown -= Time.deltaTime;

            WeaponUpdate();
        }

        private void LateUpdate()
        {
            WeaponLateUpdate();
        }

        protected virtual void WeaponUpdate()
        {
            // Leave blank.
        }

        protected virtual void WeaponLateUpdate()
        {
            // Leave blank.
        }

        #endregion

        #region Equipping

        public virtual void EquipWeapon(IWeaponHolder weaponHolder)
        {
            this.WeaponHolder = weaponHolder;

            OnAmmoChangedCallback?.Invoke(FormatAmmoLeft());
        }

        public virtual void UnequipWeapon()
        {

        }

        #endregion

        #region Calculations

        public virtual int CalculateDamage(float range)
        {
            return (int)(Damage * (1 - Mathf.Clamp01(range / barrelTemplate.Range)));
        }

        #endregion

        #region Reloading

        /// <summary>
        /// Begins the reload action and starts the reload animation.
        /// </summary>
        public void Reload()
        {
            if (!isReloading)
            {
                isReloading = true;

                StopFiring();

                StartCoroutine("ReloadAction");
            }
        }

        /// <summary>
        /// Cancels the reload and reload animation.
        /// </summary>
        public void CancelReload()
        {
            if (isReloading)
            {
                isReloading = false;

                // TODO: Reset reload animation:
                StopCoroutine("ReloadAction");

                OnReloadCanceledCallback?.Invoke();
            }
        }

        /// <summary>
        /// Contains logic for reloading.
        /// Do NOT calls this to reload, call <see cref="Reload"/> instead.
        /// Do NOT stop this coroutine to cancel a reload, call <see cref="CancelReload"/> instead.
        /// </summary>
        private IEnumerator ReloadAction()
        {
            OnReloadBegunCallback?.Invoke();

            float reloadTimeLeft = AmmoLeftInClip == 0 ? bodyTemplate.FullReloadTime : bodyTemplate.ReloadTime;

            while (reloadTimeLeft > 0f && isReloading)
            {
                yield return new WaitForEndOfFrame();

                reloadTimeLeft -= Time.deltaTime;
            }

            if (isReloading)
            {
                int unsedAmmo   = AmmoLeftInClip;
                AmmoLeftInClip  = Mathf.Min(bodyTemplate.MaxAmmoPerClip, AmmoLeftInStock);
                AmmoLeftInStock = Mathf.Clamp(AmmoLeftInStock - bodyTemplate.MaxAmmoPerClip - unsedAmmo, 0, bodyTemplate.MaxAmmoStock);

                isReloading = false;

                OnAmmoChangedCallback?.Invoke(FormatAmmoLeft());
                OnReloadFinishedCallback?.Invoke();
            }
        }

        #endregion

        #region Firing

        public void CheckForInput()
        {
            if (!isReloading)
            {
                bool shouldFire = false;

                // TODO: Check for mobile input:
#if UNITY_STANDALONE
                switch (bodyTemplate.FiringMode)
                {
                    case FiringMode.Single:
                    case FiringMode.Burst:
                        shouldFire = Input.GetMouseButtonDown(0);
                        break;
                    case FiringMode.Automatic:
                        shouldFire = Input.GetMouseButton(0);
                        break;
                }
#elif UNITY_IOS || UNITY_ANDROID

#endif

                if (shouldFire)
                {
                    if (!weaponIsFiring)
                    {
                        OnBeginFire();
                    }

                    TryFiring();

                    OnFireFrame();
                }
                else if (weaponIsFiring)
                {
                    StopFiring();
                }
            }
        }

        public void StopFiring()
        {
            if (weaponIsFiring)
            {
                OnEndFire();
            }

            weaponIsFiring = false;
        }

        private void TryFiring()
        {
            if (AmmoLeftInClip - bodyTemplate.AmmoPerFire >= 0 && WeaponCanFire)
            {
                if (CurrentFireCooldown <= 0f)
                {
                    // Remove used ammo:
                    AmmoLeftInClip -= bodyTemplate.AmmoPerFire;

                    // Invoke ammo spent callback for UI:
                    OnAmmoChangedCallback?.Invoke(FormatAmmoLeft());

                    // Reset the cooldown:
                    CurrentFireCooldown = bodyTemplate.FireCooldown;

                    weaponIsFiring = true;

                    // Make the weapon actually fire:
                    OnFire();
                }
            }
            else
            {
                StopFiring();
                OnFailedToFire();

                if (AmmoLeftInClip <= 0)
                {
                    // Auto reload:
                    Reload();
                }
            }
        }

        /// <summary>
        /// Is called the same frame that the weapon holder fires and succeeds.
        /// </summary>
        protected abstract void OnFire();

        /// <summary>
        /// Is called whenever the weapon holder stars firing.
        /// This method is called before <see cref="OnFire"/> is called.
        /// </summary>
        protected virtual void OnBeginFire()
        {
            // Leave blank.
        }

        /// <summary>
        /// Is called every frame that the weapon holder is trying to fire.
        /// This method is called right after <see cref="OnFire"/> is called.
        /// </summary>
        protected virtual void OnFireFrame()
        {
            // Leave blank.
        }

        /// <summary>
        /// Is called whenever the weapon holder stops firing.
        /// This method is called after <see cref="OnFire"/> is called.
        /// </summary>
        protected virtual void OnEndFire()
        {
            // Leave blank.
        }

        /// <summary>
        /// Is called whenever the weapon holder tries to fire but cannot.
        /// This method is called after <see cref="OnEndFire"/> is called.
        /// </summary>
        protected virtual void OnFailedToFire()
        {
            // Leave blank.
        }

        #endregion

        #region Helpers

        protected virtual string FormatAmmoLeft()
        {
            return string.Format("{0}/{1} | {2}", AmmoLeftInClip, bodyTemplate.MaxAmmoPerClip, AmmoLeftInStock);
        }

        #endregion

    }

}
