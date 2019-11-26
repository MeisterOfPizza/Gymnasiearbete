using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Extensions.UIComponents
{

    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    [DefaultExecutionOrder(100)]
    sealed class UISizeCopy : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private RectTransform rectToCopy;

        [Header("Values")]
        [SerializeField] private bool rectToCopyIsParent;

        [Space]
        [SerializeField] private bool copyWidth  = true;
        [SerializeField] private bool copyHeight = true;

        #endregion

        private void Start()
        {
            if (rectToCopyIsParent)
            {
                rectToCopy = transform.parent.GetComponent<RectTransform>();
            }
        }

        private void OnEnable()
        {
            SetSize();
        }

        private void OnTransformParentChanged()
        {
            SetSize();
        }

        private void SetSize()
        {
            if (Application.isPlaying && rectTransform != null && rectToCopy != null)
            {
                Vector2 size = new Vector2(copyWidth ? rectToCopy.rect.width : rectTransform.rect.width, copyHeight ? rectToCopy.rect.height : rectTransform.rect.height);
                rectTransform.sizeDelta = size;
            }
        }

    }

}
