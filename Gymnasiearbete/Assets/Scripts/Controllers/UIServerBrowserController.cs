using ArenaShooter.Extensions;
using ArenaShooter.Networking;
using ArenaShooter.Networking.Protocols;
using ArenaShooter.UI;
using System.Collections;
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
        [SerializeField] private UILoader      uiLoader;
        [SerializeField] private GameObject    noServersFoundText;

        [Header("Prefabs")]
        [SerializeField] private GameObject uiServerBrowserInfoPrefab;

        #endregion

        #region Public properties

        public bool HasRequestedUpdate { get; set; }

        #endregion

        public void OpenServerBrowser()
        {
            canvasGroup.interactable = true;

            ClearUIServerBrowser();

            serverBrowserMenu.SetActive(true);
            noServersFoundText.SetActive(false);

            uiLoader.Begin();

            HasRequestedUpdate = true;

            StartCoroutine("WaitToSuspendServerSearch");
        }

        public void CloseServerBrowser()
        {
            serverBrowserMenu.SetActive(false);

            uiLoader.Stop();

            HasRequestedUpdate = false;

            StopCoroutine("WaitToSuspendServerSearch");
        }

        #region Actions

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

        public void RefreshServerBrowser()
        {
            ClearUIServerBrowser();
            
            uiLoader.Begin();

            HasRequestedUpdate = true;

            noServersFoundText.SetActive(false);

            BoltNetwork.UpdateSessionList(BoltNetwork.SessionList);

            StartCoroutine("WaitToSuspendServerSearch");
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

        private IEnumerator WaitToSuspendServerSearch()
        {
            float countdown = 5f;

            while (countdown > 0 && HasRequestedUpdate)
            {
                countdown -= Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            // We did not find any servers for 5 seconds, terminate the search:
            if (HasRequestedUpdate)
            {
                NoServersFound();
            }
        }

        #endregion

        #region Events

        public void DoneLoading()
        {
            uiLoader.Stop();

            HasRequestedUpdate = false;

            StopCoroutine("WaitToSuspendServerSearch");
        }

        public void NoServersFound()
        {
            uiLoader.Stop();

            HasRequestedUpdate = false;

            noServersFoundText.SetActive(true);

            StopCoroutine("WaitToSuspendServerSearch");
        }

        #endregion

        #region Helpers

        public void AddUIServerBrowserInfo(ServerInfo serverInfo)
        {
            noServersFoundText.SetActive(false);

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
