using UnityEngine;
using ArenaShooter.Entities;
using Bolt;

namespace ArenaShooter.Templates.Interactable
{
    [CreateAssetMenu(menuName = "Templates/Interactable/Medkits")]
    class MedKitsTemplate : InteractableTemplate
    {
        #region Editor

        [SerializeField] private int restoredHealth = 50;//PlaceHolder

        #endregion

        public override void Interact(IEntity entity)
        {
            var healEvent    = HealEvent.Create(GlobalTargets.Everyone, ReliabilityModes.ReliableOrdered);
            healEvent.Target = entity.entity;
            healEvent.Heal   = restoredHealth;
            healEvent.Send();

        }
        public InteractableType thisType = InteractableType.Medkit; 

        public override InteractableType type => thisType;
    }

}