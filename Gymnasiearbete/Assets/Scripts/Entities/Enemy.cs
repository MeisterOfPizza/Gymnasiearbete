using ArenaShooter.Controllers;
using ArenaShooter.Extensions;
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

        public LayerMask playerHitLayerMask;
        public LayerMask weaponHitLayerMask;
        private void TestShoot()
        {
            Ray ray = new Ray(transform.position + Vector3.up, transform.forward * 100f);
            using (var hits = BoltNetwork.RaycastAll(ray))
            {
                bool networkHit = false;

                // Search network hitboxes for hits:
                for (int i = 0; i < hits.count; i++)
                {
                    var hitbox = hits.GetHit(i).hitbox;

                    if (hitbox.gameObject != gameObject && playerHitLayerMask.HasLayer(hitbox.gameObject.layer) && hitbox.GetComponent<IDamagable>() is IDamagable damagable)
                    {
                        var takeDamageEvent = TakeDamageEvent.Create(GlobalTargets.Everyone, ReliabilityModes.ReliableOrdered);
                        takeDamageEvent.Target = hitbox.GetComponent<IEntity>().entity;
                        takeDamageEvent.DamageTaken = 10;
                        takeDamageEvent.Send();

                        var weaponFireEvent = WeaponFireEvent.Create(entity);
                        weaponFireEvent.Shooter = entity;
                        weaponFireEvent.Send();

                        networkHit = true;

                        break;
                    }
                }

                // None were found, search geometry hitboxes for hits:
                if (!networkHit)
                {
                    if (Physics.Raycast(ray, out RaycastHit hit, 100f, weaponHitLayerMask, QueryTriggerInteraction.Ignore))
                    {
                        var weaponFireEvent = WeaponFireEvent.Create(entity);
                        weaponFireEvent.Shooter = entity;
                        weaponFireEvent.Send();
                    }
                }
            }
        }

        public override void OnEvent(WeaponFireEvent evnt)
        {
            Ray ray = new Ray(transform.position + Vector3.up, transform.forward * 100f);
            using (var hits = BoltNetwork.RaycastAll(ray))
            {
                bool networkHit = false;

                // Search network hitboxes for hits:
                for (int i = 0; i < hits.count; i++)
                {
                    var hitbox = hits.GetHit(i).hitbox;

                    if (hitbox.gameObject != gameObject && playerHitLayerMask.HasLayer(hitbox.gameObject.layer) && hitbox.GetComponent<IDamagable>() is IDamagable damagable)
                    {
                        VFXExplosion(ray.origin + ray.direction * hits.GetHit(i).distance);
                    }
                }

                // None were found, search geometry hitboxes for hits:
                if (!networkHit)
                {
                    if (Physics.Raycast(ray, out RaycastHit hit, 100f, weaponHitLayerMask, QueryTriggerInteraction.Ignore))
                    {
                        VFXExplosion(hit.point);
                    }
                }
            }
        }

        private void VFXExplosion(Vector3 position)
        {
            explosionEffect.transform.position = position;
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
