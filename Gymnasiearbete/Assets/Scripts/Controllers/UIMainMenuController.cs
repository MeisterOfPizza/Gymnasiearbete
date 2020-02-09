using ArenaShooter.Extensions;
using ArenaShooter.Player;
using ArenaShooter.UI;
using System;
using System.Globalization;
using TMPro;
using UnityEditor;
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
        [SerializeField] private TMP_InputField profileUsernameInputField;
        [SerializeField] private TMP_Text       profileTotalKillsText;
        [SerializeField] private TMP_Text       profileTotalDeathsText;
        [SerializeField] private TMP_Text       profileTotalShotsText;
        [SerializeField] private TMP_Text       profileTimePlayedText;

        [Space]
        [SerializeField] private GameObject loadingOnlineTextContainer;
        [SerializeField] private UILoader   loadingOnlineLoader;

        [Header("Values")]
        [SerializeField] private Color profileStatsColor = Color.white;

        #endregion

        private void Start()
        {
            profileUsernameInputField.onSubmit.AddListener((string input) => profileUsernameInputField.SetTextWithoutNotify(UserUtils.SetUsername(input)));
            profileUsernameInputField.characterLimit      = UserUtils.MAX_USERNAME_LENGTH;
            profileUsernameInputField.characterValidation = TMP_InputField.CharacterValidation.Name;
            profileUsernameInputField.keyboardType        = TouchScreenKeyboardType.ASCIICapable;
            profileUsernameInputField.contentType         = TMP_InputField.ContentType.Name;

            UpdateProfileUI();
        }

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

        private void UpdateProfileUI()
        {
            string htmlColor = ColorUtility.ToHtmlStringRGBA(profileStatsColor);

            profileUsernameInputField.text = UserUtils.GetUsername();
            profileTotalKillsText.text     = $"<color=#{htmlColor}>{Profile.TotalKills}</color> confirmed kills";
            profileTotalDeathsText.text    = $"<color=#{htmlColor}>{Profile.TotalDeaths}</color> deaths";
            profileTotalShotsText.text     = $"<color=#{htmlColor}>{Profile.TotalShots}</color> shots fired";
            profileTimePlayedText.text     = $"<color=#{htmlColor}>{Utils.GetSecondsFormatted(Profile.TimePlayed)}</color> spent in-game";
        }

    }

}
