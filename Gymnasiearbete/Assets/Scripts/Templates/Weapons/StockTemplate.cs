using UnityEngine;

namespace ArenaShooter.Templates.Weapons
{

    [CreateAssetMenu(menuName = "Templates/Weapons/Stock")]
    class StockTemplate : WeaponPartTemplate
    {

        [Header("Stats")]
        [SerializeField] private ushort movementSpeed;
        [SerializeField] private ushort stability;

        public override WeaponTemplateType type
        {
            get
            {
                return WeaponTemplateType.Stock;
            }
        }

        public ushort MovmentSpeed
        {
            get
            {
                return movementSpeed;
            }
        }

        public ushort Stability
        {
            get
            {
                return stability;
            }
        }

    }

}
