using ArenaShooter.Combat;
using ArenaShooter.Player;
using System;
using System.Collections.Generic;

namespace ArenaShooter.Data
{

    [Serializable]
    sealed class LoadoutData
    {

        public int stockPartItemIndex;
        public int bodyPartItemIndex;
        public int barrelPartItemIndex;

        public LoadoutData(Loadout loadout, List<WeaponPartItemWrapper> weaponPartItems)
        {
            this.stockPartItemIndex  = weaponPartItems.IndexOf(loadout.StockPartItem);
            this.bodyPartItemIndex   = weaponPartItems.IndexOf(loadout.BodyPartItem);
            this.barrelPartItemIndex = weaponPartItems.IndexOf(loadout.BarrelPartItem);
        }

    }

}
