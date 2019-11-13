using ArenaShooter.Player;
using System;

namespace ArenaShooter.Data
{

    [Serializable]
    sealed class LoadoutData
    {

        public WeaponPartItemData stockPartItemData;
        public WeaponPartItemData bodyPartItemData;
        public WeaponPartItemData barrelPartItemData;

        public LoadoutData(Loadout loadout)
        {
            this.stockPartItemData  = new WeaponPartItemData(loadout.StockPartItem);
            this.bodyPartItemData   = new WeaponPartItemData(loadout.BodyPartItem);
            this.barrelPartItemData = new WeaponPartItemData(loadout.BarrelPartItem);
        }

    }

}
