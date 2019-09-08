using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Templates.Weapons
{

    enum WeaponPartTemplateType
    {
        Stock,
        Body,
        Barrel
    }

    enum WeaponOutputType
    {
        Raycasting,
        Projectile,
        Electric,
        Support
    }

    abstract class WeaponPartTemplate : ScriptableObject
    {

        #region Editor

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

    }

}
