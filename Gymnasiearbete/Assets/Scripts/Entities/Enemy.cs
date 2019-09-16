﻿using ArenaShooter.AI;
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
    class Enemy : Entity<IEnemyState>, IWeaponHolder, IAIAgentBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private AIAgent aiAgent;
        [SerializeField] private Body    body;

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
                return body.UpperBodyCurrent;
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

        #region IAIAgentBehaviour

        public EntityTeam SearchTargetTeam
        {
            get
            {
                return weapon.Stats.TargetEntityTeam;
            }
        }

        public float SearchInterval
        {
            get
            {
                return enemyTemplate.TargetSearchFrequency;
            }
        }

        public float SearchThreshold
        {
            get
            {
                return weapon.Stats.Range * 0.9f;
            }
        }

        public float StoppingDistance
        {
            get
            {
                return weapon.Stats.Range / 2f;
            }
        }

        public float MovementSpeed
        {
            get
            {
                return enemyTemplate.MovementSpeed;
            }
        }

        public float TurnSpeed
        {
            get
            {
                return enemyTemplate.TurnSpeed;
            }
        }

        public Body Body
        {
            get
            {
                return body;
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
                body.UpperBodyCurrent = state.UpperBodyNormal;
                body.LowerBodyCurrent = state.LowerBodyNormal;
            }
        }

        private void FixedUpdate()
        {
            if (entity.IsOwner)
            {
                CheckForTargets();
            }
        }

        public override void SimulateOwner()
        {
            state.UpperBodyNormal = body.UpperBodyCurrent;
            state.LowerBodyNormal = body.LowerBodyCurrent;
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
                
                aiAgent.Initialize(this);
            }
            else
            {
                Destroy(aiAgent);
            }

            state.AddCallback("Health", uiEnemyGameStats.UpdateUI);

            body.ManualControls = !entity.IsOwner;
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

        private void CheckForTargets()
        {
            var hit = Utils.Raycast(new Ray(BodyOriginPosition, body.UpperBodyCurrent), weapon.Stats.Range, Physics.AllLayers, gameObject, QueryTriggerInteraction.Ignore);
            
            if (hit.GameObject != null && hit.GameObject.GetComponent<IEntity>() is IEntity entity)
            {
                if (entity.EntityTeam == weapon.Stats.TargetEntityTeam)
                {
                    weapon.FireWithoutInput();

                    return;
                }
            }

            weapon.StopFiring();
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
