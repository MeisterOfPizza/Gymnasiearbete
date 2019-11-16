using System;

namespace ArenaShooter.Data
{

    [Serializable]
    sealed class ProfileData
    {

        public int    totalKills  = 0;
        public int    totalDeaths = 0;
        public int    totalShots  = 0;
        public double timePlayed  = 0;

        public InventoryData inventoryData;
        public int           selectedLoadoutSlotIndex = 0;

        public ProfileData(int totalKills, int totalDeaths, int totalShots, double timePlayed, InventoryData inventoryData, int selectedLoadoutSlotIndex)
        {
            this.totalKills               = totalKills;
            this.totalDeaths              = totalDeaths;
            this.totalShots               = totalShots;
            this.timePlayed               = timePlayed;
            this.inventoryData            = inventoryData;
            this.selectedLoadoutSlotIndex = selectedLoadoutSlotIndex;
        }

    }

}
