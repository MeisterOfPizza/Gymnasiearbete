using ArenaShooter.Templates.Items;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Templates.Weapons
{

    enum WeaponPartTemplateType : byte
    {
        Stock  = 0,
        Body   = 10,
        Barrel = 20
    }

    enum WeaponOutputType : byte
    {
        Raycasting = 0,
        Projectile = 1,
        Electric   = 2,
        Support    = 3
    }

    abstract class WeaponPartTemplate : ScriptableObject
    {

        #region Editor
        [Help(@"Player weapon part templates follow a specific ID pattern, where each output type begins with a 100 offset:
Raycast    = 000
Projectile = 100
Electric   = 200
Support    = 300

And each template part begins with a 1000 offset:
Stock  = 0000
Body   = 1000
Barrel = 2000

Which makes the final ID for any default weapon template part of any output type be calculated like this:
STOCK  = OUTPUT_TYPE + 0000
BODY   = OUTPUT_TYPE + 1000
BARREL = OUTPUT_TYPE + 2000

Which is this:
defaultTemplate = OUTPUT_TYPE + TEMPLATE_PART
")]

        [Header("General")]
        [SerializeField] private     ushort templateId;
        [SerializeField] private new string name;

        [TextArea]
        [SerializeField] private string description;

        [Space]
        [SerializeField] private WeaponOutputType outputType;
        
        [Header("Graphics")]
        [SerializeField] private GameObject weaponPartPrefab;

        #endregion

        #region Getters

        public abstract WeaponPartTemplateType Type { get; }

        public ushort TemplateId
        {
            get
            {
                return templateId;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
        }

        public WeaponOutputType OutputType
        {
            get
            {
                return outputType;
            }
        }

        public GameObject WeaponPartPrefab
        {
            get
            {
                return weaponPartPrefab;
            }
        }

        #endregion

        #region Helpers

        public abstract Dictionary<StatType, float> GetStatTypeValues();

        public static string GetStatTypeValueFormatted(StatType statType, float value, string color)
        {
            // Skip MaxAngleOffset as that should be hard coded.
            switch (statType)
            {
                case StatType.Range:
                    return $"Range: <color={color}>{value.ToString("F1")} M</color>";
                case StatType.MaxDistance:
                    return $"Distance: <color={color}>{value.ToString("F1")} M</color>";
                case StatType.DamageMultiplier:
                    return $"Damage Boost: <color={color}>{value.ToString("P0")}</color>";
                case StatType.Damage:
                    return $"Damage: <color={color}>{value}</color>";
                case StatType.MaxAmmoPerClip:
                    return $"Ammo: <color={color}>{value}</color>";
                case StatType.MaxAmmoStock:
                    return $"Ammo Stock: <color={color}>{value}</color>";
                case StatType.FireCooldown:
                    return $"Fire Interval: <color={color}>{value.ToString("F1")} s</color>";
                case StatType.ReloadTime:
                    return $"Reload: <color={color}>{value.ToString("F1")} s</color>";
                case StatType.FullReloadTime:
                    return $"Full Reload: <color={color}>{value.ToString("F1")} s</color>";
                case StatType.BurstFireInterval:
                    return $"Burst Interval: <color={color}>{value.ToString("F1")} s</color>";
                case StatType.BurstShots:
                    return $"Burst Shots: <color={color}>{value}</color>";
                case StatType.Mobility:
                    return $"Mobility: <color={color}>{value.ToString("F1")}</color>";
                case StatType.Accuracy:
                    return $"Accuracy: <color={color}>{value.ToString("P0")}</color>";
                case StatType.FiringMode:
                    return "Mode: " + System.Enum.GetName(typeof(FiringMode), (byte)value);
                default:
                    return "";
            }
        }

        public static sbyte GetStatTypeValueDeltaDirection(StatType statType, float oldValue, float newValue)
        {
            if (oldValue != newValue)
            {
                switch (statType)
                {
                    // old > new = -1 and old < new = 1
                    case StatType.Range:
                    case StatType.MaxDistance:
                    case StatType.DamageMultiplier:
                    case StatType.Damage:
                    case StatType.MaxAmmoPerClip:
                    case StatType.MaxAmmoStock:
                    case StatType.BurstShots:
                    case StatType.Mobility:
                    case StatType.Accuracy:
                        return oldValue > newValue ? (sbyte)-1 : (sbyte)1;

                    // old > new = 1 and old < new = -1
                    case StatType.FireCooldown:
                    case StatType.ReloadTime:
                    case StatType.FullReloadTime:
                    case StatType.BurstFireInterval:
                        return oldValue > newValue ? (sbyte)1 : (sbyte)-1;
                    default:
                        return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        #endregion

    }

}
