using ArenaShooter.Combat;
using ArenaShooter.Controllers;
using ArenaShooter.Data;
using ArenaShooter.Templates.Items;
using ArenaShooter.Templates.Weapons;
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

        public Loadout(LoadoutData loadoutData)
        {
            this.StockPartItem  = loadoutData.stockPartItemData.CreateWeaponPartItem() as WeaponPartItem<StockTemplate>;
            this.BodyPartItem   = loadoutData.bodyPartItemData.CreateWeaponPartItem() as WeaponPartItem<BodyTemplate>;
            this.BarrelPartItem = loadoutData.barrelPartItemData.CreateWeaponPartItem() as WeaponPartItem<BarrelTemplate>;
        }

        #endregion

        #region Helpers

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

        public static Loadout CreateLoadoutOfType(WeaponOutputType weaponOutputType)
        {
            var templates = WeaponController.Singleton.GetDefaultTemplatesOfType(weaponOutputType);

            return new Loadout(new WeaponPartItem<StockTemplate>(WeaponPartItemRarity.Standard, templates.Item1), new WeaponPartItem<BodyTemplate>(WeaponPartItemRarity.Standard, templates.Item2), new WeaponPartItem<BarrelTemplate>(WeaponPartItemRarity.Standard, templates.Item3));
        }

        #endregion

    }

}
