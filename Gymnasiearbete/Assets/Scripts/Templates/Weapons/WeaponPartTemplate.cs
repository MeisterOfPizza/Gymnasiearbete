using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Templates.Weapons
{

    enum WeaponPartTemplateType : byte
    {
        Stock,
        Body,
        Barrel
    }

    enum WeaponOutputType : byte
    {
        Raycasting,
        Projectile,
        Electric,
        Support
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

    }

}
