using ArenaShooter.Extensions.Attributes;
using ArenaShooter.Player;
using System;

namespace ArenaShooter.Controllers
{

    [Persistent]
    class LoadoutController : Controller<LoadoutController>
    {

        #region Public constants

        public const int MAX_PLAYER_LOADOUT_SLOTS = 5;

        #endregion

        #region Public properties

        public Action OnLoadoutChanged { get; set; }

        public Loadout CurrentLoadout
        {
            get
            {
                return selectedLoadoutSlot.Loadout;
            }
        }

        #endregion

        #region Private variables

        private LoadoutSlot selectedLoadoutSlot;

        #endregion

        protected override void OnAwake()
        {
            Profile.Load();

            selectedLoadoutSlot = Profile.Inventory.LoadoutSlots[0];
        }

        private void OnApplicationQuit()
        {
            Profile.Save();
        }

        public void SetSelectedLoadoutSlot(LoadoutSlot loadoutSlot)
        {
            this.selectedLoadoutSlot = loadoutSlot;

            OnLoadoutChanged?.Invoke();
        }

    }

}
