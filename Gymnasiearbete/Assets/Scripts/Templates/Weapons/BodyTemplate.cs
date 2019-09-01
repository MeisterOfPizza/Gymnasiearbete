using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Templates.Weapons
{

    enum FiringMode : byte
    {
        Single,
        Burst,
        Automatic
    }

    [CreateAssetMenu(menuName = "Templates/Weapons/Body")]
    class BodyTemplate : WeaponPartTemplate
    {
        
        [Header("Stats")]
        [SerializeField] private ushort damage         = 10;
        [SerializeField] private short  maxAmmoPerClip = 30;
        [SerializeField] private short  maxAmmoStock   = 150;
        [SerializeField] private short  ammoPerFire    = 1;
        [SerializeField] private float  fireCooldown   = 0.1f;
        [SerializeField] private float  reloadTime     = 1f;
        [SerializeField] private float  fullReloadTime = 1.5f;

        [Space]
        [SerializeField] private FiringMode firingMode = FiringMode.Automatic;

        [Header("Prefabs")]
        [SerializeField] private GameObject firePrefab;

        #region Getters

        public override WeaponPartTemplateType Type
        {
            get
            {
                return WeaponPartTemplateType.Body;
            }
        }

        public ushort Damage
        {
            get
            {
                return damage;
            }
        }

        public short MaxAmmoPerClip
        {
            get
            {
                return maxAmmoPerClip;
            }
        }

        public short MaxAmmoStock
        {
            get
            {
                return maxAmmoStock;
            }
        }

        public short AmmoPerFire
        {
            get
            {
                return ammoPerFire;
            }
        }

        public float FireCooldown
        {
            get
            {
                return fireCooldown;
            }
        }

        public float ReloadTime
        {
            get
            {
                return reloadTime;
            }
        }

        public float FullReloadTime
        {
            get
            {
                return fullReloadTime;
            }
        }

        public GameObject FirePrefab
        {
            get
            {
                return firePrefab;
            }
        }

        public FiringMode FiringMode
        {
            get
            {
                return firingMode;
            }
        }

        #endregion

    }

}
