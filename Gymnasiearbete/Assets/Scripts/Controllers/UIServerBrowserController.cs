using ArenaShooter.Extensions;
using ArenaShooter.Networking;
using ArenaShooter.Networking.Protocols;
using ArenaShooter.UI;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class UIServerBrowserController : Controller<UIServerBrowserController>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private GameObject serverBrowserMenu;

        [Space]
        [SerializeField] private CanvasGroup canvasGroup;

        [Space]
        [SerializeField] private RectTransform uiServerBrowserInfoContainer;

        [Header("Prefabs")]
        [SerializeField] private GameObject uiServerBrowserInfoPrefab;

        #endregion

        public void OpenServerBrowser()
        {
            canvasGroup.interactable = true;

            ClearUIServerBrowser();

            serverBrowserMenu.SetActive(true);
        }

        public void CloseServerBrowser()
        {
            serverBrowserMenu.SetActive(false);
        }

        public void JoinSession(ServerInfo serverInfo)
        {
            ServerUtils.CurrentServerInfo = serverInfo;

            if (serverInfo.Info.ServerIsPassworded)
            {
                UIConnectAuthController.Singleton.OpenConnectAuthWindow(serverInfo.UdpSession, canvasGroup);
            }
            else
            {
                BoltNetwork.Connect(serverInfo.UdpSession, new UserToken(UserUtils.GetUserId(), ""));
            }
        }

        /// <summary>
        /// Disconnects the client and closes the server browser.
        /// </summary>
        public void Disconnect()
        {
            if (BoltNetwork.IsRunning)
            {
                canvasGroup.interactable = false;

                BoltLauncher.Shutdown();
            }
        }

        #region Helpers

        public void AddUIServerBrowserInfo(ServerInfo serverInfo)
        {
            var uiServerInfo = Instantiate(uiServerBrowserInfoPrefab, uiServerBrowserInfoContainer).GetComponent<UIServerBrowserInfo>();
            uiServerInfo.Initialize(serverInfo);
        }

        public void ClearUIServerBrowser()
        {
            uiServerBrowserInfoContainer.Clear();
        }

        #endregion

    }

}
