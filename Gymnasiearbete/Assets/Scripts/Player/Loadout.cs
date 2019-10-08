using ArenaShooter.Combat;
using ArenaShooter.Controllers;
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

        public StockTemplate  StockTemplate  { get; private set; }
        public BodyTemplate   BodyTemplate   { get; private set; }
        public BarrelTemplate BarrelTemplate { get; private set; }

        #endregion

        #region Constructors

        public Loadout()
        {
            this.StockTemplate  = WeaponController.Singleton.DefaultStockTemplate;
            this.BodyTemplate   = WeaponController.Singleton.DefaultBodyTemplate;
            this.BarrelTemplate = WeaponController.Singleton.DefaultBarrelTemplate;
        }

        public Loadout(StockTemplate stockTemplate, BodyTemplate bodyTemplate, BarrelTemplate barrelTemplate)
        {
            this.StockTemplate  = stockTemplate;
            this.BodyTemplate   = bodyTemplate;
            this.BarrelTemplate = barrelTemplate;
        }

        #endregion

        #region Helpers

        public Weapon CreateWeapon(Transform parent)
        {
            return WeaponController.Singleton.CreateWeapon(StockTemplate, BodyTemplate, BarrelTemplate, parent);
        }

        #endregion

        #region Static helpers

        public static Loadout CreateLoadoutOfType(WeaponOutputType weaponOutputType)
        {
            var templates = WeaponController.Singleton.GetDefaultTemplatesOfType(weaponOutputType);

            return new Loadout(templates.Item1, templates.Item2, templates.Item3);
        }

        #endregion

    }

}
