using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.UI
{

    /// <summary>
    /// Animates a loader.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    class UILoader : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private RectTransform rectTransform;

        [Space]
        [SerializeField] private Animator animator;
        [SerializeField] private string   beginAnimationName = "Begin";
        [SerializeField] private string   stopAnimationName  = "Stop";
        [SerializeField] private string   speedParameterName = "Speed";
        [SerializeField] private float    spinSpeed          = 1.5f;

        [Header("Values")]
        [SerializeField] private UILoaderAction loaderAction = UILoaderAction.TransformSpin;
        [SerializeField] private bool           hideOnStop   = true;

        #endregion

        #region Private variables

        private bool isLoading;

        #endregion

        #region Private enum

        private enum UILoaderAction
        {
            Animate,
            AnimateSpeed,
            TransformSpin
        }

        #endregion

        private void Awake()
        {
            if (hideOnStop)
            {
                gameObject.SetActive(false);
            }

            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }
        }

        public void Begin()
        {
            isLoading = true;

            gameObject.SetActive(true);

            switch (loaderAction)
            {
                case UILoaderAction.Animate:
                    animator.Play(beginAnimationName);
                    break;
                case UILoaderAction.AnimateSpeed:
                    animator.Play(beginAnimationName);
                    animator.SetFloat(speedParameterName, spinSpeed);
                    break;
                case UILoaderAction.TransformSpin:
                default:
                    rectTransform.rotation = Quaternion.identity;
                    break;
            }
        }

        public void Stop()
        {
            switch (loaderAction)
            {
                case UILoaderAction.Animate:
                case UILoaderAction.AnimateSpeed:
                    animator.Play(stopAnimationName);
                    break;
                case UILoaderAction.TransformSpin:
                default:
                    break;
            }

            isLoading = false;

            /// Hide if <see cref="hideOnStop"/> is true.
            gameObject.SetActive(!hideOnStop);
        }

        private void Update()
        {
            if (isLoading && loaderAction == UILoaderAction.TransformSpin)
            {
                rectTransform.Rotate(Vector3.forward, spinSpeed);
            }
        }

        private void OnDisable()
        {
            Stop();
        }

    }

}
