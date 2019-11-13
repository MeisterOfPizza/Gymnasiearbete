using ArenaShooter.Combat;
using ArenaShooter.Controllers;
using ArenaShooter.Data;
using ArenaShooter.Templates.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArenaShooter.Player
{

    sealed class Inventory
    {

        public LoadoutSlot[]               LoadoutSlots    { get; private set; }
        public List<WeaponPartItemWrapper> WeaponPartItems { get; private set; }

        public Inventory()
        {
            LoadoutSlots    = new LoadoutSlot[LoadoutController.MAX_PLAYER_LOADOUT_SLOTS];
            WeaponPartItems = new List<WeaponPartItemWrapper>();
        }

        public Inventory(InventoryData inventoryData)
        {
            LoadoutSlots = new LoadoutSlot[inventoryData.loadoutSlotsData.Length];

            for (int i = 0; i < LoadoutSlots.Length; i++)
            {
                LoadoutSlots[i] = new LoadoutSlot(inventoryData.loadoutSlotsData[i]);
            }

            WeaponPartItems = new List<WeaponPartItemWrapper>(inventoryData.weaponPartItemsData.Length);
            for (int i = 0; i < WeaponPartItems.Count; i++)
            {
                WeaponPartItems.Add(inventoryData.weaponPartItemsData[i].CreateWeaponPartItem());
            }
        }

        public static Inventory CreateDefault()
        {
            Inventory inventory = new Inventory();

            var outputTypes = Enum.GetValues(typeof(WeaponOutputType)).Cast<WeaponOutputType>().ToArray();

            for (int i = 0; i < outputTypes.Length; i++)
            {
                var loadout = Loadout.CreateLoadoutOfType(outputTypes[i]);

                inventory.LoadoutSlots[i] = new LoadoutSlot((byte)outputTypes[i], Enum.GetName(typeof(WeaponOutputType), outputTypes[i]), true, loadout);
            }

            for (int i = outputTypes.Length; i < LoadoutController.MAX_PLAYER_LOADOUT_SLOTS; i++)
            {
                var loadout = Loadout.CreateLoadoutOfType(WeaponOutputType.Raycasting);

                inventory.LoadoutSlots[i] = new LoadoutSlot((byte)i, "Raycasting", true, loadout);
            }

            return inventory;
        }

    }

}
