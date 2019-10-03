using UnityEngine;
using Bolt;
using ArenaShooter.Player;
using ArenaShooter.Templates.Interactable;

namespace ArenaShooter.Combat.Pickup
{
    [RequireComponent(typeof(BoltEntity))]
    class Interactable : EntityEventListener<IInteractbleState>
    {
        #region Private Variables

        [SerializeField] private InteractableTemplate template;

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (entity.IsOwner && other.GetComponent<PlayerController>() is PlayerController player && state.IsAvailable)
            {
                template.Interact(player);
                state.IsAvailable = false;
            }
        }
    }
}