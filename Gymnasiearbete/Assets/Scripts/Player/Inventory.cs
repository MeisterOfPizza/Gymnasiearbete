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
            WeaponPartItems = new List<WeaponPartItemWrapper>(100);
        }

        public Inventory(InventoryData inventoryData)
        {
            LoadoutSlots    = new LoadoutSlot[inventoryData.loadoutSlotsData.Length];
            WeaponPartItems = new List<WeaponPartItemWrapper>(100);

            for (int i = 0; i < inventoryData.weaponPartItemsData.Length; i++)
            {
                WeaponPartItems.Add(inventoryData.weaponPartItemsData[i].CreateWeaponPartItem());
            }

            for (int i = 0; i < LoadoutSlots.Length; i++)
            {
                LoadoutSlots[i] = new LoadoutSlot(inventoryData.loadoutSlotsData[i], WeaponPartItems);
            }
        }

        public static Inventory CreateDefault()
        {
            Inventory inventory = new Inventory();

            Loadout raycastLoadout = Loadout.CreateLoadoutOfType(WeaponOutputType.Raycasting);

            var outputTypes = Enum.GetValues(typeof(WeaponOutputType)).Cast<WeaponOutputType>().ToArray();

            for (int i = 0; i < outputTypes.Length; i++)
            {
                var loadout = outputTypes[i] == WeaponOutputType.Raycasting ? raycastLoadout : Loadout.CreateLoadoutOfType(outputTypes[i]);

                inventory.LoadoutSlots[i] = new LoadoutSlot((byte)outputTypes[i], true, loadout);

                inventory.WeaponPartItems.Add(loadout.StockPartItem);
                inventory.WeaponPartItems.Add(loadout.BodyPartItem);
                inventory.WeaponPartItems.Add(loadout.BarrelPartItem);
            }

            for (int i = outputTypes.Length; i < LoadoutController.MAX_PLAYER_LOADOUT_SLOTS; i++)
            {
                inventory.LoadoutSlots[i] = new LoadoutSlot((byte)i, true, raycastLoadout);
            }

            return inventory;
        }

        #region Helpers

        public IEnumerable<WeaponPartItemWrapper> GetStockItems(WeaponOutputType type)
        {
            return WeaponPartItems.Where(i => i.BaseTemplate.Type == WeaponPartTemplateType.Stock && i.BaseTemplate.OutputType == type);
        }

        public IEnumerable<WeaponPartItemWrapper> GetBodyItems(WeaponOutputType type)
        {
            return WeaponPartItems.Where(i => i.BaseTemplate.Type == WeaponPartTemplateType.Body && i.BaseTemplate.OutputType == type);
        }

        public IEnumerable<WeaponPartItemWrapper> GetBarrelItems(WeaponOutputType type)
        {
            return WeaponPartItems.Where(i => i.BaseTemplate.Type == WeaponPartTemplateType.Barrel && i.BaseTemplate.OutputType == type);
        }

        #endregion

    }

}
