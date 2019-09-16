using ArenaShooter.Entities;
using ArenaShooter.Templates.Enemies;
using ArenaShooter.Templates.Weapons;
using UnityEngine;

namespace ArenaShooter.Combat
{

    sealed class WeaponStats
    {

        #region Public properties

        public float Mobility
        {
            get
            {
                return ownerIsPlayer ? stockTemplate.Mobility : enemyWeaponTemplate.Mobility;
            }
        }

        public float Accuracy
        {
            get
            {
                return ownerIsPlayer ? stockTemplate.Accuracy : enemyWeaponTemplate.Accuracy;
            }
        }

        public ushort Damage
        {
            get
            {
                return ownerIsPlayer ? bodyTemplate.Damage : enemyWeaponTemplate.Damage;
            }
        }

        public short MaxAmmoPerClip
        {
            get
            {
                return ownerIsPlayer ? bodyTemplate.MaxAmmoPerClip : enemyWeaponTemplate.MaxAmmoPerClip;
            }
        }

        public short MaxAmmoStock
        {
            get
            {
                return ownerIsPlayer ? bodyTemplate.MaxAmmoStock : short.MaxValue;
            }
        }

        public short AmmoPerFire
        {
            get
            {
                return ownerIsPlayer ? bodyTemplate.AmmoPerFire : enemyWeaponTemplate.AmmoPerFire;
            }
        }

        public float FireCooldown
        {
            get
            {
                return ownerIsPlayer ? bodyTemplate.FireCooldown : enemyWeaponTemplate.FireCooldown;
            }
        }

        public float ReloadTime
        {
            get
            {
                return ownerIsPlayer ? bodyTemplate.ReloadTime : enemyWeaponTemplate.ReloadTime;
            }
        }

        public float FullReloadTime
        {
            get
            {
                return ownerIsPlayer ? bodyTemplate.FullReloadTime : enemyWeaponTemplate.ReloadTime;
            }
        }

        public float BurstFireInterval
        {
            get
            {
                return ownerIsPlayer ? bodyTemplate.BurstFireInterval : enemyWeaponTemplate.BurstFireInterval;
            }
        }

        public sbyte BurstShots
        {
            get
            {
                return ownerIsPlayer ? bodyTemplate.BurstShots : enemyWeaponTemplate.BurstShots;
            }
        }

        public float Range
        {
            get
            {
                return ownerIsPlayer ? barrelTemplate.Range : enemyWeaponTemplate.Range;
            }
        }

        public float MaxDistance
        {
            get
            {
                return ownerIsPlayer ? barrelTemplate.MaxDistance : enemyWeaponTemplate.MaxDistance;
            }

        }

        public float DamageMultiplier
        {
            get
            {
                return ownerIsPlayer ? barrelTemplate.DamageMultiplier : 1f;
            }
        }

        public FiringMode FiringMode
        {
            get
            {
                return ownerIsPlayer ? bodyTemplate.FiringMode : enemyWeaponTemplate.FiringMode;
            }
        }

        public bool ManualAmmoDepletion
        {
            get
            {
                return ownerIsPlayer ? bodyTemplate.ManualAmmoDepletion : enemyWeaponTemplate.ManualAmmoDepletion;
            }
        }

        public GameObject FirePrefab
        {
            get
            {
                return ownerIsPlayer ? bodyTemplate.FirePrefab : enemyWeaponTemplate.FirePrefab; ;
            }
        }

        /// <summary>
        /// Returns <see cref="EntityTeam.Player"/> if the owner is a player and <see cref="EntityTeam.Enemy"/> if the owner is an enemy.
        /// </summary>
        public EntityTeam SupportTargetEntityTeam
        {
            get
            {
                return ownerIsPlayer ? EntityTeam.Player : EntityTeam.Enemy;
            }
        }

        /// <summary>
        /// Returns the team that the weapon should target.
        /// Returns <see cref="EntityTeam.Player"/> if it's a support weapon held by the player and <see cref="EntityTeam.Enemy"/> if it's not.
        /// Returns <see cref="EntityTeam.Enemy"/> if it's a support weapon held by an enemy and <see cref="EntityTeam.Player"/> if it's not.
        /// </summary>
        public EntityTeam TargetEntityTeam
        {
            get
            {
                if (ownerIsPlayer)
                {
                    return stockTemplate.OutputType == WeaponOutputType.Support ? EntityTeam.Player : EntityTeam.Enemy;
                }
                else
                {
                    return enemyWeaponTemplate.OutputType == WeaponOutputType.Support ? EntityTeam.Enemy : EntityTeam.Player;
                }
            }
        }

        public WeaponOutputType WeaponOutputType
        {
            get
            {
                return ownerIsPlayer ? stockTemplate.OutputType : enemyWeaponTemplate.OutputType;
            }
        }

        #endregion

        #region Private variables

        private StockTemplate  stockTemplate;
        private BodyTemplate   bodyTemplate;
        private BarrelTemplate barrelTemplate;

        private EnemyWeaponTemplate enemyWeaponTemplate;

        private bool ownerIsPlayer;

        #endregion

        #region Constructors

        public WeaponStats(StockTemplate stockTemplate, BodyTemplate bodyTemplate, BarrelTemplate barrelTemplate)
        {
            this.stockTemplate  = stockTemplate;
            this.bodyTemplate   = bodyTemplate;
            this.barrelTemplate = barrelTemplate;
            this.ownerIsPlayer  = true;
        }

        public WeaponStats(EnemyWeaponTemplate enemyWeaponTemplate)
        {
            this.enemyWeaponTemplate = enemyWeaponTemplate;
            this.ownerIsPlayer       = false;
        }

        #endregion

        #region Helper methods

        public ushort GetEnemyWeaponTemplateId()
        {
            return enemyWeaponTemplate.TemplateId;
        }

        public void GetWeaponPartTemplateIds(out ushort stockTemplateId, out ushort bodyTemplateId, out ushort barrelTemplateId)
        {
            stockTemplateId  = stockTemplate.TemplateId;
            bodyTemplateId   = bodyTemplate.TemplateId;
            barrelTemplateId = barrelTemplate.TemplateId;
        }

        #endregion

    }

}
