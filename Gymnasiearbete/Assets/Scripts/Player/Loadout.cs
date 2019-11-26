using ArenaShooter.Combat;
using ArenaShooter.Controllers;
using ArenaShooter.Data;
using ArenaShooter.Extensions;
using ArenaShooter.Templates.Items;
using ArenaShooter.Templates.Weapons;
using System.Collections.Generic;
using UnityEngine;

namespace ArenaShooter.Player
{

    /// <summary>
    /// Loadout class for the player to save weapon part templates, colors, cosmetic items and so on.
    /// </summary>
    sealed class Loadout
    {

        #region Public properites

        public WeaponPartItem<StockTemplate>  StockPartItem  { get; private set; }
        public WeaponPartItem<BodyTemplate>   BodyPartItem   { get; private set; }
        public WeaponPartItem<BarrelTemplate> BarrelPartItem { get; private set; }

        #endregion

        #region Constructors

        public Loadout(WeaponPartItem<StockTemplate> stockPartItem, WeaponPartItem<BodyTemplate> bodyPartItem, WeaponPartItem<BarrelTemplate> barrelPartItem)
        {
            this.StockPartItem  = stockPartItem;
            this.BodyPartItem   = bodyPartItem;
            this.BarrelPartItem = barrelPartItem;
        }

        public Loadout(LoadoutData loadoutData, List<WeaponPartItemWrapper> weaponPartItems)
        {
            this.StockPartItem  = weaponPartItems[loadoutData.stockPartItemIndex] as WeaponPartItem<StockTemplate>;
            this.BodyPartItem   = weaponPartItems[loadoutData.bodyPartItemIndex] as WeaponPartItem<BodyTemplate>;
            this.BarrelPartItem = weaponPartItems[loadoutData.barrelPartItemIndex] as WeaponPartItem<BarrelTemplate>;
        }

        #endregion

        #region Helpers

        public void SwitchWeaponPartItem(WeaponPartItemWrapper weaponPartItem)
        {
            switch (weaponPartItem.BaseTemplate.Type)
            {
                case WeaponPartTemplateType.Stock:
                    StockPartItem = weaponPartItem as WeaponPartItem<StockTemplate>;
                    break;
                case WeaponPartTemplateType.Body:
                    BodyPartItem = weaponPartItem as WeaponPartItem<BodyTemplate>;
                    break;
                case WeaponPartTemplateType.Barrel:
                    BarrelPartItem = weaponPartItem as WeaponPartItem<BarrelTemplate>;
                    break;
            }
        }

        public WeaponPartItem<T> SwitchWeaponPartItem<T>(WeaponPartItem<T> weaponPartItem) where T : WeaponPartTemplate
        {
            switch (weaponPartItem.Template.Type)
            {
                case WeaponPartTemplateType.Stock:
                    {
                        var oldWeaponPartItem = StockPartItem;

                        StockPartItem = weaponPartItem as WeaponPartItem<StockTemplate>;

                        return oldWeaponPartItem as WeaponPartItem<T>;
                    }
                case WeaponPartTemplateType.Body:
                    {
                        var oldWeaponPartItem = BodyPartItem;

                        BodyPartItem = weaponPartItem as WeaponPartItem<BodyTemplate>;

                        return oldWeaponPartItem as WeaponPartItem<T>;
                    }
                case WeaponPartTemplateType.Barrel:
                    {
                        var oldWeaponPartItem = BarrelPartItem;

                        BarrelPartItem = weaponPartItem as WeaponPartItem<BarrelTemplate>;

                        return oldWeaponPartItem as WeaponPartItem<T>;
                    }
            }

            return null;
        }

        public Weapon CreateWeapon(Transform parent)
        {
            return WeaponController.Singleton.CreateWeapon(StockPartItem, BodyPartItem, BarrelPartItem, parent);
        }

        #endregion

        #region Static helpers

        /// <summary>
        /// Creates a loadout from default parts of the <paramref name="weaponOutputType"/> type.
        /// </summary>
        public static Loadout CreateLoadoutOfType(WeaponOutputType weaponOutputType)
        {
            var templates = WeaponController.Singleton.GetDefaultTemplatesOfType(weaponOutputType);

            return new Loadout(new WeaponPartItem<StockTemplate>(WeaponPartItemRarity.Standard, templates.Item1), new WeaponPartItem<BodyTemplate>(WeaponPartItemRarity.Standard, templates.Item2), new WeaponPartItem<BarrelTemplate>(WeaponPartItemRarity.Standard, templates.Item3));
        }

        /// <summary>
        /// Creates a loadout from random parts of the <paramref name="weaponOutputType"/> type.
        /// </summary>
        public static Loadout CreateRandomLoadoutOfType(WeaponOutputType weaponOutputType)
        {
            var stockItem  = Profile.Inventory.GetStockItems(weaponOutputType).TakeRandom() as WeaponPartItem<StockTemplate>;
            var bodyItem   = Profile.Inventory.GetBodyItems(weaponOutputType).TakeRandom() as WeaponPartItem<BodyTemplate>;
            var barrelItem = Profile.Inventory.GetBarrelItems(weaponOutputType).TakeRandom() as WeaponPartItem<BarrelTemplate>;

            return new Loadout(stockItem, bodyItem, barrelItem);
        }

        #endregion

    }

}
