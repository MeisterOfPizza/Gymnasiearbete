using ArenaShooter.AI;
using ArenaShooter.Combat;
using ArenaShooter.Controllers;
using ArenaShooter.Extensions;
using ArenaShooter.Player;
using ArenaShooter.Templates.Enemies;
using ArenaShooter.UI;
using Bolt;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Entities
{

    /// <summary>
    /// Enemies only exist on the server (or host).
    /// </summary>
    class Enemy : Entity<IEnemyState>, IWeaponHolder
    {

        #region Editor

        [Header("References")]
        [SerializeField] private AIAgent      aiAgent;
        [SerializeField] private HumanoidBody humanoidBody;

        [Space]
        [SerializeField] private Transform renderTransform;

        [Header("Values")]
        [SerializeField] private LayerMask weaponHitLayerMask;

        [Header("Prefabs")]
        [SerializeField] private GameObject uiEnemyGameStatsPrefab;

        #endregion

        #region IEntity

        public override EntityTeam EntityTeam
        {
            get
            {
                return EntityTeam.Enemy;
            }
        }

        public override HealableBy HealableBy
        {
            get
            {
                return HealableBy.Enemy;
            }
        }

        #endregion

        #region IWeaponHolder

        public Vector3 WeaponFirePosition
        {
            get
            {
                return transform.position + Vector3.up;
            }
        }

        public Vector3 WeaponForward
        {
            get
            {
                return transform.forward;
            }
        }

        public LayerMask WeaponHitLayerMask
        {
            get
            {
                return weaponHitLayerMask;
            }
        }

        public GlobalTargets WeaponTargets
        {
            get
            {
                return GlobalTargets.Everyone;
            }
        }

        #endregion

        #region Protected variables

        protected EnemyTemplate enemyTemplate;
        protected Weapon        weapon;

        protected UIEnemyGameStats uiEnemyGameStats;

        #endregion

        #region Game cycle

        public void Initialize(ushort enemyTemplateId)
        {
            state.EnemyTemplateId = enemyTemplateId;
        }

        private void Update()
        {
            uiEnemyGameStats.transform.position = MainCameraController.MainCamera.WorldToScreenPoint(transform.position);

            if (!entity.IsOwner)
            {
                humanoidBody.UpperBodyCurrent = state.UpperBodyNormal;
                humanoidBody.LowerBodyCurrent = state.LowerBodyNormal;
            }
        }

        private void FixedUpdate()
        {
            if (entity.IsOwner)
            {
                CheckForEnemies();
            }
        }

        public override void SimulateOwner()
        {
            state.UpperBodyNormal = humanoidBody.UpperBodyCurrent;
            state.LowerBodyNormal = humanoidBody.LowerBodyCurrent;
        }

        public override void Attached()
        {
            this.enemyTemplate = EnemyTemplateController.Singleton.GetEnemyTemplate((ushort)state.EnemyTemplateId);
            this.weapon        = WeaponController.Singleton.CreateWeapon(enemyTemplate.GetEnemyWeaponTemplate(), transform);
            this.weapon.EquipWeapon(this);

            uiEnemyGameStats = Instantiate(uiEnemyGameStatsPrefab, UIEnemyGameStatsController.Singleton.Container).GetComponent<UIEnemyGameStats>();
            uiEnemyGameStats.Initialize(this);
            uiEnemyGameStats.transform.position = MainCameraController.MainCamera.WorldToScreenPoint(transform.position);

            state.SetTransforms(state.Transform, transform, renderTransform);

            if (entity.IsOwner)
            {
                state.Health           = enemyTemplate.Health;
                state.WeaponTemplateId = weapon.Stats.GetEnemyWeaponTemplateId();

                entity.TakeControl();

                aiAgent.Initialize(this, weapon.Stats.TargetEntityTeam, enemyTemplate.TargetSearchFrequency, weapon.Stats.Range * 0.9f, weapon.Stats.Range / 2f, enemyTemplate.MovementSpeed, enemyTemplate.TurnSpeed, humanoidBody);
            }
            else
            {
                Destroy(aiAgent);
            }

            state.AddCallback("Health", uiEnemyGameStats.UpdateUI);

            humanoidBody.ManualControls = !entity.IsOwner;
        }

        private void OnDestroy()
        {
            if (uiEnemyGameStats != null)
            {
                Destroy(uiEnemyGameStats.gameObject);
            }
        }

        #endregion

        #region Life

        public override void Revive(EntityRevivedEvent @event)
        {
            base.Revive(@event);

            if (entity.IsOwner)
            {
                state.Health = enemyTemplate.Health;

                // TODO: Reset weapon and reload it instantly.
            }

            uiEnemyGameStats.gameObject.SetActive(true);
            uiEnemyGameStats.UpdateUI();
        }

        public override void Die(EntityDiedEvent @event)
        {
            base.Die(@event);

            uiEnemyGameStats.gameObject.SetActive(false);
        }

        #endregion

        #region Combat

        private void CheckForEnemies()
        {
            var hit = Utils.Raycast<PlayerController>(new Ray(BodyOriginPosition, transform.forward), weapon.Stats.Range, weaponHitLayerMask, gameObject);

            if (hit.NetworkHit)
            {
                weapon.FireWithoutInput();
            }
        }

        #endregion

        #region OnEvent

        public override void OnEvent(WeaponFireEffectEvent evnt)
        {
            if (evnt.Shooter == entity)
            {
                weapon.OnEvent(evnt);
            }
        }

        public override void OnEvent(WeaponSupportBeginFireEffectEvent evnt)
        {
            if (evnt.Shooter == entity)
            {
                weapon.OnEvent(evnt);
            }
        }

        #endregion

    }

}
