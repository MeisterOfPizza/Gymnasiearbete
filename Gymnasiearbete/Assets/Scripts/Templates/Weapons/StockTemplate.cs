using UnityEngine;

namespace ArenaShooter.Templates.Weapons
{

    [CreateAssetMenu(menuName = "Templates/Weapons/Stock")]
    class StockTemplate : WeaponPartTemplate
    {

        [Header("Stats")]
        [SerializeField] private float  mobility;
        [SerializeField] private float  accuracy;

        public override WeaponTemplateType type
        {
            get
            {
                return WeaponTemplateType.Stock;
            }
        }

        #region getters
        public float Mobility
        {
            get
            {
                return mobility;
            }
        }

        public float Accuracy
        {
            get
            {
                return accuracy;
            }
        }
        #endregion


    }

}
