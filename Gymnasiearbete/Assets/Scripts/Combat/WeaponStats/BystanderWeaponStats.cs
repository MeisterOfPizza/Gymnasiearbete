using ArenaShooter.Entities;
using ArenaShooter.Templates.Weapons;
using UnityEngine;

namespace ArenaShooter.Combat
{

    /// <summary>
    /// This weapon stats class is used by
    /// clients on the local machine.
    /// </summary>
    sealed class BystanderWeaponStats : WeaponStats
    {

        // * NOTE *
        // As this class won't really be used other than getting prefabs and ammo the formatting of this class is not that important.
        // ********

        public override float            Mobility                { get { return stockTemplate.Mobility; } }
        public override float            Accuracy                { get { return stockTemplate.Accuracy; } }
        public override float            MaxAngleOffset          { get { return stockTemplate.MaxAngleOffset; } }
        public override ushort           Damage                  { get { return bodyTemplate.Damage; } }
        public override short            MaxAmmoPerClip          { get { return bodyTemplate.MaxAmmoPerClip; } }
        public override short            MaxAmmoStock            { get { return bodyTemplate.MaxAmmoStock; } }
        public override short            AmmoPerFire             { get { return bodyTemplate.AmmoPerFire; } }
        public override float            FireCooldown            { get { return bodyTemplate.FireCooldown; } }
        public override float            ReloadTime              { get { return bodyTemplate.ReloadTime; } }
        public override float            FullReloadTime          { get { return bodyTemplate.FullReloadTime; } }
        public override float            BurstFireInterval       { get { return bodyTemplate.BurstFireInterval; } }
        public override sbyte            BurstShots              { get { return bodyTemplate.BurstShots; } }
        public override float            Range                   { get { return barrelTemplate.Range; } }
        public override float            MaxDistance             { get { return barrelTemplate.MaxDistance; } }
        public override float            DamageMultiplier        { get { return barrelTemplate.DamageMultiplier; } }
        public override FiringMode       FiringMode              { get { return bodyTemplate.FiringMode; } }
        public override bool             ManualAmmoDepletion     { get { return bodyTemplate.ManualAmmoDepletion; } }
        public override EntityTeam       SupportTargetEntityTeam { get { return EntityTeam.Player; } }
        public override EntityTeam       TargetEntityTeam        { get { return stockTemplate.OutputType == WeaponOutputType.Support ? EntityTeam.Player : EntityTeam.Enemy; } }
        public override WeaponOutputType WeaponOutputType        { get { return stockTemplate.OutputType; } }

        public override GameObject FirePrefab
        {
            get
            {
                return bodyTemplate.FirePrefab;
            }
        }

        #region Private variables

        private StockTemplate  stockTemplate;
        private BodyTemplate   bodyTemplate;
        private BarrelTemplate barrelTemplate;

        #endregion

        public BystanderWeaponStats(StockTemplate stockTemplate, BodyTemplate bodyTemplate, BarrelTemplate barrelTemplate)
        {
            this.stockTemplate  = stockTemplate;
            this.bodyTemplate   = bodyTemplate;
            this.barrelTemplate = barrelTemplate;
        }

        public override ushort GetEnemyWeaponTemplateId()
        {
            return 0;
        }

        public override void GetWeaponPartTemplateIds(out ushort stockTemplateId, out ushort bodyTemplateId, out ushort barrelTemplateId)
        {
            stockTemplateId  = stockTemplate.TemplateId;
            bodyTemplateId   = bodyTemplate.TemplateId;
            barrelTemplateId = barrelTemplate.TemplateId;
        }

    }

}
