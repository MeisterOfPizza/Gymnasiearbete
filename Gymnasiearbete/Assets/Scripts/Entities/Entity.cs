using Bolt;
using UnityEngine;

namespace ArenaShooter.Entities
{
    
    [RequireComponent(typeof(BoltEntity))]
    abstract class Entity<T> : EntityEventListener<T>, IEntity where T : IState
    {

        #region Protected variables

        protected GlobalEntityCallbacks entityCallbacks;

        #endregion

        private void Start()
        {
            entityCallbacks = gameObject.AddComponent<GlobalEntityCallbacks>();
            entityCallbacks.Initialize(this);

            entity.AddEventListener(entityCallbacks);

            entityCallbacks.OnTakeDamage += TakeDamage;

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

    }

}
