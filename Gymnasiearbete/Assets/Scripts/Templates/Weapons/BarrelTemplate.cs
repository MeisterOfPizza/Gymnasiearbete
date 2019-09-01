using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Templates.Weapons
{

    [CreateAssetMenu(menuName = "Templates/Weapons/Barrel")]
    class BarrelTemplate : WeaponPartTemplate
    {

        [Header("Stats")]
        [SerializeField] private float range            = 10f;
        [SerializeField] private float maxDistance      = 50f;
        [SerializeField] private float damageMulitplier = 1f;

        #region Getters

        public override WeaponPartTemplateType Type
        {
            get
            {
                return WeaponPartTemplateType.Barrel;
            }
        }

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
