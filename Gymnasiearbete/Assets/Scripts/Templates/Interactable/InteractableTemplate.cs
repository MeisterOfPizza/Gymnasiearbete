using ArenaShooter.Entities;
using UnityEngine;

namespace ArenaShooter.Templates.Interactable
{
    enum InteractableType
    {
        Medkit,
        Ammo,
        LootBox
    }
    abstract class InteractableTemplate : ScriptableObject
    {
        public int id;
        public abstract InteractableType type { get; }

        public abstract void Interact(IEntity entity);

    }
}

