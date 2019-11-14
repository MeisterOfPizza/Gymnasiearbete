using ArenaShooter.Combat;
using ArenaShooter.Data;
using ArenaShooter.Extensions;
using System;
using System.Collections.Generic;

namespace ArenaShooter.Player
{

    [Serializable]
    sealed class LoadoutSlot
    {

        #region Public constants

        public const int MAX_LOADOUT_SLOT_NAME_LENGTH = 15;

        #endregion

        public byte    SlotId      { get; private set; }
        public string  LoadoutName { get; private set; }
        public bool    IsUnlocked  { get; private set; }
        public Loadout Loadout     { get; private set; }

        public LoadoutSlot(byte slotId, bool isUnlocked, Loadout loadout)
        {
            this.SlotId      = slotId;
            this.LoadoutName = GetRandomLoadoutName();
            this.IsUnlocked  = isUnlocked;
            this.Loadout     = loadout;
        }

        public LoadoutSlot(LoadoutSlotData loadoutSlotData, List<WeaponPartItemWrapper> weaponPartItems)
        {
            this.SlotId      = loadoutSlotData.slotId;
            this.LoadoutName = loadoutSlotData.loadoutName;
            this.IsUnlocked  = loadoutSlotData.isUnlocked;
            this.Loadout     = new Loadout(loadoutSlotData.loadoutData, weaponPartItems);
        }

        public void Unlock()
        {
            IsUnlocked = true;
        }

        public string Rename(string newName)
        {
            if (!string.IsNullOrWhiteSpace(newName))
            {
                LoadoutName = newName.ToASCII().Truncate(MAX_LOADOUT_SLOT_NAME_LENGTH);
            }

            return LoadoutName;
        }

        public void SetLoadout(Loadout newLoadout)
        {
            Loadout = newLoadout;
        }

        public string WeaponOutputTypeString()
        {
            switch (Loadout.StockPartItem.BaseTemplate.OutputType)
            {
                case Templates.Weapons.WeaponOutputType.Raycasting:
                    return "Kinetic";
                case Templates.Weapons.WeaponOutputType.Projectile:
                    return "Projectile";
                case Templates.Weapons.WeaponOutputType.Electric:
                    return "Electric";
                case Templates.Weapons.WeaponOutputType.Support:
                    return "Support";
                default:
                    return "Loadout Slot";
            }
        }

        public static string GetRandomLoadoutName()
        {
            string[] loadoutNamePresets = new string[] { "Ranger", "Sniper", "Assassin", "Spy", "Soldier", "Berserk", "Grunt", "Boogeyman", "Reaper", "Warrior", "Fighter", "Officer", "Protector" };
            return loadoutNamePresets[UnityEngine.Random.Range(0, loadoutNamePresets.Length)];
        }

    }

}
