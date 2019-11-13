using ArenaShooter.Data;
using System;

namespace ArenaShooter.Player
{

    [Serializable]
    sealed class LoadoutSlot
    {

        public byte    SlotId      { get; private set; }
        public string  LoadoutName { get; private set; }
        public bool    IsUnlocked  { get; private set; }
        public Loadout Loadout     { get; private set; }

        public LoadoutSlot(byte slotId, string loadoutName, bool isUnlocked, Loadout loadout)
        {
            this.SlotId      = slotId;
            this.LoadoutName = loadoutName;
            this.IsUnlocked  = isUnlocked;
            this.Loadout     = loadout;
        }

        public LoadoutSlot(LoadoutSlotData loadoutSlotData)
        {
            this.SlotId      = loadoutSlotData.slotId;
            this.LoadoutName = loadoutSlotData.loadoutName;
            this.IsUnlocked  = loadoutSlotData.isUnlocked;
            this.Loadout     = new Loadout(loadoutSlotData.loadoutData);
        }

        public void Unlock()
        {
            IsUnlocked = true;
        }

        public void Rename(string newName)
        {
            LoadoutName = newName;
        }

    }

}
