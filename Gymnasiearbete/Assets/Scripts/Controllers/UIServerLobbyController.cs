using ArenaShooter.Extensions;
using ArenaShooter.Player;
using ArenaShooter.UI;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class UIServerLobbyController : Controller<UIServerLobbyController>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private GameObject serverLobbyMenu;

        [Space]
        [SerializeField] private CanvasGroup canvasGroup;

        [Space]
        [SerializeField] private RectTransform uiLobbyPlayerInfoContainer;

        [Space]
        [SerializeField] private TMP_Text serverNameText;
        [SerializeField] private Button   readyButton;
        [SerializeField] private TMP_Text readyButtonText;
        [SerializeField] private Button   hostStartMatchButton;

        [Space]
        [SerializeField] private TMP_Text matchStartCountdownText;

        [Header("Prefabs")]
        [SerializeField] private GameObject uiLobbyPlayerInfoPrefab;

        #endregion

        #region Private variables

        private LobbyPlayer lobbyPlayer;

        #endregion

        #region Initialization

        public void OpenLobby()
        {
            serverLobbyMenu.SetActive(true);

            canvasGroup.interactable = true;

            uiLobbyPlayerInfoContainer.Clear();

            if (BoltNetwork.IsServer)
            {
                serverNameText.text = string.Format("Lobby | {0} - Hosted by {1}", ServerUtils.ServerName, UserUtils.GetUsername());
            }
            else
            {
                serverNameText.text = string.Format("Lobby | {0} - Hosted by {1}", ServerUtils.CurrentServerInfo.Info.ServerName, ServerUtils.CurrentServerInfo.Info.HostUsername);
            }

            hostStartMatchButton.gameObject.SetActive(BoltNetwork.IsServer);
            hostStartMatchButton.interactable = true;

            readyButton.gameObject.SetActive(!BoltNetwork.IsServer);

            matchStartCountdownText.text = "";
        }

        public void CloseLobby()
        {
            serverLobbyMenu.SetActive(false);
        }

        public void SetLobbyPlayer(LobbyPlayer lobbyPlayer)
        {
            this.lobbyPlayer = lobbyPlayer;

            if (BoltNetwork.IsServer)
            {
                CheckIfClientsAreReady();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Starts the match start countdown.
        /// </summary>
        public void HostStartMatchCountdown()
        {
            // Check if the local machine is the host for security reasons:
            if (BoltNetwork.IsServer)
            {
                lobbyPlayer.HostStartMatchCountdown();

                LockLobbyScreen();
            }
        }

        /// <summary>
        /// Locks the lobby screen.
        /// </summary>
        public void LockLobbyScreen()
        {
            canvasGroup.interactable = false;
        }

        /// <summary>
        /// Loads the actual scene that the match will be taking place in.
        /// </summary>
        public void StartMatch()
        {
            BoltNetwork.LoadScene(ServerUtils.ServerMapTemplate.SceneName);
        }

        /// <summary>
        /// Updates the match start countdown text.
        /// </summary>
        public void UpdateMatchStartCountdown(int countdown)
        {
            matchStartCountdownText.text = "Starts in .. " + countdown;
        }

        /// <summary>
        /// Disconnects the client OR shuts down the server.
        /// </summary>
        public void Disconnect()
        {
            if (BoltNetwork.IsConnected)
            {
                canvasGroup.interactable = false;

                BoltNetwork.Shutdown();
            }
        }

        /// <summary>
        /// Event called whenever the <see cref="readyButton"/> is clicked.
        /// </summary>
        public void ToggleReady()
        {
            lobbyPlayer.ToggleReady();
            readyButtonText.text = lobbyPlayer.state.Ready ? "Unready" : "Ready up";
        }

        #endregion

        #region Helpers

        public UILobbyPlayerInfo CreateUILobbyPlayerInfo(LobbyPlayer lobbyPlayer)
        {
            UILobbyPlayerInfo uiLobbyPlayerInfo = Instantiate(uiLobbyPlayerInfoPrefab, uiLobbyPlayerInfoContainer).GetComponent<UILobbyPlayerInfo>();

            return uiLobbyPlayerInfo;
        }

        /// <summary>
        /// Enables the start game button if all clients are ready, otherwise this disables it.
        /// Only run this on the host.
        /// </summary>
        public void CheckIfClientsAreReady()
        {
            hostStartMatchButton.interactable = ServerLobbyController.Singleton.LobbyPlayers.All(p => p.state.Ready);
        }

        #endregion

    }

}
