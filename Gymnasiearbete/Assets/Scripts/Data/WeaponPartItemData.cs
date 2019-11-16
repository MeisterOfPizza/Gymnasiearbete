using ArenaShooter.Combat;
using ArenaShooter.Controllers;
using ArenaShooter.Templates.Items;
using ArenaShooter.Templates.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArenaShooter.Data
{

    [Serializable]
    class WeaponPartItemData
    {

        private ushort          weaponPartTemplateId;
        private byte            weaponPartItemRarity;
        private byte            weaponPartTemplateType;
        private StatTypeValue[] statTypeValues;

        #region Classes

        [Serializable]
        private struct StatTypeValue
        {

            public short statType;
            public float value;

            public StatTypeValue(KeyValuePair<StatType, float> statTypeValueKVP)
            {
                statType = (short)statTypeValueKVP.Key;
                value    = statTypeValueKVP.Value;
            }

            public KeyValuePair<StatType, float> ToKeyValuePair()
            {
                return new KeyValuePair<StatType, float>((StatType)statType, value);
            }

        }

        #endregion

        public WeaponPartItemData(WeaponPartItemWrapper weaponPartItem)
        {
            this.weaponPartTemplateId   = weaponPartItem.BaseTemplate.TemplateId;
            this.weaponPartItemRarity   = (byte)weaponPartItem.Rarity;
            this.weaponPartTemplateType = (byte)weaponPartItem.BaseTemplate.Type;

            this.statTypeValues = new StatTypeValue[weaponPartItem.StatTypeValues.Count];

            int i = 0;
            foreach (var kvp in weaponPartItem.StatTypeValues)
            {
                this.statTypeValues[i++] = new StatTypeValue(kvp);
            }
        }

        public WeaponPartItemWrapper CreateWeaponPartItem()
        {
            switch ((WeaponPartTemplateType)weaponPartTemplateType)
            {
                case WeaponPartTemplateType.Stock:
                    return new WeaponPartItem<StockTemplate>(
                        (WeaponPartItemRarity)weaponPartItemRarity,
                        WeaponController.Singleton.GetStockTemplate(weaponPartTemplateId),
                        new Dictionary<StatType, float>(statTypeValues.Select(s => s.ToKeyValuePair()).ToDictionary(s => s.Key, s => s.Value))
                        );
                case WeaponPartTemplateType.Body:
                    return new WeaponPartItem<BodyTemplate>(
                        (WeaponPartItemRarity)weaponPartItemRarity,
                        WeaponController.Singleton.GetBodyTemplate(weaponPartTemplateId),
                        new Dictionary<StatType, float>(statTypeValues.Select(s => s.ToKeyValuePair()).ToDictionary(s => s.Key, s => s.Value))
                        );
                case WeaponPartTemplateType.Barrel:
                    return new WeaponPartItem<BarrelTemplate>(
                        (WeaponPartItemRarity)weaponPartItemRarity,
                        WeaponController.Singleton.GetBarrelTemplate(weaponPartTemplateId),
                        new Dictionary<StatType, float>(statTypeValues.Select(s => s.ToKeyValuePair()).ToDictionary(s => s.Key, s => s.Value))
                        );
                default:
                    return null;
            }
        }

    }

}
