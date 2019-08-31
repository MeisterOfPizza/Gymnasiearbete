using ArenaShooter.Entities;
using UnityEngine;

namespace ArenaShooter.Combat
{

    /// <summary>
    /// Weapon object to be used by players ONLY.
    /// </summary>
    abstract class Weapon : MonoBehaviour
    {

        #region Public variables

        public int   Damage       { get; private set; } = 10; // TEST: Test data.
        public float Range        { get; private set; } = 10f; // TEST: Test data.
        public float MaxDistance  { get; private set; } = 100f; // TEST: Test data.
        public float Accuracy     { get; private set; }
        public float Mobility     { get; private set; }
        public float FireCooldown { get; private set; }
        public int   MaxAmmoStock { get; private set; } = 30; // TEST: Test data.
        public int   MaxAmmoClip  { get; private set; } = 5; // TEST: Test data.
        public int   AmmoPerFire  { get; private set; }

        public int AmmoLeft { get; set; }

        public IWeaponHolder WeaponHolder { get; private set; }

        #endregion

        public virtual void EquipWeapon(IWeaponHolder weaponHolder)
        {
            this.WeaponHolder = weaponHolder;
        }

        public virtual void UnequipWeapon()
        {

        }

        public virtual int CalculateDamage(float range)
        {
            return (int)(Damage * (1 - Mathf.Clamp01(range / Range)));
        }

        public abstract void Fire();

    }

}
