using UnityEngine;


namespace ArenaShooter.Templates.Weapons
{
    enum WeaponTemplateType
    {
        stock,
        body,
        barrel
    }
    abstract class WeaponPartTemplate : ScriptableObject
    {
        [Header("General")]
        [SerializeField]private ushort templateId;
        [SerializeField]private string name;
        

        [TextArea]
        [SerializeField]private string description;
        
        [Header("Graphics")]
        [SerializeField]private GameObject weaponPartPrefab;

        public abstract WeaponTemplateType type { get; }

        public ushort TemplateId
        {
            get
            {
                return templateId;
            }
        }

        

        
    }
}


