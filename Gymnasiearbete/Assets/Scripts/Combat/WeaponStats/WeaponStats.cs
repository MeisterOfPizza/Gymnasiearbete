using ArenaShooter.Entities;
using ArenaShooter.Templates.Weapons;
using UnityEngine;

namespace ArenaShooter.Combat
{

    abstract class WeaponStats
    {

        #region Public properties

        public abstract float      Mobility            { get; }
        public abstract float      Accuracy            { get; }
        public abstract ushort     Damage              { get; }
        public abstract short      MaxAmmoPerClip      { get; }
        public abstract short      MaxAmmoStock        { get; }
        public abstract short      AmmoPerFire         { get; }
        public abstract float      FireCooldown        { get; }
        public abstract float      ReloadTime          { get; }
        public abstract float      FullReloadTime      { get; }
        public abstract float      BurstFireInterval   { get; }
        public abstract sbyte      BurstShots          { get; }
        public abstract float      Range               { get; }
        public abstract float      MaxDistance         { get; }
        public abstract float      DamageMultiplier    { get; }
        public abstract FiringMode FiringMode          { get; }
        public abstract bool       ManualAmmoDepletion { get; }
        public abstract GameObject FirePrefab          { get; }

        /// <summary>
        /// Returns <see cref="EntityTeam.Player"/> if the owner is a player and <see cref="EntityTeam.Enemy"/> if the owner is an enemy.
        /// </summary>
        public abstract EntityTeam       SupportTargetEntityTeam { get; }
        /// <summary>
        /// Returns the team that the weapon should target.
        /// Returns <see cref="EntityTeam.Player"/> if it's a support weapon held by the player and <see cref="EntityTeam.Enemy"/> if it's not.
        /// Returns <see cref="EntityTeam.Enemy"/> if it's a support weapon held by an enemy and <see cref="EntityTeam.Player"/> if it's not.
        /// </summary>
        public abstract EntityTeam       TargetEntityTeam        { get; }
        public abstract WeaponOutputType WeaponOutputType        { get; }

        #endregion

        #region Helper methods

        public abstract ushort GetEnemyWeaponTemplateId();
        public abstract void   GetWeaponPartTemplateIds(out ushort stockTemplateId, out ushort bodyTemplateId, out ushort barrelTemplateId);

        #endregion

    }

}
