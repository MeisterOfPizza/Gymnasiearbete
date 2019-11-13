using ArenaShooter.Player;
using System;

#pragma warning disable 0649

namespace ArenaShooter.Data
{

    [Serializable]
    sealed class InventoryData
    {

        public LoadoutSlotData[]    loadoutSlotsData;
        public WeaponPartItemData[] weaponPartItemsData;

        public InventoryData(Inventory inventory)
        {
            loadoutSlotsData = new LoadoutSlotData[inventory.LoadoutSlots.Length];

            for (int i = 0; i < loadoutSlotsData.Length; i++)
            {
                loadoutSlotsData[i] = new LoadoutSlotData(inventory.LoadoutSlots[i]);
            }

            weaponPartItemsData = new WeaponPartItemData[inventory.WeaponPartItems.Count];
            for (int i = 0; i < weaponPartItemsData.Length; i++)
            {
                weaponPartItemsData[i] = new WeaponPartItemData(inventory.WeaponPartItems[i]);
            }
        }

    }

}
