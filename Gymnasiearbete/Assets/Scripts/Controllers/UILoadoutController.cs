using ArenaShooter.Combat;
using UnityEngine;

namespace ArenaShooter.Controllers
{

    class UILoadoutController : Controller<UILoadoutController>
    {

        // TEST: Test methods

        private static ushort id;

        public static Weapon GetWeapon(Transform parent)
        {
            return WeaponController.Singleton.CreateWeapon(WeaponController.Singleton.GetStockTemplate((ushort)(id * 100)), WeaponController.Singleton.GetBodyTemplate((ushort)(1000 + id * 100)), WeaponController.Singleton.GetBarrelTemplate((ushort)(2000 + id * 100)), parent);
        }

        public void SetLoadoutToRaycast()
        {
            id = 0;
        }

        public void SetLoadoutToProjectile()
        {
            id = 1;
        }

        public void SetLoadoutToElectric()
        {
            id = 2;
        }

        public void SetLoadoutToSupport()
        {
            id = 3;
        }

    }

}
