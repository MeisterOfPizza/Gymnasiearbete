using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Templates.Weapons
{

    [CreateAssetMenu(menuName = "Templates/Weapons/Barrel")]
    class BarrelTemplate : WeaponPartTemplate
    {

        [Header("Stats")]
        [Help(@"range and maxDistance has different uses depending on what type of weapon you're using:

* If you're using a raycast weapon, only the range is used.
* Projectile weapons can shoot projectiles up to maxDistance but the explosion radius is only as wide as range.
* Electric weapons can hit its first target at the distance of maxDistance but only jump from target to target that are within range.
* Support weapons can only support targets that are within range."
)]

        [SerializeField] private float range       = 10f;
        [SerializeField] private float maxDistance = 50f;

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
