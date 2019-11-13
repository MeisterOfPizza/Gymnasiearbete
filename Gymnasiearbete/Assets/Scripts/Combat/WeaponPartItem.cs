using ArenaShooter.Templates.Items;
using ArenaShooter.Templates.Weapons;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArenaShooter.Combat
{

    sealed class WeaponPartItem<T> : WeaponPartItemWrapper where T : WeaponPartTemplate
    {

        public T Template { get; private set; }

        public override WeaponPartTemplate BaseTemplate
        {
            get
            {
                return Template;
            }
        }

        public WeaponPartItem(WeaponPartItemRarity rarity, T weaponPartTemplate)
        {
            this.Rarity         = rarity;
            this.Template       = weaponPartTemplate;
            this.StatTypeValues = weaponPartTemplate.GetStatTypeValues();
        }

        public WeaponPartItem(WeaponPartItemRarity rarity, T weaponPartTemplate, Dictionary<StatType, float> overridingStatTypeValues)
        {
            this.Rarity         = rarity;
            this.Template       = weaponPartTemplate;
            this.StatTypeValues = weaponPartTemplate.GetStatTypeValues(); // Get the default stat type values in case not all default values are present in the new dictionary.

            foreach (var kvp in overridingStatTypeValues.ToDictionary(o => o.Key, o => o.Value))
            {
                overridingStatTypeValues[kvp.Key] = kvp.Value;
            }
        }

        public override float GetFloat(StatType statType)
        {
            return StatTypeValues[statType];
        }

        public override int GetInt(StatType statType)
        {
            return Mathf.RoundToInt(StatTypeValues[statType]);
        }

        public override short GetShort(StatType statType)
        {
            return (short)Mathf.RoundToInt(StatTypeValues[statType]);
        }

        public override ushort GetUshort(StatType statType)
        {
            return (ushort)Mathf.Clamp(Mathf.RoundToInt(StatTypeValues[statType]), 0, ushort.MaxValue);
        }

        public override sbyte GetSbyte(StatType statType)
        {
            return (sbyte)Mathf.Clamp(Mathf.RoundToInt(StatTypeValues[statType]), 0, sbyte.MaxValue);
        }

    }

}
