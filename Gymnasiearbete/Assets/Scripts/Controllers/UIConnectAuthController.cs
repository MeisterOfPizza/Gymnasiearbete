using ArenaShooter.Extensions;
using ArenaShooter.Networking.Protocols;
using ArenaShooter.UI;
using TMPro;
using UdpKit;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class UIConnectAuthController : Controller<UIConnectAuthController>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private GameObject connectAuthWindow;

        [Space]
        [SerializeField] private CanvasGroup canvasGroup;

        [Space]
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private GameObject     authIncorrectText;
        [SerializeField] private UILoader       connectionLoader;

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
        private CanvasGroup backgroundCanvasGroup;

        #endregion

        protected override void OnAwake()
        {
            passwordInputField.characterLimit      = ServerUtils.MAX_SERVER_PASSWORD_LENGTH;
            passwordInputField.characterValidation = TMP_InputField.CharacterValidation.Alphanumeric;
            passwordInputField.keyboardType        = TouchScreenKeyboardType.ASCIICapable;
            passwordInputField.contentType         = TMP_InputField.ContentType.Alphanumeric;
        }

        public void OpenConnectAuthWindow(UdpSession udpSession, CanvasGroup backgroundCanvasGroup)
        {
            connectAuthWindow.SetActive(true);

            passwordInputField.text = "";
            authIncorrectText.gameObject.SetActive(false);
            canvasGroup.interactable = true;

            connectionLoader.Stop();

            this.udpSession                         = udpSession;
            this.backgroundCanvasGroup              = backgroundCanvasGroup;
            this.backgroundCanvasGroup.interactable = false;

            this.isAuthOpen = true;
        }

        public void CloseConnectAuthWindow()
        {
            connectAuthWindow.SetActive(false);

            isAuthOpen = false;

            if (backgroundCanvasGroup != null)
            {
                backgroundCanvasGroup.interactable = true;
            }
        }

        public void AttemptToConnectToSession()
        {
            BoltNetwork.Connect(udpSession, new UserToken(UserUtils.GetUserId(), passwordInputField.text));

            canvasGroup.interactable = false;
            authIncorrectText.gameObject.SetActive(false);
            connectionLoader.Begin();
        }

        public void PasswordAuthFailed()
        {
            canvasGroup.interactable = true;
            authIncorrectText.gameObject.SetActive(true);
            connectionLoader.Stop();
        }

    }

}
