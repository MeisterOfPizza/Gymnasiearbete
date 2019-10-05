using ArenaShooter.Player;
using ArenaShooter.Templates.Weapons;

namespace ArenaShooter.Controllers
{

    class UILoadoutController : Controller<UILoadoutController>
    {

        public void SetLoadoutToRaycast()
        {
            LoadoutController.Singleton.UpdateLoadout(0, Loadout.CreateLoadoutOfType(WeaponOutputType.Raycasting));
        }

        public void SetLoadoutToProjectile()
        {
            LoadoutController.Singleton.UpdateLoadout(0, Loadout.CreateLoadoutOfType(WeaponOutputType.Projectile));
        }

        public void SetLoadoutToElectric()
        {
            LoadoutController.Singleton.UpdateLoadout(0, Loadout.CreateLoadoutOfType(WeaponOutputType.Electric));
        }

        public void SetLoadoutToSupport()
        {
            LoadoutController.Singleton.UpdateLoadout(0, Loadout.CreateLoadoutOfType(WeaponOutputType.Support));
        }

    }

}
