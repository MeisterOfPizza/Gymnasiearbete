using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class UIErrorMessageBoxController : Controller<UIErrorMessageBoxController>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private GameObject errorBox;
        [SerializeField] private TMP_Text   errorTitle;
        [SerializeField] private TMP_Text   errorText;
        [SerializeField] private Button     okButton;

        #endregion

        public void DisplayError(string title, string message, Action okCallback = null)
        {
            errorBox.SetActive(true);
            errorTitle.text = title;
            errorText.text  = message;

            okButton.onClick.RemoveAllListeners();
            okButton.onClick.AddListener(() => okCallback?.Invoke());
        }

        public void DisplayError(string message, Action okCallback = null)
        {
            errorBox.SetActive(true);
            errorTitle.text = "Oops! Something went wrong :(";
            errorText.text  = message;

            okButton.onClick.RemoveAllListeners();
            okButton.onClick.AddListener(() => okCallback?.Invoke());
        }

        public void OkButtonClicked()
        {
            okButton.onClick.RemoveAllListeners();
            errorBox.SetActive(false);
        }

    }

}
