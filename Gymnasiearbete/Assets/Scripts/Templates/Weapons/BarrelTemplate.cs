using UnityEngine;

namespace ArenaShooter.Templates.Weapons
{

    [CreateAssetMenu(menuName = "Templates/Weapons/Barrel")]
    class BarrelTemplate : WeaponPartTemplate
    {

        [Header("Stats")]
        [SerializeField] private ushort range;
        [SerializeField] private ushort accuracy;

        public override WeaponTemplateType type
        {
            get
            {
                return WeaponTemplateType.Barrel;
            }
        }

        public ushort Range
        {
            get
            {
                return range;
            }
        }

        public ushort Accuracy
        {
            get
            {
                return accuracy;
            }
        }

    }

}
