using Bolt;
using UnityEngine;

namespace ArenaShooter.Entities
{
    
    [RequireComponent(typeof(BoltEntity))]
    abstract class Entity<T> : EntityEventListener<T>, IEntity where T : IState
    {

        #region Public properties

        public abstract EntityTeam EntityTeam { get; }

        #endregion

        #region Protected variables

        protected GlobalEntityCallbacks entityCallbacks;

        #endregion

        #region IHealable

        public abstract HealableBy HealableBy { get; }

        #endregion

        protected virtual void Start()
        {
            entityCallbacks = gameObject.AddComponent<GlobalEntityCallbacks>();
            entityCallbacks.Initialize(this);

            entity.AddEventListener(entityCallbacks);

            entityCallbacks.OnTakeDamage += TakeDamage;
            entityCallbacks.OnHeal       += Heal;

            OnEntityCallbacksReady();
        }

        /// <summary>
        /// Called whenever <see cref="entityCallbacks"/> has been created and is ready.
        /// </summary>
        protected virtual void OnEntityCallbacksReady()
        {
            // Leave blank.
        }

        #region IDamagable

        public virtual void TakeDamage(TakeDamageEvent takeDamageEvent)
        {
            state.SetDynamic("Health", (int)state.GetDynamic("Health") - takeDamageEvent.DamageTaken);

            if ((int)state.GetDynamic("Health") <= 0)
            {
                Die();
            }
        }

        public virtual void Die()
        {
            BoltNetwork.Destroy(gameObject);
        }

        #endregion

        #region IHealable

        public void Heal(HealEvent healEvent)
        {
            state.SetDynamic("Health", (int)state.GetDynamic("Health") + healEvent.Heal);
        }

        #endregion

    }

}
