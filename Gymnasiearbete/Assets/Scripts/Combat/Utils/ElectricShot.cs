using ArenaShooter.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace ArenaShooter.Combat.Utils
{

    abstract class ElectricShot : MonoBehaviour
    {

        #region Protected variables

        protected ElectricWeapon weapon;

        #endregion

        public void Initialize(ElectricWeapon weapon)
        {
            this.weapon = weapon;

            OnInitialized();
        }

        protected virtual void OnInitialized()
        {
            // Leave blank.
        }
        
        public abstract List<IEntity> GetTargets();
        public abstract void OnEvent(WeaponFireEffectEvent @event);

    }

}
