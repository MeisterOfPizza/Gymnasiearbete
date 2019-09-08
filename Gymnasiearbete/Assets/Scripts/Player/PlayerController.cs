using ArenaShooter.Combat;
using ArenaShooter.Controllers;
using ArenaShooter.Entities;
using ArenaShooter.UI;
using Bolt;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Player
{

    sealed class PlayerController : Entity<IPlayerState>, IWeaponHolder
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

        protected override void Start()
        {
            base.Start();

            if (entity.IsOwner)
            {
                weapon = UILoadoutController.GetWeapon(transform);

                weapon.Stats.GetWeaponPartTemplateIds(out ushort stockId, out ushort bodyId, out ushort barrelId);
                state.Weapon.WeaponStockId  = stockId;
                state.Weapon.WeaponBodyId   = bodyId;
                state.Weapon.WeaponBarrelId = barrelId;
            }
            else
            {
                weapon = WeaponController.Singleton.CreateWeapon(WeaponController.Singleton.GetStockTemplate((ushort)state.Weapon.WeaponStockId), WeaponController.Singleton.GetBodyTemplate((ushort)state.Weapon.WeaponBodyId), WeaponController.Singleton.GetBarrelTemplate((ushort)state.Weapon.WeaponBarrelId), transform);
            }

            if (entity.IsOwner)
            {
                // Adding callbacks to the UI:
                weapon.OnAmmoChangedCallback += uiPlayerGameStats.UpdateAmmoUI;
            }

            weapon.EquipWeapon(this);
        }

        public override void Attached()
        {
            if (entity.IsOwner)
            {
                state.Health = startHealth;

                uiPlayerGameStats = UIPlayerGameStatsController.Singleton.UIPlayerGameStats;
                uiPlayerGameStats.Initialize(this);

                // Adding callbacks to the UI:
                state.AddCallback("Health", uiPlayerGameStats.UpdateHealthUI);

                entity.TakeControl();
            }
        }

        public GameObject enemyPrefab;
        
        private void Update()
        {
            if (entity.IsControllerOrOwner)
            {
                weapon.CheckForInput();
            }
        }

        #region OnEvents

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
