using ArenaShooter.Templates.Items;
using ArenaShooter.Templates.Weapons;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArenaShooter.Combat
{

    sealed class WeaponPartItem<T> : WeaponPartItemWrapper where T : WeaponPartTemplate
    {

        #region Private static variables

        private static string PositiveStatChange = "#" + ColorUtility.ToHtmlStringRGBA(new Color32(129, 255, 061, 255));
        private static string NegativeStatChange = "#" + ColorUtility.ToHtmlStringRGBA(new Color32(252, 059, 025, 255));

        #endregion

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

            // Create the values delta dictionary.
            this.StatTypeValuesDelta = StatTypeValues.ToDictionary(s => s.Key, s => (sbyte)0);
        }

        public WeaponPartItem(WeaponPartItemRarity rarity, T weaponPartTemplate, Dictionary<StatType, float> overridingStatTypeValues)
        {
            this.Rarity         = rarity;
            this.Template       = weaponPartTemplate;
            this.StatTypeValues = weaponPartTemplate.GetStatTypeValues(); // Get the default stat type values in case not all default values are present in the new dictionary.

            // Create the values delta dictionary.
            this.StatTypeValuesDelta = StatTypeValues.ToDictionary(s => s.Key, s => (sbyte)0);

            UpdateStatTypeValueDeltas(overridingStatTypeValues);

            foreach (var kvp in overridingStatTypeValues)
            {
                StatTypeValues[kvp.Key] = kvp.Value;
            }

            ValidateStatTypeValues(StatTypeValues);
        }

        public override float GetFloat(StatType statType)
        {
            return StatTypeValues[statType];
        }

        public override int GetInt(StatType statType)
        {
            return (int)StatTypeValues[statType];
        }

        public override short GetShort(StatType statType)
        {
            return (short)StatTypeValues[statType];
        }

        public override ushort GetUshort(StatType statType)
        {
            return (ushort)Mathf.Clamp(StatTypeValues[statType], 0, ushort.MaxValue);
        }

        public override sbyte GetSbyte(StatType statType)
        {
            return (sbyte)Mathf.Clamp(StatTypeValues[statType], 0, sbyte.MaxValue);
        }

        public override string GetStatsFormatted()
        {
            string formatted = "";

            foreach (var kvp in StatTypeValues)
            {
                string color = StatTypeValuesDelta[kvp.Key] != 0 ? (StatTypeValuesDelta[kvp.Key] == 1 ? PositiveStatChange : NegativeStatChange) : "#ffffff";

                formatted += WeaponPartTemplate.GetStatTypeValueFormatted(kvp.Key, kvp.Value, color) + "\n";
            }

            return formatted;
        }

    }

}
