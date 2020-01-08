using ArenaShooter.Controllers;
using Bolt;
using System;
using UnityEngine;

namespace ArenaShooter.Entities
{
    
    [RequireComponent(typeof(BoltEntity))]
    abstract class Entity<T> : EntityEventListener<T>, IEntity where T : IState
    {

        #region Editor

        [Header("References - Entity")]
        [SerializeField] private new GameObject     renderer;
        [SerializeField] private     BoltHitboxBody hitboxBody;
        [SerializeField] private     Collider[]     colliders;

        #endregion

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

        public virtual Vector3 HeadOriginPosition
        {
            get
            {
                return transform.position + Vector3.up * 2;
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

        protected virtual void OnEnable()
        {
            EntityController.Singleton?.AddEntity(this);
        }

        protected virtual void OnDisable()
        {
            EntityController.Singleton?.RemoveEntity(this);
        }

        protected virtual void OnDestroy()
        {
            OnDestroyCallback?.Invoke();
        }

        #region IDamagable

        public virtual void TakeDamage(TakeDamageEvent takeDamageEvent)
        {
            state.SetDynamic("Health", Mathf.Clamp((int)state.GetDynamic("Health") - takeDamageEvent.DamageTaken, 0, int.MaxValue));

            if ((int)state.GetDynamic("Health") <= 0)
            {
                var entityDeathEvent                          = EntityDiedEvent.Create(GlobalTargets.Others, ReliabilityModes.ReliableOrdered);
                entityDeathEvent.DeadEntity                   = entity;
                entityDeathEvent.WeaponPartItemTemplateDropId = -1;
                entityDeathEvent.Send();

                Die(entityDeathEvent);
            }
        }

        public virtual void Revive(EntityRevivedEvent @event)
        {
            SetEntityVisible(true);

            OnReviveCallback?.Invoke();

            if (entity.IsOwner)
            {
                state.SetDynamic("Dead", false);
            }
        }

        public virtual void Die(EntityDiedEvent @event)
        {
            SetEntityVisible(false);

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

        #region Helpers

        /// <summary>
        /// Sets the entity visible in the scene by activating/deactivating hitboxes, colliders and renderers.
        /// </summary>
        public void SetEntityVisible(bool visible)
        {
            renderer.SetActive(visible);
            hitboxBody.enabled = visible;

            foreach (Collider collider in colliders)
            {
                collider.enabled = visible;
            }
        }

        #endregion

        #region Static helpers

        public static void SetEntityActive<E>(E entity, bool active) where E : IEntity
        {
            entity.SetEntityVisible(active);
        }

        #endregion

    }

}
