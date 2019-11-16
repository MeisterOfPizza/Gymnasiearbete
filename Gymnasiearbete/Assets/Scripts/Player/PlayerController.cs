using ArenaShooter.Combat;
using ArenaShooter.Controllers;
using ArenaShooter.Entities;
using ArenaShooter.Extensions;
using ArenaShooter.UI;
using Bolt;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Player
{

    sealed class PlayerController : Entity<IPlayerState>, IWeaponHolder
    {

        #region Editor

        [Header("References")]
        [SerializeField] private Transform    rightHand;
        [SerializeField] private LineRenderer laserSight;

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
                weapon = Profile.SelectedLoadoutSlot.Loadout.CreateWeapon(rightHand);
                BuiltWeapon.AssembleWeapon(Profile.SelectedLoadoutSlot.Loadout, rightHand).transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                weapon.Stats.GetWeaponPartTemplateIds(out ushort stockId, out ushort bodyId, out ushort barrelId);
                state.Weapon.WeaponStockId  = stockId;
                state.Weapon.WeaponBodyId   = bodyId;
                state.Weapon.WeaponBarrelId = barrelId;
            }
            else
            {
                var stockTemplate  = WeaponController.Singleton.GetStockTemplate((ushort)state.Weapon.WeaponStockId);
                var bodyTemplate   = WeaponController.Singleton.GetBodyTemplate((ushort)state.Weapon.WeaponBodyId);
                var barrelTemplate = WeaponController.Singleton.GetBarrelTemplate((ushort)state.Weapon.WeaponBarrelId);

                weapon = WeaponController.Singleton.CreateBystanderWeapon(stockTemplate, bodyTemplate, barrelTemplate, rightHand);
                BuiltWeapon.AssembleWeapon(stockTemplate, bodyTemplate, barrelTemplate, rightHand).transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }

            if (entity.IsOwner)
            {
                // Adding callbacks to the UI:
                weapon.OnAmmoChangedCallback += uiPlayerGameStats.UpdateAmmoUI;
            }

            weapon.EquipWeapon(this);

            UpdateLaserSight();
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
        
        private void Update()
        {
            if (entity.IsControllerOrOwner)
            {
                weapon.CheckForInput();
            }

            UpdateLaserSight();
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

        #region Helpers

        private void UpdateLaserSight()
        {
            Ray ray = new Ray(rightHand.position, rightHand.forward);

            laserSight.SetPosition(0, rightHand.position);

            var hit = Utils.Raycast(ray, weapon.Stats.MaxDistance, WeaponHitLayerMask, gameObject, QueryTriggerInteraction.Ignore);

            if (hit.HitAnything)
            {
                laserSight.SetPosition(1, hit.HitPoint);
            }
            else
            {
                laserSight.SetPosition(1, ray.origin + ray.direction * weapon.Stats.MaxDistance);
            }
        }

        #endregion

    }

}
