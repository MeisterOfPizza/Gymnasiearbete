using ArenaShooter.Controllers;
using ArenaShooter.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#pragma warning disable 0649

namespace ArenaShooter.UI
{

    class UIServerBrowserInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        #region Editor

        [Header("References")]
        [SerializeField] private Image    background;
        [SerializeField] private Image    mapThumbnailImage;
        [SerializeField] private TMP_Text serverNameText;
        [SerializeField] private TMP_Text mapNameText;
        [SerializeField] private TMP_Text hostUsernameText;
        [SerializeField] private TMP_Text playerCountText;
        [SerializeField] private TMP_Text pingText;
        [SerializeField] private Button   joinButton;

        #endregion

        #region Private variables

        private ServerInfo serverInfo;

        #endregion

        public void Initialize(ServerInfo serverInfo)
        {
            this.serverInfo = serverInfo;

            this.serverNameText.text   = serverInfo.Info.ServerName;
            this.hostUsernameText.text = serverInfo.Info.HostUsername;
            this.playerCountText.text  = serverInfo.PlayerCount;
            //this.pingText.text = serverInfo.UdpSession;

#if UNITY_IOS || UNITY_ANDROID
            joinButton.gameObject.SetActive(true);
#endif
        }

        #region Events

        public void AttemptToJoin()
        {
            UIServerBrowserController.Singleton.JoinSession(serverInfo);
        }

        #endregion

        #region IPointerEnterHandler

        public void OnPointerEnter(PointerEventData eventData)
        {
            background.color = Color.white;

            serverNameText.color   = Color.black;
            mapNameText.color      = Color.black;
            hostUsernameText.color = Color.black;
            playerCountText.color  = Color.black;
            pingText.color         = Color.black;

#if UNITY_STANDALONE
            joinButton.gameObject.SetActive(true);
#endif
        }

        #endregion

        #region IPointerExitHandler

        public void OnPointerExit(PointerEventData eventData)
        {
            background.color = Color.clear;

            serverNameText.color   = Color.white;
            mapNameText.color      = Color.white;
            hostUsernameText.color = Color.white;
            playerCountText.color  = Color.white;
            pingText.color         = Color.white;

#if UNITY_STANDALONE
            joinButton.gameObject.SetActive(false);
#endif
        }

        #endregion

    }

}
