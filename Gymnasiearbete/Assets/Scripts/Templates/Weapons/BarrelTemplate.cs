using UnityEngine;

namespace ArenaShooter.Templates.Weapons
{

    [CreateAssetMenu(menuName = "Templates/Weapons/Barrel")]
    class BarrelTemplate : WeaponPartTemplate
    {

        [Header("Stats")]
        [SerializeField] private float range;
        [SerializeField] private float maxDistance;
        [SerializeField] private float damageMulitplier;

        public override WeaponTemplateType type
        {
            get
            {
                return WeaponTemplateType.Barrel;
            }
        }

        #region getters
        public float Range
        {
            get
            {
                return range;
            }
        }

        public float MaxDistance
        {
            get
            {
                return maxDistance;
            }
               
        }
        
        public float DamageMultiplier
        {
            get
            {
                return damageMulitplier;
            }
        }


        #endregion


    }

}
