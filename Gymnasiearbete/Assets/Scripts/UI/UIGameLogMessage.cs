using System.Collections;
using TMPro;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.UI
{

    class UIGameLogMessage : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private TMP_Text messageText;

        [Header("Values")]
        [SerializeField] private float visibilityTime = 5f;
        [SerializeField] private float fadeTime       = 0.5f;

        #endregion

        public void Initialize(string message)
        {
            this.messageText.text = message;

            StartCoroutine("Fade");
        }

        private IEnumerator Fade()
        {
            yield return new WaitForSecondsRealtime(visibilityTime);

            float fadeTimeLeft = fadeTime;

            while (fadeTimeLeft > 0f)
            {
                fadeTimeLeft -= Time.deltaTime;

                messageText.color = new Color(messageText.color.r, messageText.color.g, messageText.color.b, fadeTimeLeft / fadeTime);

                yield return new WaitForEndOfFrame();
            }

            Destroy(gameObject);
        }

    }

}
