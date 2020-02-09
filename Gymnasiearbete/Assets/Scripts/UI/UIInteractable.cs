using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649

namespace ArenaShooter.UI
{
    class UIInteractable : MonoBehaviour
    {
        #region Editor

        [SerializeField] private Image countDownImage;

        #endregion

        #region Methods

        public void CountDown(float value)
        {
            countDownImage.fillAmount = value;
        }

        #endregion
    }
}

