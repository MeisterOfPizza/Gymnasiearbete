
using UnityEngine;

namespace ArenaShooter.Templates.Weapons
{
    [CreateAssetMenu(menuName = "Templates/Weapons/Body")]
    class BodyTemplate : WeaponPartTemplate
    {
        [Header("Stats")]
        [SerializeField] private ushort damage;
       
        public ushort Damage
        {
            get
            {
                return damage;
            }
        }

        public override WeaponTemplateType type
        {
            get
            {
                return WeaponTemplateType.body;
            }
        }

        
    }
}

