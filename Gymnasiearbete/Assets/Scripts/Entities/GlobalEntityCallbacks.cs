using Bolt;
using System;

namespace ArenaShooter.Entities
{
    
    class GlobalEntityCallbacks : GlobalEventListener
    {

        #region Event callbacks

        public Action<TakeDamageEvent> OnTakeDamage { get; set; }
        public Action<HealEvent>       OnHeal       { get; set; }

        #endregion

        public IEntity Entity { get; private set; }

        public void Initialize(IEntity entity)
        {
            this.Entity = entity;
        }

        #region Event listeners

        public override void OnEvent(TakeDamageEvent evnt)
        {
            if (evnt.Target == Entity.entity)
            {
                OnTakeDamage?.Invoke(evnt);
            }
        }

        public override void OnEvent(HealEvent evnt)
        {
            if (evnt.Target == Entity.entity)
            {
                OnHeal?.Invoke(evnt);
            }
        }

        #endregion

    }

}
