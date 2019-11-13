using ArenaShooter.Templates.Items;
using ArenaShooter.Templates.Weapons;
using System.Collections.Generic;

namespace ArenaShooter.Combat
{

    /// <summary>
    /// Wrapper class to pass weapon part items without needing to specify the generic type T.
    /// </summary>
    abstract class WeaponPartItemWrapper
    {

        public WeaponPartItemRarity Rarity { get; protected set; }

        public abstract WeaponPartTemplate BaseTemplate { get; }

        /// <summary>
        /// The stat type values merged with the template stat values.
        /// </summary>
        public Dictionary<StatType, float> StatTypeValues { get; protected set; }

        public abstract float  GetFloat(StatType statType);
        public abstract int    GetInt(StatType statType);
        public abstract short  GetShort(StatType statType);
        public abstract ushort GetUshort(StatType statType);
        public abstract sbyte  GetSbyte(StatType statType);

    }

}
