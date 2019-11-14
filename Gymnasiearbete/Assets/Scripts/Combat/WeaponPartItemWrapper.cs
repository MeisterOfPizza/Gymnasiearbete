using ArenaShooter.Templates.Items;
using ArenaShooter.Templates.Weapons;
using System.Collections.Generic;
using UnityEngine;

namespace ArenaShooter.Combat
{

    /// <summary>
    /// Wrapper class to pass weapon part items without needing to specify the generic type T of <see cref="WeaponPartItem{T}"/>.
    /// </summary>
    abstract class WeaponPartItemWrapper
    {

        #region Public static properties

        public Color32 StandardRarityColor  { get; private set; } = new Color32(199, 199, 199, 255);
        public Color32 CommonRarityColor    { get; private set; } = new Color32(255, 255, 255, 255);
        public Color32 UncommonRarityColor  { get; private set; } = new Color32(142, 224, 054, 255);
        public Color32 RareRarityColor      { get; private set; } = new Color32(095, 216, 250, 255);
        public Color32 LegendaryRarityColor { get; private set; } = new Color32(255, 248, 051, 255);
        public Color32 AncientRarityColor   { get; private set; } = new Color32(255, 031, 064, 255);

        #endregion

        public WeaponPartItemRarity Rarity { get; protected set; }

        public abstract WeaponPartTemplate BaseTemplate { get; }

        /// <summary>
        /// The stat type values merged with the template stat values.
        /// </summary>
        public Dictionary<StatType, float> StatTypeValues { get; protected set; }

        /// <summary>
        /// Value delta dictionary. Use this when formatting the stats to check for 
        /// positive (+1) / neutral (0) / negative (-1) stats (relative to the base template) and display color accordingly.
        /// </summary>
        protected Dictionary<StatType, sbyte> StatTypeValuesDelta = new Dictionary<StatType, sbyte>();

        public abstract float  GetFloat(StatType statType);
        public abstract int    GetInt(StatType statType);
        public abstract short  GetShort(StatType statType);
        public abstract ushort GetUshort(StatType statType);
        public abstract sbyte  GetSbyte(StatType statType);

        public abstract string GetStatsFormatted();

        /// <summary>
        /// Returns the rarity formatted with color and weapon part type name.
        /// </summary>
        public string GetRarityFormatted()
        {
            Color color;

            switch (Rarity)
            {
                case WeaponPartItemRarity.Common:
                    color = CommonRarityColor;
                    break;
                case WeaponPartItemRarity.Uncommon:
                    color = UncommonRarityColor;
                    break;
                case WeaponPartItemRarity.Rare:
                    color = RareRarityColor;
                    break;
                case WeaponPartItemRarity.Legendary:
                    color = LegendaryRarityColor;
                    break;
                case WeaponPartItemRarity.Ancient:
                    color = AncientRarityColor;
                    break;
                case WeaponPartItemRarity.Standard:
                default:
                    color = StandardRarityColor;
                    break;
            }

            return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{Rarity} {BaseTemplate.Type}</color>";
        }

        /// <summary>
        /// Sets the stat type values of this weapon part item.
        /// </summary>
        public void SetStatTypeValues(Dictionary<StatType, float> statTypeValues)
        {
            this.StatTypeValues = statTypeValues;
        }

        /// <summary>
        /// Validates the stat type values to correct and / or remove any unforeseen errors.
        /// </summary>
        public void ValidateStatTypeValues(Dictionary<StatType, float> statTypeValues)
        {
            if (BaseTemplate.Type == WeaponPartTemplateType.Stock)
            {
                statTypeValues[StatType.Mobility] = Mathf.Clamp(statTypeValues[StatType.Mobility], 0, float.MaxValue);
                statTypeValues[StatType.Accuracy] = Mathf.Clamp01(statTypeValues[StatType.Accuracy]);
            }
            else if (BaseTemplate.Type == WeaponPartTemplateType.Body)
            {
                statTypeValues[StatType.Damage]         = Mathf.Clamp(Mathf.CeilToInt(statTypeValues[StatType.Damage]), 0, int.MaxValue);
                statTypeValues[StatType.MaxAmmoPerClip] = Mathf.Clamp(Mathf.CeilToInt(statTypeValues[StatType.MaxAmmoPerClip]), 0, int.MaxValue);
                statTypeValues[StatType.MaxAmmoStock]   = Mathf.Clamp(Mathf.CeilToInt(statTypeValues[StatType.MaxAmmoStock]), 0, int.MaxValue);
                statTypeValues[StatType.FireCooldown]   = Mathf.Clamp(statTypeValues[StatType.FireCooldown], 0, float.MaxValue);
                statTypeValues[StatType.ReloadTime]     = Mathf.Clamp(statTypeValues[StatType.ReloadTime], 0, float.MaxValue);
                statTypeValues[StatType.FullReloadTime] = Mathf.Clamp(statTypeValues[StatType.FullReloadTime], 0, float.MaxValue);
                statTypeValues[StatType.FiringMode]     = (float)((BodyTemplate)BaseTemplate).FiringMode;

                if (((BodyTemplate)BaseTemplate).FiringMode == FiringMode.Burst)
                {
                    statTypeValues[StatType.BurstFireInterval] = Mathf.Clamp(statTypeValues[StatType.BurstFireInterval], 0, float.MaxValue);
                    statTypeValues[StatType.BurstShots]        = Mathf.Clamp(Mathf.CeilToInt(statTypeValues[StatType.BurstShots]), 0, int.MaxValue);
                }
            }
            else if (BaseTemplate.Type == WeaponPartTemplateType.Barrel)
            {
                statTypeValues[StatType.Range]            = Mathf.Clamp(statTypeValues[StatType.Range], 1f, float.MaxValue);
                statTypeValues[StatType.MaxDistance]      = Mathf.Clamp(statTypeValues[StatType.MaxDistance], 1f, float.MaxValue);
                statTypeValues[StatType.DamageMultiplier] = Mathf.Clamp(statTypeValues[StatType.DamageMultiplier], 0.1f, float.MaxValue);
            }
        }

        /// <summary>
        /// Checks the stats for positive (+1) / neutral (0) / negative (-1) stats (relative to the base template).
        /// </summary>
        public void UpdateStatTypeValueDeltas(Dictionary<StatType, float> modifiedStatTypeValues)
        {
            foreach (var kvp in modifiedStatTypeValues)
            {
                if (StatTypeValues.ContainsKey(kvp.Key))
                {
                    if (StatTypeValues[kvp.Key] > kvp.Value)
                    {
                        StatTypeValuesDelta[kvp.Key] = -1;
                    }
                    else if (StatTypeValues[kvp.Key] < kvp.Value)
                    {
                        StatTypeValuesDelta[kvp.Key] = 1;
                    }
                }
            }
        }

    }

}
