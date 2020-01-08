using Bolt;
using System;

namespace ArenaShooter.Entities
{
    
    class GlobalEntityCallbacks : GlobalEventListener
    {

        #region Event callbacks

        public Action<TakeDamageEvent>    OnTakeDamage    { get; set; }
        public Action<HealEvent>          OnHeal          { get; set; }
        public Action<EntityRevivedEvent> OnEntityRevived { get; set; }
        public Action<EntityDiedEvent>    OnEntityDied    { get; set; }
        public Action<RefillAmmoEvent>    OnAmmoRefill    { get; set; }   
        
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

        public override void OnEvent(EntityRevivedEvent evnt)
        {
            if (evnt.RevivedEntity == Entity.entity)
            {
                OnEntityRevived?.Invoke(evnt);
            }
        }

        public override void OnEvent(EntityDiedEvent evnt)
        {
            if (evnt.DeadEntity == Entity.entity)
            {
                OnEntityDied?.Invoke(evnt);
            }
        }

        public override void OnEvent(RefillAmmoEvent evnt)
        {
            if(evnt.Target == Entity.entity)
            {
                OnAmmoRefill?.Invoke(evnt);
            }
        }

        #endregion

    }

}
