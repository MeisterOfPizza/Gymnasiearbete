using System.Collections;
using System.Collections.Generic;
using ArenaShooter.Entities;
using UnityEngine;
using Bolt;
using ArenaShooter.Player;

namespace ArenaShooter.Templates.Interactable
{
    [CreateAssetMenu(menuName = "Templates/Interactable/AmmoBox")]
    class AmmoBoxTemplate : InteractableTemplate
    {
        #region Editor

        [SerializeField] private int amountOfClips = 4; //PlaceHolder

        #endregion
        public InteractableType thisInteractableType = InteractableType.Ammo;
        public override InteractableType type => thisInteractableType;

        public override void Interact(IEntity entity)
        {
            var refillAmmoEvent = RefillAmmoEvent.Create(entity.entity, EntityTargets.Everyone);
            refillAmmoEvent.Target = entity.entity;
            refillAmmoEvent.AmountOfClips = amountOfClips;
            refillAmmoEvent.Send();
        }

    }

}
 
