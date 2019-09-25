using ArenaShooter.Extensions;
using ArenaShooter.Networking.Protocols;
using TMPro;
using UdpKit;
using UnityEngine;

namespace ArenaShooter.Controllers
{

    class UIConnectAuthController : Controller<UIConnectAuthController>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private GameObject connectAuthWindow;

        [Space]
        [SerializeField] private TMP_InputField passwordInputField;

        #endregion

        #region Public properties

        public bool IsAuthOpen
        {
            get
            {
                return isAuthOpen;
            }
        }

        #endregion

        #region Private variables

        private bool isAuthOpen;

        private UdpSession  udpSession;
        private CanvasGroup canvasGroup;

        #endregion

        protected override void OnAwake()
        {
            passwordInputField.characterLimit      = ServerUtils.MAX_SERVER_PASSWORD_LENGTH;
            passwordInputField.characterValidation = TMP_InputField.CharacterValidation.Alphanumeric;
            passwordInputField.keyboardType        = TouchScreenKeyboardType.ASCIICapable;
            passwordInputField.contentType         = TMP_InputField.ContentType.Alphanumeric;
        }

        public void OpenConnectAuthWindow(UdpSession udpSession, CanvasGroup canvasGroup)
        {
            connectAuthWindow.SetActive(true);

            passwordInputField.text = "";

            this.udpSession               = udpSession;
            this.canvasGroup              = canvasGroup;
            this.canvasGroup.interactable = false;

            this.isAuthOpen = true;
        }

        public void CloseConnectAuthWindow()
        {
            connectAuthWindow.SetActive(false);

            isAuthOpen = false;

            if (canvasGroup != null)
            {
                canvasGroup.interactable = true;
            }
        }

        public void AttemptToConnectToSession()
        {
            BoltNetwork.Connect(udpSession, new UserToken(UserUtils.GetUserId(), passwordInputField.text));
        }

    }

}
