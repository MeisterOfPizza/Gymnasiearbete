using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace ArenaShooter.Controllers
{

    class OptionsController : Controller<OptionsController>
    {

        #region Editor

        [SerializeField] private TMPro.TMP_Dropdown resolutionsDropDown;
        [SerializeField] private AudioMixer master;
        [SerializeField] private AudioMixer SFX;
        [SerializeField] private AudioMixer music;
        [SerializeField] private AudioMixer misc;

        #endregion

        #region PrivateVariables

        Resolution[] resolutions;

        private float masterVolume;
        private float miscVolume;
        private float sFXVolume;
        private float musicVolume;

        #endregion

        #region Methods

        private void Start()
        {
            QualitySettings.SetQualityLevel(3);
            resolutions = Screen.resolutions;
            resolutionsDropDown.ClearOptions();

            List<string> options = new List<string>();

            int currenResolutionsIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                options.Add(resolutions[i].width + " x " + resolutions[i].height);

                if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currenResolutionsIndex = i;
                }
            }

            resolutionsDropDown.AddOptions(options);
            resolutionsDropDown.value = currenResolutionsIndex;
            resolutionsDropDown.RefreshShownValue();
        }

        public void SetGraphicsQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        public void SetShadowQuality(int qualityIndex)
        {
            ShadowResolution shadows = (ShadowResolution)qualityIndex;
        }

        public void FullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        #endregion

        #region AudioMixerMethods

        public void SetMasterVolume(float volume)
        {
            masterVolume = volume;
        }

        public void SetMusicVolume(float volume)
        {
            musicVolume = volume;
        }

        public void SetSFXVolume(float volume)
        {
            sFXVolume = volume;
        }

        public void SetMiscVolume(float volume)
        {
            miscVolume = volume;
        }

        #endregion

        #region Getters

        public float MasterVolume
        {
            get
            {
                return masterVolume;
            }
        }

        public float MiscVolume
        {
            get
            {
                return miscVolume;
            }
        }

        public float SFXVolume
        {
            get
            {
                return sFXVolume;
            }
        }

        public float MusicVolume
        {
            get
            {
                return musicVolume;
            }
        }

        #endregion

    }

}

