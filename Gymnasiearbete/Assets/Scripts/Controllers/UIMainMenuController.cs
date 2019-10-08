using ArenaShooter.UI;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class UIMainMenuController : Controller<UIMainMenuController>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private GameObject mainMenu;

        [Space]
        [SerializeField] private GameObject loadingOnlineTextContainer;
        [SerializeField] private UILoader   loadingOnlineLoader;

        #endregion

        public void OpenMainMenu()
        {
            mainMenu.SetActive(true);

            loadingOnlineTextContainer.SetActive(false);
        }

        public void CloseMainMenu()
        {
            mainMenu.SetActive(false);
        }

        public void BeginOnlineLoader()
        {
            loadingOnlineTextContainer.SetActive(true);
            loadingOnlineLoader.Begin();
        }

        public void StopOnlineLoader()
        {
            loadingOnlineTextContainer.SetActive(false);
            loadingOnlineLoader.Stop();
        }

    }

}
