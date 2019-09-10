using ArenaShooter.Controllers;
using Bolt;
using System;
using UnityEngine;

namespace ArenaShooter.Entities
{
    
    [RequireComponent(typeof(BoltEntity))]
    abstract class Entity<T> : EntityEventListener<T>, IEntity where T : IState
    {

        #region Public properties

        public Action OnDeathCallback   { get; set; }
        public Action OnReviveCallback  { get; set; }
        public Action OnDestroyCallback { get; set; }

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

            entityCallbacks.OnTakeDamage    += TakeDamage;
            entityCallbacks.OnHeal          += Heal;
            entityCallbacks.OnEntityRevived += Revive;
            entityCallbacks.OnEntityDied    += Die;

            OnEntityCallbacksReady();

            gameObject.SetActive(!(bool)state.GetDynamic("Dead"));
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

        private void OnDestroy()
        {
            OnDestroyCallback?.Invoke();
        }

        #region IDamagable

        public void TakeDamage(TakeDamageEvent takeDamageEvent)
        {
            state.SetDynamic("Health", Mathf.Clamp((int)state.GetDynamic("Health") - takeDamageEvent.DamageTaken, 0, int.MaxValue));

            if ((int)state.GetDynamic("Health") <= 0)
            {
                var entityDeathEvent = EntityDiedEvent.Create(GlobalTargets.Others, ReliabilityModes.ReliableOrdered);
                entityDeathEvent.DeadEntity = entity;
                entityDeathEvent.Send();

                Die(entityDeathEvent);
            }
        }

        public virtual void Revive(EntityRevivedEvent @event)
        {
            gameObject.SetActive(true);

            OnReviveCallback?.Invoke();

            if (entity.IsOwner)
            {
                state.SetDynamic("Dead", false);
            }
        }

        public virtual void Die(EntityDiedEvent @event)
        {
            gameObject.SetActive(false);

            OnDeathCallback?.Invoke();

            if (entity.IsOwner)
            {
                state.SetDynamic("Dead", true);
            }
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
