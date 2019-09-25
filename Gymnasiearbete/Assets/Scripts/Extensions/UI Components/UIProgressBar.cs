using UnityEngine;
using UnityEngine.UI;

namespace ArenaShooter.Extensions.UIComponents
{

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    sealed class UIProgressBar : MonoBehaviour
    {

        #region Public properties

        public float Progress
        {
            get
            {
                return progress;
            }

            set
            {
                progress = Mathf.Clamp01(value);

                if (image != null)
                {
                    image.fillAmount = progress;
                }
                else
                {
                    Awake();
                }
            }
        }

        #endregion

        #region Private variables

        private Image image;

        private float progress = 0f;

        #endregion

        #region MonoBehaivour methods

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        private void Reset()
        {
            image = GetComponent<Image>();
        }

        #endregion

    }

}
