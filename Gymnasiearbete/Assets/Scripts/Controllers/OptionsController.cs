using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using ArenaShooter.Extensions.UIComponents;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class OptionsController : Controller<OptionsController>
    {

        #region Editor

        [SerializeField] private TMP_Dropdown resolutionsDropDown;
        [SerializeField] private TMP_Dropdown refreshrateDropDown;
        [SerializeField] private AudioMixer   master;
        [SerializeField] private AudioMixer   sfx;
        [SerializeField] private AudioMixer   music;
        [SerializeField] private AudioMixer   misc;
        [SerializeField] private GameObject   menu;
        [SerializeField] private GameObject   options;
        [SerializeField] private TMP_Dropdown   graphicsQuality;
        [SerializeField] private GameObject   shadowQuality;
        [SerializeField] private Toggle       fullscreen;
        [SerializeField] private Button       back;
        [SerializeField] private UISlider masterVolume;
        [SerializeField] private UISlider musicVolume;
        [SerializeField] private UISlider miscVolume;
        [SerializeField] private UISlider sfxVolume;


        #endregion

        #region Private variables

        private Resolution[] resolutions;
        private List<string> refreshRates = new List<string>();
        private Resolution currentRes;

        #endregion

        #region Methods

        private void Start()
        {
#if UNITY_STANDALONE
            QualitySettings.SetQualityLevel(3);
            resolutions = Screen.resolutions;
            refreshrateDropDown.ClearOptions();
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (!refreshRates.Contains(resolutions[i].refreshRate.ToString()))
                {
                    refreshRates.Add(resolutions[i].refreshRate.ToString());
                }

            }

            refreshrateDropDown.AddOptions(refreshRates);
            refreshrateDropDown.RefreshShownValue();

            resolutions = Screen.resolutions.Where(r => r.refreshRate == 60).ToArray();
            resolutionsDropDown.ClearOptions();

            List<string> options = new List<string>();

            int currenResolutionsIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                options.Add(string.Format("{0} x {1}", resolutions[i].width, resolutions[i].height));

                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currenResolutionsIndex = i;
                }
            }

            resolutionsDropDown.AddOptions(options);
            resolutionsDropDown.value = currenResolutionsIndex;
            resolutionsDropDown.RefreshShownValue();
#elif UNITY_IOS || UNITY_ANDROID
            fullscreen.gameObject.SetActive(false);
            shadowQuality.SetActive(false);
            graphicsQuality.SetActive(false);
            refreshrateDropDown.gameObject.SetActive(false);
            resolutionsDropDown.gameObject.SetActive(false);
#endif

        }

        public void SetGraphicsQuality(int qualityIndex)
        {
            int quality = graphicsQuality.value;
            QualitySettings.SetQualityLevel(quality);
            
        }

        public void SetShadowQuality(int qualityIndex)
        {
            ShadowResolution shadows = (ShadowResolution)qualityIndex;
        }

        public void FullScreen(bool isFullScreen)
        {
            Screen.fullScreen = fullscreen.isOn;
            
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = resolutions[resolutionsDropDown.value];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            currentRes = resolution;
        }

        public void SetRefreshRate(int refresRate)
        {
            Screen.SetResolution(currentRes.width, currentRes.height, Screen.fullScreen, int.Parse(refreshRates[refreshrateDropDown.value]));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && menu != null && menu.activeInHierarchy == false && options.activeInHierarchy == false)
            {
                menu.SetActive(true);
            }
            else if(Input.GetKeyDown(KeyCode.Escape) && menu != null && menu.activeInHierarchy == true)
            {
                menu.SetActive(false);
            }
        }

        #endregion

        #region Audio mixer methods

        public void SetMasterVolume(float input)
        {
            float volume = masterVolume.FloatValue;
            master.SetFloat(("Volume"), -80 + 100 * volume);
        }

        public void SetMusicVolume(float input)
        {
            float volume = musicVolume.FloatValue;
            music.SetFloat("Volume", -80 + 100 * volume);
        }

        public void SetSFXVolume(float input)
        {
            float volume = sfxVolume.FloatValue;
            sfx.SetFloat("Volume", -80 + 100 * volume);
        }

        public void SetMiscVolume(float input)
        {
            float volume = miscVolume.FloatValue;
            misc.SetFloat("Volume", -80 + 100 * volume);
        }

        #endregion

    }

}
