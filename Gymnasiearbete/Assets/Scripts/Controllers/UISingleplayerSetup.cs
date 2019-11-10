using ArenaShooter.Assets.Scripts.UI;
using ArenaShooter.Extensions;
using ArenaShooter.Networking;
using UnityEngine;
using UnityEngine.EventSystems;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class UISingleplayerSetup : Controller<UISingleplayerSetup>
    {

        

        #region Editor

        [Header("References")]
        [SerializeField] private GameObject singleplayerSetupMenu;

        [Space]
        [SerializeField] private CanvasGroup canvasGroup;

        [Space]
        [SerializeField] private UIMapSelect defaultMapSelect;

        #endregion

        #region Private variables

        private UIMapSelect currentlySelectedMap;

        #endregion

        #region Events

        public void SetSelectedMap(UIMapSelect uiMapSelect)
        {
            EventSystem.current.SetSelectedGameObject(null);

            currentlySelectedMap = uiMapSelect;
            currentlySelectedMap.Select();
        }

        #endregion

        public void OpenSingleplayerSetup()
        {
            ServerUtils.CurrentServerHostInfo = new ServerHostInfo(defaultMapSelect.MapTemplate);

            singleplayerSetupMenu.SetActive(true);

            // Deselect old UIMapSelect (if there is any) and select the default:
            SetSelectedMap(defaultMapSelect);

            canvasGroup.interactable = true;
        }

        public void CloseSingleplayerSetup()
        {
            singleplayerSetupMenu.SetActive(false);
        }

        public void StartGame()
        {
            canvasGroup.interactable = false;

            ServerUtils.CurrentServerHostInfo.SetServerName("Singleplayer");
            ServerUtils.CurrentServerHostInfo.SetServerPassword("0");
            ServerUtils.CurrentServerHostInfo.SetServerInviteOnly(false);
            ServerUtils.CurrentServerHostInfo.SetServerInLobby(false);
            ServerUtils.CurrentServerHostInfo.ServerMapTemplate = currentlySelectedMap.MapTemplate;

            BoltNetwork.LoadScene(ServerUtils.CurrentServerHostInfo.ServerMapTemplate.SceneName);
        }

    }

}
