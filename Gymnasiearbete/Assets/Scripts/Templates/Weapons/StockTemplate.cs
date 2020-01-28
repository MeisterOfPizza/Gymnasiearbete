using ArenaShooter.Templates.Items;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Templates.Weapons
{

    [CreateAssetMenu(menuName = "Templates/Weapons/Stock")]
    class StockTemplate : WeaponPartTemplate
    {

        [Header("Stats")]
        [SerializeField]                 private float mobility;
        [SerializeField, Range(0f, 1f)]  private float accuracy       = 0.75f;
        [SerializeField, Range(0f, 90f)] private float maxAngleOffset = 5f;

        #region Getters

        public override WeaponPartTemplateType Type
        {
            get
            {
                return WeaponPartTemplateType.Stock;
            }
        }

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

        public float MaxAngleOffset
        {
            get
            {
                return maxAngleOffset;
            }
        }

        #endregion

        #region Helpers

        public override Dictionary<StatType, float> GetStatTypeValues()
        {
            return new Dictionary<StatType, float>()
            {
                { StatType.Mobility, mobility },
                { StatType.Accuracy, accuracy },
                { StatType.MaxAngleOffset, maxAngleOffset }
            };
        }

        #endregion

    }

}
