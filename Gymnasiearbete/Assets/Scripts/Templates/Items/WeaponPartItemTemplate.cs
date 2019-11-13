using ArenaShooter.Combat;
using ArenaShooter.Templates.Weapons;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Templates.Items
{

    #region Enums

    enum WeaponPartItemRarity : byte
    {
        /// <summary>
        /// Grey color theme.
        /// The default weapon part items.
        /// </summary>
        Standard = 0,
        /// <summary>
        /// White color theme.
        /// </summary>
        Common = 10,
        /// <summary>
        /// Green color theme.
        /// </summary>
        Uncommon = 20,
        /// <summary>
        /// Cyan color theme.
        /// </summary>
        Rare = 30,
        /// <summary>
        /// Yellow color theme.
        /// </summary>
        Legendary = 40,
        /// <summary>
        /// Red color theme.
        /// </summary>
        Ancient = 50
    }

    /// <summary>
    /// All types of stats on weapon parts.
    /// </summary>
    [System.Flags]
    enum StatType : short
    {
        None              = 0,
        Range             = 1,
        MaxDistance       = 2,
        DamageMultiplier  = 4,
        Damage            = 8,
        MaxAmmoPerClip    = 16,
        MaxAmmoStock      = 32,
        FireCooldown      = 64,
        ReloadTime        = 128,
        FullReloadTime    = 256,
        BurstFireInterval = 512,
        BurstShots        = 1024,
        Mobility          = 2048,
        Accuracy          = 4096,
        Everything        = ~0
    }
    
    #endregion

    /// <summary>
    /// This is an item of a weapon part.
    /// These should be used with WeaponParts to create WeaponPartItems to be used in the inventory system.
    /// </summary>
    [CreateAssetMenu(menuName = "Templates/Weapon Part Item")]
    class WeaponPartItemTemplate : ScriptableObject
    {

        #region Editor

        [SerializeField] private WeaponPartItemRarity rarity                       = WeaponPartItemRarity.Common;
        [SerializeField] private WeaponPartTemplate[] possibleWeaponPartTemplates  = new WeaponPartTemplate[0];

        [Space]
        [SerializeField] private StatChange[] statChanges;

        #endregion

        #region Classes

        [System.Serializable]
        private class StatChange
        {

            [SerializeField] private StatType   statType   = StatType.None;
            [SerializeField] private ChangeType changeType = ChangeType.Multiplication;
            [SerializeField] private float      minValue   = 1f;
            [SerializeField] private float      maxValue   = 1f;

            #region Enum

            private enum ChangeType
            {
                Addition,
                Multiplication
            }

            #endregion

            public void ApplyValues(Dictionary<StatType, float> statTypeValues)
            {
                foreach (var statType in System.Enum.GetValues(typeof(StatType)).Cast<StatType>())
                {
                    if (this.statType.HasFlag(statType) && statTypeValues.ContainsKey(statType))
                    {
                        statTypeValues[statType] = GetValue(statTypeValues[statType]);
                    }
                }
            }

            private float GetValue(float baseValue)
            {
                return changeType == ChangeType.Addition ? baseValue + Random.Range(minValue, maxValue) : baseValue * Random.Range(minValue, maxValue);
            }

        }

        #endregion

        public WeaponPartItem<T> CreateWeaponPartItem<T>() where T : WeaponPartTemplate
        {
            StatChange chosenStatChange = statChanges[Random.Range(0, statChanges.Length)];

            var templatePool = possibleWeaponPartTemplates.Where(p => p.GetType() == typeof(T));

            if (templatePool.Count() > 0)
            {
                WeaponPartItem<T> weaponPartItem = new WeaponPartItem<T>(rarity, (T)templatePool.ElementAt(Random.Range(0, templatePool.Count())));

                chosenStatChange.ApplyValues(weaponPartItem.StatTypeValues);

                return weaponPartItem;
            }
            else
            {
                return null;
            }
        }

        public WeaponPartItem<WeaponPartTemplate> CreateRandomWeaponPartItem()
        {
            StatChange chosenStatChange = statChanges[Random.Range(0, statChanges.Length)];

            WeaponPartItem<WeaponPartTemplate> weaponPartItem = new WeaponPartItem<WeaponPartTemplate>(rarity, possibleWeaponPartTemplates[Random.Range(0, possibleWeaponPartTemplates.Length)]);

            chosenStatChange.ApplyValues(weaponPartItem.StatTypeValues);

            return weaponPartItem;
        }

    }

}
