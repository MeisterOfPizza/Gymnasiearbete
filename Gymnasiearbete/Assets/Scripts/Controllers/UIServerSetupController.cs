using ArenaShooter.Assets.Scripts.UI;
using ArenaShooter.Extensions;
using ArenaShooter.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class UIServerSetupController : Controller<UIServerSetupController>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private GameObject serverSetupMenu;

        [Space]
        [SerializeField] private CanvasGroup canvasGroup;

        [Space]
        [SerializeField] private TMP_InputField serverNameInputField;
        [SerializeField] private TMP_InputField serverPasswordInputField;
        [SerializeField] private UIMapSelect    defaultMapSelect;
        [SerializeField] private Toggle         serverIsInviteOnlyToggle;

        #endregion

        #region Private variables

        private UIMapSelect currentlySelectedMap;

        #endregion

        private void Start()
        {
            serverNameInputField.onSubmit.AddListener((string input) => serverNameInputField.SetTextWithoutNotify(ServerUtils.CurrentServerHostInfo?.SetServerName(input)));
            serverNameInputField.characterLimit      = ServerUtils.MAX_SERVER_NAME_LENGTH;
            serverNameInputField.characterValidation = TMP_InputField.CharacterValidation.Alphanumeric;
            serverNameInputField.keyboardType        = TouchScreenKeyboardType.ASCIICapable;
            serverNameInputField.contentType         = TMP_InputField.ContentType.Alphanumeric;

            serverPasswordInputField.onSubmit.AddListener((string input) => serverPasswordInputField.SetTextWithoutNotify(ServerUtils.CurrentServerHostInfo?.SetServerPassword(input)));
            serverPasswordInputField.characterLimit      = ServerUtils.MAX_SERVER_PASSWORD_LENGTH;
            serverPasswordInputField.characterValidation = TMP_InputField.CharacterValidation.Alphanumeric;
            serverPasswordInputField.keyboardType        = TouchScreenKeyboardType.ASCIICapable;
            serverPasswordInputField.contentType         = TMP_InputField.ContentType.Alphanumeric;
        }

        #region Events

        public void SetSelectedMap(UIMapSelect uiMapSelect)
        {
            EventSystem.current.SetSelectedGameObject(null);

            currentlySelectedMap = uiMapSelect;
            currentlySelectedMap.Select();
        }

        #endregion

        public void OpenServerSetup()
        {
            ServerUtils.CurrentServerHostInfo = new ServerHostInfo(defaultMapSelect.MapTemplate);

            serverSetupMenu.SetActive(true);

            serverNameInputField.text     = "";
            serverPasswordInputField.text = "";
            serverIsInviteOnlyToggle.isOn = false;

            // Deselect old UIMapSelect (if there is any) and select the default:
            SetSelectedMap(defaultMapSelect);

            canvasGroup.interactable = true;
        }

        public void CloseServerSetup()
        {
            serverSetupMenu.SetActive(false);
        }

        public void CreateServer()
        {
            if (ServerSetupSettingsAreValid())
            {
                canvasGroup.interactable = false;

                ServerUtils.CurrentServerHostInfo.SetServerName(serverNameInputField.text);
                ServerUtils.CurrentServerHostInfo.SetServerPassword(serverPasswordInputField.text);
                ServerUtils.CurrentServerHostInfo.SetServerInviteOnly(serverIsInviteOnlyToggle.isOn);
                ServerUtils.CurrentServerHostInfo.ServerMapTemplate = currentlySelectedMap.MapTemplate;

                NetworkController.Singleton.StartServer();
            }
        }

        private bool ServerSetupSettingsAreValid()
        {
            return serverNameInputField.text.Length >= ServerUtils.MIN_SERVER_NAME_LENGTH && currentlySelectedMap != null;
        }

    }

}
