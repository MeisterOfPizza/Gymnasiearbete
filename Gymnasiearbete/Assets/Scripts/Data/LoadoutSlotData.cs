using ArenaShooter.Player;
using System;

namespace ArenaShooter.Data
{

    [Serializable]
    sealed class LoadoutSlotData
    {

        public byte        slotId;
        public string      loadoutName;
        public bool        isUnlocked;
        public LoadoutData loadoutData;

        public LoadoutSlotData(LoadoutSlot loadoutSlot)
        {
            this.slotId      = loadoutSlot.SlotId;
            this.loadoutName = loadoutSlot.LoadoutName;
            this.isUnlocked  = loadoutSlot.IsUnlocked;
            this.loadoutData = new LoadoutData(loadoutSlot.Loadout);
        }

    }

}
