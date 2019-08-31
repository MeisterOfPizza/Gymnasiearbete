using ArenaShooter.Controllers;
using ArenaShooter.Extensions;
using ArenaShooter.Player;
using ArenaShooter.UI;
using Bolt;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Entities
{

    /// <summary>
    /// Enemies only exist on the server (or host).
    /// </summary>
    class Enemy : Entity<IEnemyState>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private ParticleSystem explosionEffect;

        [Header("Prefabs")]
        [SerializeField] private GameObject uiEnemyGameStatsPrefab;

        [Header("Values")]
        [SerializeField] private int startHealth = 100; // TEST DATA

        #endregion

        #region Private variables

        private UIEnemyGameStats uiEnemyGameStats;

        #endregion

        private void Awake()
        {
            uiEnemyGameStats = Instantiate(uiEnemyGameStatsPrefab, UIEnemyGameStatsController.Singleton.Container).GetComponent<UIEnemyGameStats>();
            uiEnemyGameStats.Initialize(this);
        }

        private void Update()
        {
            uiEnemyGameStats.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        }

        public LayerMask hitLayerMask;
        private void TestShoot()
        {
            Ray ray = new Ray(transform.position + Vector3.up, transform.forward);
            var hit = Utils.Raycast<PlayerController>(ray, 100f, hitLayerMask, gameObject, QueryTriggerInteraction.Ignore);
            if (hit.HitAnything)
            {
                if (hit.NetworkHit)
                {
                    var takeDamageEvent = TakeDamageEvent.Create(GlobalTargets.Everyone, ReliabilityModes.ReliableOrdered);
                    takeDamageEvent.Target      = hit.Hitbox.GetComponent<IEntity>().entity;
                    takeDamageEvent.DamageTaken = 10;
                    takeDamageEvent.Send();
                }

                var fireEvent = WeaponRaycastFireEffectEvent.Create(entity);
                fireEvent.Shooter = entity;
                fireEvent.Point   = hit.HitPoint;
                fireEvent.Up      = hit.HitNormal;
                fireEvent.Send();
            }
        }

        public override void OnEvent(WeaponRaycastFireEffectEvent evnt)
        {
            if (evnt.Shooter == entity)
            {
                VFXExplosion(evnt.Point, evnt.Up);
            }
        }

        private void VFXExplosion(Vector3 position, Vector3 up)
        {
            explosionEffect.transform.position = position;
            explosionEffect.transform.up       = up;
            explosionEffect.Play(true);
        }

        public override void Attached()
        {
            if (entity.IsOwner)
            {
                state.SetTransforms(state.Transform, transform);
                state.Health = startHealth;

                entity.TakeControl();

                InvokeRepeating("TestShoot", 1f, 1f);
            }

            state.AddCallback("Health", uiEnemyGameStats.UpdateUI);
        }

        private void OnDestroy()
        {
            if (uiEnemyGameStats != null)
            {
                Destroy(uiEnemyGameStats.gameObject);
            }
        }

    }

}
