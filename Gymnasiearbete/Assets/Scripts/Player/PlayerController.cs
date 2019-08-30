using ArenaShooter.Controllers;
using ArenaShooter.Entities;
using ArenaShooter.Extensions;
using ArenaShooter.UI;
using Bolt;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Player
{
    
    class PlayerController : Entity<IPlayerState>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private ParticleSystem explosionEffect;

        [Header("Values")]
        [SerializeField] private int   startHealth = 100;  // TEST DATA
        [SerializeField] private int   startDamage = 10;   // TEST DATA
        [SerializeField] private float damageRange = 100f; // TEST DATA

        [Space]
        [SerializeField] private LayerMask enemyHitLayerMask;
        [SerializeField] private LayerMask weaponHitLayerMask;

        #endregion

        #region Private variables

        private UIPlayerGameStats uiPlayerGameStats;

        #endregion

        public override void Attached()
        {
            if (entity.IsOwner)
            {
                state.Health = startHealth;
                state.Weapon.WeaponAmmo = 100;

                uiPlayerGameStats = UIPlayerGameStatsController.Singleton.UIPlayerGameStats;
                uiPlayerGameStats.Initialize(this);
                state.AddCallback("Health", uiPlayerGameStats.UpdateUI);

                entity.TakeControl();
            }

            if (BoltNetwork.IsServer && entity.IsOwner)
            {
                EntitySpawnController.Singleton.SpawnEntityOnServer<Assets.Scripts.Entities.Enemy>(enemyPrefab, null, new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f)), Quaternion.identity);
            }
        }

        public GameObject enemyPrefab;

        private void Update()
        {
            if (entity.IsControllerOrOwner && Input.GetMouseButtonDown(0))
            {
                leftMouseBtnPressedLastFrame = true;
            }
        }

        bool leftMouseBtnPressedLastFrame;

        public override void SimulateController()
        {
            if (leftMouseBtnPressedLastFrame)
            {
                var cmd = PrimaryShootCommand.Create();
                cmd.Shooter  = entity;
                cmd.Position = transform.position + Vector3.up;
                cmd.Normal   = transform.forward;

                entity.QueueInput(cmd);

                leftMouseBtnPressedLastFrame = false;
            }
        }

        public override void ExecuteCommand(Command command, bool resetState)
        {
            if (command is PrimaryShootCommand primaryShootCmd)
            {
                Shoot(primaryShootCmd);
            }
        }

        private void Shoot(PrimaryShootCommand cmd)
        {
            // TODO: Get action (or ray) from weapon.
            
            Ray ray  = new Ray(cmd.Input.Position, cmd.Input.Normal * damageRange);
            using (var hits = BoltNetwork.RaycastAll(ray))
            {
                bool networkHit = false;

                // Search network hitboxes for hits:
                for (int i = 0; i < hits.count; i++)
                {
                    var hitbox = hits.GetHit(i).hitbox;

                    if (hitbox.gameObject != gameObject && enemyHitLayerMask.HasLayer(hitbox.gameObject.layer) && hitbox.GetComponent<IDamagable>() is IDamagable damagable)
                    {
                        var takeDamageEvent = TakeDamageEvent.Create(GlobalTargets.OnlyServer, ReliabilityModes.ReliableOrdered);
                        takeDamageEvent.Target = hitbox.GetComponent<IEntity>().entity;
                        takeDamageEvent.DamageTaken = startDamage;
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
                    if (Physics.Raycast(ray, out RaycastHit hit, damageRange, weaponHitLayerMask, QueryTriggerInteraction.Ignore))
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
            Ray ray = new Ray(transform.position + Vector3.up, transform.forward * damageRange);
            using (var hits = BoltNetwork.RaycastAll(ray))
            {
                bool networkHit = false;

                // Search network hitboxes for hits:
                for (int i = 0; i < hits.count; i++)
                {
                    var hitbox = hits.GetHit(i).hitbox;

                    if (hitbox.gameObject != gameObject && enemyHitLayerMask.HasLayer(hitbox.gameObject.layer) && hitbox.GetComponent<IDamagable>() is IDamagable damagable)
                    {
                        VFXExplosion(ray.origin + ray.direction * hits.GetHit(i).distance);
                    }
                }

                // None were found, search geometry hitboxes for hits:
                if (!networkHit)
                {
                    if (Physics.Raycast(ray, out RaycastHit hit, damageRange, weaponHitLayerMask, QueryTriggerInteraction.Ignore))
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

    }

}
