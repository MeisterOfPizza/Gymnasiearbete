using UnityEngine;

namespace ArenaShooter.Extensions.UIComponents
{
    
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    sealed class UIScreenGuide : MonoBehaviour
    {

        #region Editor

        [SerializeField] [Range(0f, 1f)] private float pivotXPercentage = 0.5f;
        [SerializeField] [Range(0f, 1f)] private float pivotYPercentage = 0.5f;

        [Space]
        [SerializeField] private Vector2 padding = Vector2.zero;

        #endregion

        #region Private variables

        private RectTransform rectTransform;

        #endregion

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void LateUpdate()
        {
            UpdatePivot();
        }

        private void UpdatePivot()
        {
            float width  = Screen.width / 2f + rectTransform.sizeDelta.x / 2f;
            float height = Screen.height / 2f + rectTransform.sizeDelta.y / 2f;

            float pivotX = width - width * (1 - pivotXPercentage) * 2;
            float pivotY = height - height * (1 - pivotYPercentage) * 2;

            float paddingX = padding.x * (0.5f - pivotXPercentage) * 2;
            float paddingY = padding.y * (0.5f - pivotYPercentage) * 2;

            rectTransform.anchoredPosition = new Vector2(pivotX - paddingX, pivotY - paddingY);
        }

        private void Reset()
        {
            rectTransform = GetComponent<RectTransform>();
        }

    }

}