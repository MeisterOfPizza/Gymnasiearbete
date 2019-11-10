namespace ArenaShooter.Player
{

    sealed class LoadoutSlot
    {

        public byte   SlotId      { get; private set; }
        public string LoadoutName { get; private set; }
        public bool   IsUnlocked  { get; private set; }

        public Loadout Loadout { get; set; }

        public LoadoutSlot(byte slotId, string loadoutName, bool isUnlocked, Loadout loadout)
        {
            this.SlotId      = slotId;
            this.LoadoutName = loadoutName;
            this.IsUnlocked  = isUnlocked;
            this.Loadout     = loadout;
        }

    }

}
