using UnityEngine;

namespace ArenaShooter.Templates.Weapons
{
    enum FiringMode : byte
    {
        single,
        burst,
        automatic
    }
    [CreateAssetMenu(menuName = "Templates/Weapons/Body")]
    class BodyTemplate : WeaponPartTemplate
    {
        
        [Header("Stats")]
        [SerializeField] private ushort damage = 10;
        [SerializeField] private short  maxAmmoPerClip = 30;
        [SerializeField] private short  maxAmmoStock = 150;
        [SerializeField] private short  ammoPerFire = 1;
       

        [SerializeField] private GameObject firePrefab;

        [SerializeField] private float fireCooldown = 0.1f;


        #region getters

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

        public GameObject FirePrefab
        {
            get
            {
                return firePrefab;
            }
        }

        public float FireCooldown
        {
            get
            {
                return fireCooldown;
            }
        }


        #endregion



        public override WeaponTemplateType type
        {
            get
            {
                return WeaponTemplateType.Body;
            }
        }

    }

}
