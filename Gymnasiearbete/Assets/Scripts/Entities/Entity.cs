using ArenaShooter.Controllers;
using Bolt;
using UnityEngine;

namespace ArenaShooter.Entities
{
    
    [RequireComponent(typeof(BoltEntity))]
    abstract class Entity<T> : EntityEventListener<T>, IEntity where T : IState
    {

        #region Public properties

        public abstract EntityTeam EntityTeam { get; }

        public virtual Vector3 BodyOriginPosition
        {
            get
            {
                return transform.position + Vector3.up;
            }
        }

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

        private void OnEnable()
        {
            EntityController.Singleton.AddEntity(this);
        }

        private void OnDisable()
        {
            EntityController.Singleton.RemoveEntity(this);
        }

        #region IDamagable

        public virtual void TakeDamage(TakeDamageEvent takeDamageEvent)
        {
            state.SetDynamic("Health", Mathf.Clamp((int)state.GetDynamic("Health") - takeDamageEvent.DamageTaken, 0, int.MaxValue));

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
