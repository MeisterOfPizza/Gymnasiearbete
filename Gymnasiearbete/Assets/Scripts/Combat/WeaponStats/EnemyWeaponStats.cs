using ArenaShooter.Entities;
using ArenaShooter.Templates.Enemies;
using ArenaShooter.Templates.Weapons;
using UnityEngine;

namespace ArenaShooter.Combat
{

    sealed class EnemyWeaponStats : WeaponStats
    {

        #region Public properties

        public override float Mobility
        {
            get
            {
                return enemyWeaponTemplate.Mobility;
            }
        }

        public override float Accuracy
        {
            get
            {
                return enemyWeaponTemplate.Accuracy;
            }
        }

        public override ushort Damage
        {
            get
            {
                return enemyWeaponTemplate.Damage;
            }
        }

        public override short MaxAmmoPerClip
        {
            get
            {
                return enemyWeaponTemplate.MaxAmmoPerClip;
            }
        }

        public override short MaxAmmoStock
        {
            get
            {
                return short.MaxValue;
            }
        }

        public override short AmmoPerFire
        {
            get
            {
                return enemyWeaponTemplate.AmmoPerFire;
            }
        }

        public override float FireCooldown
        {
            get
            {
                return enemyWeaponTemplate.FireCooldown;
            }
        }

        public override float ReloadTime
        {
            get
            {
                return enemyWeaponTemplate.ReloadTime;
            }
        }

        public override float FullReloadTime
        {
            get
            {
                return enemyWeaponTemplate.ReloadTime;
            }
        }

        public override float BurstFireInterval
        {
            get
            {
                return enemyWeaponTemplate.BurstFireInterval;
            }
        }

        public override sbyte BurstShots
        {
            get
            {
                return enemyWeaponTemplate.BurstShots;
            }
        }

        public override float Range
        {
            get
            {
                return enemyWeaponTemplate.Range;
            }
        }

        public override float MaxDistance
        {
            get
            {
                return enemyWeaponTemplate.MaxDistance;
            }
        }

        public override float DamageMultiplier
        {
            get
            {
                return 1f;
            }
        }

        public override FiringMode FiringMode
        {
            get
            {
                return enemyWeaponTemplate.FiringMode;
            }
        }

        public override bool ManualAmmoDepletion
        {
            get
            {
                return enemyWeaponTemplate.ManualAmmoDepletion;
            }
        }

        public override GameObject FirePrefab
        {
            get
            {
                return enemyWeaponTemplate.FirePrefab;
            }
        }

        public override EntityTeam SupportTargetEntityTeam
        {
            get
            {
                return EntityTeam.Enemy;
            }
        }

        /// <summary>
        /// Returns the team that the weapon should target.
        /// Returns <see cref="EntityTeam.Enemy"/> if it's a support weapon held by an enemy and <see cref="EntityTeam.Player"/> if it's not.
        /// </summary>
        public override EntityTeam TargetEntityTeam
        {
            get
            {
                return enemyWeaponTemplate.OutputType == WeaponOutputType.Support ? EntityTeam.Enemy : EntityTeam.Player;
            }
        }

        public override WeaponOutputType WeaponOutputType
        {
            get
            {
                return enemyWeaponTemplate.OutputType;
            }
        }

        #endregion

        #region Private variables

        private EnemyWeaponTemplate enemyWeaponTemplate;

        #endregion

        #region Constructor

        public EnemyWeaponStats(EnemyWeaponTemplate enemyWeaponTemplate)
        {
            this.enemyWeaponTemplate = enemyWeaponTemplate;
        }

        #endregion

        #region Helper methods

        public override ushort GetEnemyWeaponTemplateId()
        {
            return enemyWeaponTemplate.TemplateId;
        }

        public override void GetWeaponPartTemplateIds(out ushort stockTemplateId, out ushort bodyTemplateId, out ushort barrelTemplateId)
        {
            stockTemplateId  = 0;
            bodyTemplateId   = 0;
            barrelTemplateId = 0;
        }

        #endregion

    }

}
