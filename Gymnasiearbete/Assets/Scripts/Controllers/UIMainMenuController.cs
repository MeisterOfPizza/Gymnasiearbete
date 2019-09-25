using Bolt.Matchmaking;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class UIMainMenuController : Controller<UIMainMenuController>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private GameObject mainMenu;

        #endregion

        public void OpenMainMenu()
        {
            mainMenu.SetActive(true);
        }

        public void CloseMainMenu()
        {
            mainMenu.SetActive(false);
        }

    }

}
