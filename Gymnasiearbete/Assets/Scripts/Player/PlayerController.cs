using ArenaShooter.Combat;
using ArenaShooter.Controllers;
using ArenaShooter.Entities;
using ArenaShooter.Extensions;
using ArenaShooter.UI;
using Bolt;
using UnityEngine;
using static ArenaShooter.Combat.Weapon;

#pragma warning disable 0649

namespace ArenaShooter.Player
{

    sealed class PlayerController : Entity<IPlayerState>, IWeaponHolder
    {

        #region Public constants

        public const int PLAYER_MAX_HEALTH = 100;

        #endregion

        #region Private statics

        private static string playerReviveMessageHTMLColor = ColorUtility.ToHtmlStringRGBA(new Color(0.62f, 1f, 0.32f));
        private static string playerDeathMessageHTMLColor  = ColorUtility.ToHtmlStringRGBA(new Color(1f, 0.24f, 0.22f));

        #endregion

        #region Editor

        [Header("References")]
        [SerializeField] private Transform    rightHand;
        [SerializeField] private LineRenderer laserSight;


        [Header("Values")]
        [SerializeField] private LayerMask hitLayerMask;

        #endregion

        #region Public properties

        public static PlayerController Singleton { get; private set; }
        public static Transform        Transform { get; private set; }

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

        public Weapon Weapon
        {
            get
            {
                return weapon;
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
                state.Weapon.WeaponStockId   = stockId;
                state.Weapon.WeaponBodyId    = bodyId;
                state.Weapon.WeaponBarrelId  = barrelId;
                state.Weapon.AmmoLeftInClip  = weapon.AmmoLeftInClip;
                state.Weapon.MaxAmmoPerClip  = weapon.AmmoLeftInClip;
                state.Weapon.AmmoLeftInStock = weapon.AmmoLeftInStock;

                state.Name = UserUtils.GetUsername();

                Singleton = this;
                Transform = this.transform;
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
                // Add callback to the UI:
                weapon.OnAmmoChangedCallback += uiPlayerGameStats.UpdateAmmoUI;

                // Add callback to the state weapon object (networking):
                weapon.OnAmmoChangedCallback += (AmmoStatus ammoStatus) =>
                {
                    state.Weapon.AmmoLeftInClip  = ammoStatus.ammoLeftInClip;
                    state.Weapon.AmmoLeftInStock = ammoStatus.ammoLeftInStock;
                };
            }
            else
            {
                // Add callback from foreign player controller:
                state.AddCallback("Weapon", () =>
                {
                    uiPlayerGameStats.UpdateAmmoUI(new AmmoStatus(state.Weapon.AmmoLeftInClip, state.Weapon.MaxAmmoPerClip, state.Weapon.AmmoLeftInStock));
                });
            }

            weapon.EquipWeapon(this);

            UpdateLaserSight();
        }

        public override void Attached()
        {
            if (entity.IsOwner)
            {
                state.Health = PLAYER_MAX_HEALTH;

                uiPlayerGameStats = UIGameController.Singleton.UIPlayerGameStats;
                uiPlayerGameStats.Initialize(this);

                entity.TakeControl();
            }
            else
            {
                uiPlayerGameStats = UIGameController.Singleton.RegisterForeignPlayerControllerForUI(this);

                state.AddCallback("Name", uiPlayerGameStats.UpdateUsernameUI);
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

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (UIGameController.Singleton != null)
            {
                UIGameController.Singleton.UnregisterForeignPlayerControllerForUI(this);
            }

            Singleton = null;
            Transform = null;
        }

        public override void Revive(EntityRevivedEvent @event)
        {
            bool wasDead = state.Dead;

            base.Revive(@event);

            if (entity.IsOwner && wasDead)
            {
                state.Health = PLAYER_MAX_HEALTH / 4;

                weapon.RefillAmmo(int.MaxValue);

                GameLogMessageEvent playerDeathEvent = GameLogMessageEvent.Create(GlobalTargets.Everyone);
                playerDeathEvent.Message             = $"<color=#{playerReviveMessageHTMLColor}>{UserUtils.GetUsername()} is back in the fight!";
                playerDeathEvent.Send();
            }
        }

        public override void Heal(HealEvent healEvent)
        {
            state.SetDynamic("Health", Mathf.Clamp((int)state.GetDynamic("Health") + healEvent.Heal, 0, PLAYER_MAX_HEALTH));
        }

        public override void Die(EntityDiedEvent @event)
        {
            base.Die(@event);

            if (entity.IsOwner)
            {
                GameLogMessageEvent playerDeathEvent = GameLogMessageEvent.Create(GlobalTargets.Everyone);
                playerDeathEvent.Message             = $"<color=#{playerDeathMessageHTMLColor}>{UserUtils.GetUsername()} died";
                playerDeathEvent.Send();

                state.Deaths++;
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

        public override void OnEvent(RefillAmmoEvent evnt)
        {
            if (evnt.Target == entity && entity.IsOwner)
            {
                weapon.RefillAmmo(evnt.AmountOfClips);
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
