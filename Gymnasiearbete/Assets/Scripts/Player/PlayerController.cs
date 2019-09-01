using ArenaShooter.Combat;
using ArenaShooter.Controllers;
using ArenaShooter.Entities;
using ArenaShooter.UI;
using Bolt;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Player
{

    class PlayerController : Entity<IPlayerState>, IWeaponHolder
    {

        #region Editor

        [Header("Values")]
        [SerializeField] private int startHealth = 100;  // TEST DATA

        [Space]
        [SerializeField] private LayerMask hitLayerMask;

        #endregion

        #region Private variables

        private Weapon weapon;

        private UIPlayerGameStats uiPlayerGameStats;

        #endregion

        #region IEntity

        public override EntityTeam EntityTeam
        {
            get
            {
                return EntityTeam.Player;
            }
        }

        public override HealableBy HealableBy
        {
            get
            {
                return HealableBy.Player;
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
                return hitLayerMask;
            }
        }

        public GlobalTargets WeaponTargets
        {
            get
            {
                return GlobalTargets.OnlyServer;
            }
        }

        #endregion

        public RaycastWeapon raycastWeapon;
        public ProjectileWeapon projectileWeapon;
        public SupportWeapon supportWeapon;

        private void Awake()
        {
            raycastWeapon = Instantiate(raycastWeapon.gameObject, transform).GetComponent<RaycastWeapon>();
            raycastWeapon.EquipWeapon(this);
            projectileWeapon = Instantiate(projectileWeapon.gameObject, transform).GetComponent<ProjectileWeapon>();
            projectileWeapon.EquipWeapon(this);
            supportWeapon = Instantiate(supportWeapon.gameObject, transform).GetComponent<SupportWeapon>();
            supportWeapon.EquipWeapon(this);

            PlayerEntityController.Singleton.AddPlayerController(this);
        }

        public override void Attached()
        {
            if (entity.IsOwner)
            {
                state.Health = startHealth;

                uiPlayerGameStats = UIPlayerGameStatsController.Singleton.UIPlayerGameStats;
                uiPlayerGameStats.Initialize(this);
                state.AddCallback("Health", uiPlayerGameStats.UpdateUI);

                entity.TakeControl();
            }

            if (BoltNetwork.IsServer && entity.IsOwner)
            {
                EntitySpawnController.Singleton.SpawnEntityOnServer<Enemy>(enemyPrefab, null, new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f)), Quaternion.identity);
            }
        }

        public GameObject enemyPrefab;
        
        private void Update()
        {
            if (Input.GetKey(KeyCode.Alpha1) && entity.IsControllerOrOwner)
            {
                raycastWeapon.Fire();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && entity.IsControllerOrOwner)
            {
                projectileWeapon.Fire();
            }

            if (Input.GetKey(KeyCode.Alpha3) && entity.IsControllerOrOwner)
            {
                supportWeapon.Fire();
            }
        }
        
        public override void OnEvent(WeaponRaycastFireEffectEvent evnt)
        {
            if (evnt.Shooter == entity)
            {
                raycastWeapon.PlayHitEffect(evnt.Point, evnt.Up);
            }
        }

        public override void OnEvent(WeaponProjectileFireEvent evnt)
        {
            if (evnt.Shooter == entity)
            {
                projectileWeapon.FireProjectileEffect(evnt);
            }
        }

        private void OnDestroy()
        {
            PlayerEntityController.Singleton.RemovePlayerController(this);
        }

    }

}
