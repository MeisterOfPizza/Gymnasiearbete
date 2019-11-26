using ArenaShooter.Entities;
using ArenaShooter.Templates.Items;
using ArenaShooter.Templates.Weapons;
using UnityEngine;

namespace ArenaShooter.Combat
{

    sealed class PlayerWeaponStats : WeaponStats
    {

        #region Public properties

        public override float Mobility
        {
            get
            {
                return stockPartItem.GetFloat(StatType.Mobility);
            }
        }

        public override float Accuracy
        {
            get
            {
                return stockPartItem.GetFloat(StatType.Accuracy);
            }
        }

        public override ushort Damage
        {
            get
            {
                return bodyPartItem.GetUshort(StatType.Damage);
            }
        }

        public override short MaxAmmoPerClip
        {
            get
            {
                return bodyPartItem.GetShort(StatType.MaxAmmoPerClip);
            }
        }

        public override short MaxAmmoStock
        {
            get
            {
                return bodyPartItem.GetShort(StatType.MaxAmmoStock);
            }
        }

        public override short AmmoPerFire
        {
            get
            {
                return bodyPartItem.Template.AmmoPerFire;
            }
        }

        public override float FireCooldown
        {
            get
            {
                return bodyPartItem.GetFloat(StatType.FireCooldown);
            }
        }

        public override float ReloadTime
        {
            get
            {
                return bodyPartItem.GetFloat(StatType.ReloadTime);
            }
        }

        public override float FullReloadTime
        {
            get
            {
                return bodyPartItem.GetFloat(StatType.FullReloadTime);
            }
        }

        public override float BurstFireInterval
        {
            get
            {
                return bodyPartItem.GetFloat(StatType.BurstFireInterval);
            }
        }

        public override sbyte BurstShots
        {
            get
            {
                return bodyPartItem.GetSbyte(StatType.BurstShots);
            }
        }

        public override float Range
        {
            get
            {
                return barrelPartItem.GetFloat(StatType.Range);
            }
        }

        public override float MaxDistance
        {
            get
            {
                return barrelPartItem.GetFloat(StatType.MaxDistance);
            }
        }

        public override float DamageMultiplier
        {
            get
            {
                return barrelPartItem.GetFloat(StatType.DamageMultiplier);
            }
        }

        public override FiringMode FiringMode
        {
            get
            {
                return bodyPartItem.Template.FiringMode;
            }
        }

        public override bool ManualAmmoDepletion
        {
            get
            {
                return bodyPartItem.Template.ManualAmmoDepletion;
            }
        }

        public override GameObject FirePrefab
        {
            get
            {
                return bodyPartItem.Template.FirePrefab;
            }
        }

        public override EntityTeam SupportTargetEntityTeam
        {
            get
            {
                return EntityTeam.Player;
            }
        }

        /// <summary>
        /// Returns the team that the weapon should target.
        /// Returns <see cref="EntityTeam.Player"/> if it's a support weapon held by the player and <see cref="EntityTeam.Enemy"/> if it's not.
        /// </summary>
        public override EntityTeam TargetEntityTeam
        {
            get
            {
                return stockPartItem.Template.OutputType == WeaponOutputType.Support ? EntityTeam.Player : EntityTeam.Enemy;
            }
        }

        public override WeaponOutputType WeaponOutputType
        {
            get
            {
                return stockPartItem.Template.OutputType;
            }
        }

        #endregion

        #region Private variables

        private WeaponPartItem<StockTemplate>  stockPartItem;
        private WeaponPartItem<BodyTemplate>   bodyPartItem;
        private WeaponPartItem<BarrelTemplate> barrelPartItem;

        #endregion

        #region Constructor

        public PlayerWeaponStats(WeaponPartItem<StockTemplate> stockPartItem, WeaponPartItem<BodyTemplate> bodyPartItem, WeaponPartItem<BarrelTemplate> barrelPartItem)
        {
            this.stockPartItem  = stockPartItem;
            this.bodyPartItem   = bodyPartItem;
            this.barrelPartItem = barrelPartItem;
        }

        #endregion

        #region Helper methods

        public override ushort GetEnemyWeaponTemplateId()
        {
            return 0;
        }

        public override void GetWeaponPartTemplateIds(out ushort stockTemplateId, out ushort bodyTemplateId, out ushort barrelTemplateId)
        {
            stockTemplateId  = stockPartItem.Template.TemplateId;
            bodyTemplateId   = bodyPartItem.Template.TemplateId;
            barrelTemplateId = barrelPartItem.Template.TemplateId;
        }

        #endregion

    }

}
