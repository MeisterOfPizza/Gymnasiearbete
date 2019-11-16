using ArenaShooter.Player;
using ArenaShooter.Templates.Weapons;
using UnityEngine;

namespace ArenaShooter.Combat
{

    sealed class BuiltWeapon : MonoBehaviour
    {

        public WeaponPart StockPart
        {
            get
            {
                return stockPart;
            }
        }

        public WeaponPart BodyPart
        {
            get
            {
                return bodyPart;
            }
        }

        public WeaponPart BarrelPart
        {
            get
            {
                return barrelPart;
            }
        }

        private WeaponPart stockPart;
        private WeaponPart bodyPart;
        private WeaponPart barrelPart;

        private void Initialize(WeaponPart stockPart, WeaponPart bodyPart, WeaponPart barrelPart)
        {
            this.stockPart  = stockPart;
            this.bodyPart   = bodyPart;
            this.barrelPart = barrelPart;
        }

        public static BuiltWeapon AssembleWeapon(StockTemplate stockTemplate, BodyTemplate bodyTemplate, BarrelTemplate barrelTemplate, Transform parent)
        {
            BuiltWeapon builtWeapon = new GameObject("Built Weapon", typeof(BuiltWeapon)).GetComponent<BuiltWeapon>();
            builtWeapon.transform.SetParent(parent, false);

            var stockPart  = Instantiate(stockTemplate.WeaponPartPrefab, builtWeapon.transform).GetComponent<WeaponPart>();
            var bodyPart   = Instantiate(bodyTemplate.WeaponPartPrefab, builtWeapon.transform).GetComponent<WeaponPart>();
            var barrelPart = Instantiate(barrelTemplate.WeaponPartPrefab, builtWeapon.transform).GetComponent<WeaponPart>();

            stockPart.Initialize(stockTemplate, false);
            bodyPart.Initialize(bodyTemplate, false);
            barrelPart.Initialize(barrelTemplate, false);

            bodyPart.AttachStock(stockPart);
            bodyPart.AttachBarrel(barrelPart);

            builtWeapon.Initialize(stockPart, bodyPart, barrelPart);

            return builtWeapon;
        }

        public static BuiltWeapon AssembleWeapon(Loadout loadout, Transform parent, bool shouldHighlight = false)
        {
            BuiltWeapon builtWeapon = new GameObject("Built Weapon", typeof(BuiltWeapon)).GetComponent<BuiltWeapon>();
            builtWeapon.transform.SetParent(parent, false);

            var stockPart  = Instantiate(loadout.StockPartItem.Template.WeaponPartPrefab, builtWeapon.transform).GetComponent<WeaponPart>();
            var bodyPart   = Instantiate(loadout.BodyPartItem.Template.WeaponPartPrefab, builtWeapon.transform).GetComponent<WeaponPart>();
            var barrelPart = Instantiate(loadout.BarrelPartItem.Template.WeaponPartPrefab, builtWeapon.transform).GetComponent<WeaponPart>();

            stockPart.Initialize(loadout.StockPartItem.Template, shouldHighlight);
            bodyPart.Initialize(loadout.BodyPartItem.Template, shouldHighlight);
            barrelPart.Initialize(loadout.BarrelPartItem.Template, shouldHighlight);

            bodyPart.AttachStock(stockPart);
            bodyPart.AttachBarrel(barrelPart);

            builtWeapon.Initialize(stockPart, bodyPart, barrelPart);

            return builtWeapon;
        }

    }

}
