using UnityEditor;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class UIGameMenuController : Controller<UIGameMenuController>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private GameObject exitMatchButton;
        [SerializeField] private GameObject disconnectFromMatchButton;

        #endregion

        private void Start()
        {
            if (BoltNetwork.IsRunning)
            {
                exitMatchButton.SetActive(BoltNetwork.IsSinglePlayer);
                disconnectFromMatchButton.SetActive(!BoltNetwork.IsSinglePlayer);
            }
        }

        public void ExitMatch()
        {
            if (BoltNetwork.IsClient)
            {
                BoltNetwork.Shutdown();
            }
            else
            {
                BoltNetwork.LoadScene(BoltScenes.Lobby);
                BoltNetwork.Shutdown();
            }
        }

        public void ExitGame()
        {
            if (Application.isPlaying && !Application.isEditor)
            {
                Application.Quit();
            }
            else if (Application.isEditor)
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif
            }
        }

    }

}
