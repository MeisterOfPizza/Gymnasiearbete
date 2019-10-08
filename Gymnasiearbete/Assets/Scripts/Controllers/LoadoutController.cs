using ArenaShooter.Extensions.Attributes;
using ArenaShooter.Player;
using ArenaShooter.Templates.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private List<LoadoutSlot> loadoutSlots = new List<LoadoutSlot>(MAX_PLAYER_LOADOUT_SLOTS);

        #endregion

        protected override void OnAwake()
        {
            // TODO: Load data from file to check which weapons etc. the player has already selected.

            CreateDefaultLoadoutSlots();
        }

        private void CreateDefaultLoadoutSlots()
        {
            var outputTypes = Enum.GetValues(typeof(WeaponOutputType)).Cast<WeaponOutputType>().ToArray();

            foreach (var outputType in outputTypes)
            {
                var loadout = Loadout.CreateLoadoutOfType(outputType);

                loadoutSlots.Add(new LoadoutSlot((byte)outputType, Enum.GetName(typeof(WeaponOutputType), outputType), true, loadout));
            }

            for (byte i = 0; i < MAX_PLAYER_LOADOUT_SLOTS - outputTypes.Length; i++)
            {
                var loadout = Loadout.CreateLoadoutOfType(WeaponOutputType.Raycasting);

                loadoutSlots.Add(new LoadoutSlot((byte)(i + outputTypes.Length), "Raycasting", true, loadout));
            }

            this.selectedLoadoutSlot = loadoutSlots[0];
        }

        public void UpdateLoadout(byte id, Loadout newLoadout)
        {
            loadoutSlots[id].Loadout = newLoadout;

            OnLoadoutChanged?.Invoke();
        }

        public void SetSelectedLoadoutSlot(LoadoutSlot selectedLoadoutSlot)
        {
            this.selectedLoadoutSlot = selectedLoadoutSlot;

            OnLoadoutChanged?.Invoke();
        }

    }

}
