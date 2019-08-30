using UnityEngine;


namespace ArenaShooter.Templates.Weapons
{
    abstract class WeaponPartTemplate : ScriptableObject
    {
        [SerializeField]private ushort templateId;
        [SerializeField]private string name;

        [TextArea]
        [SerializeField]private string description;

        public ushort TemplateId
        {
            get
            {
                return templateId;
            }
        }
        
    }
}


