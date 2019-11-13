using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

