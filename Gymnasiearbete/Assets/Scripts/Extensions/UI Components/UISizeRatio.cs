using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Extensions.UIComponents
{

    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    [DefaultExecutionOrder(200)]
    sealed class UISizeRatio : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private RectTransform rectTransform;

        [Header("Values")]
        [SerializeField] private SizeRatioCopy sizeRatioCopy = SizeRatioCopy.SmallerMatchLarger;

        [Space]
        [SerializeField] private float          ratioScale     = 1f;
        [SerializeField] private SizeRatioScale sizeRatioScale = SizeRatioScale.ApplyToSmallest;

        #endregion

        #region Private variables

        private Vector2 unscaledSize;

        #endregion

        #region Enums

        private enum SizeRatioCopy : byte
        {
            WidthMatchHeight,
            HeightMatchWidth,
            SmallerMatchLarger,
            LargerMatchSmaller
        }

        private enum SizeRatioScale
        {
            ApplyToWidth,
            ApplyToHeight,
            ApplyToSmallest,
            ApplyToLargest
        }

        #endregion

        private void Start()
        {
            Setup();

            SetSize();
        }

        private void Setup()
        {
            if (rectTransform != null)
            {
                unscaledSize = rectTransform.rect.size;
            }
        }

        private void SetSize()
        {
            if (Application.isPlaying && rectTransform != null)
            {
                rectTransform.sizeDelta = CalculateSize();
            }
        }

        private Vector2 CalculateSize()
        {
            Vector2 scaledSize = unscaledSize;

            switch (sizeRatioCopy)
            {
                case SizeRatioCopy.WidthMatchHeight:
                    scaledSize = new Vector2(unscaledSize.y, unscaledSize.y);
                    break;
                case SizeRatioCopy.HeightMatchWidth:
                    scaledSize = new Vector2(unscaledSize.x, unscaledSize.x);
                    break;
                case SizeRatioCopy.SmallerMatchLarger:
                    {
                        float size = Mathf.Max(unscaledSize.x, unscaledSize.y);
                        scaledSize = new Vector2(size, size);
                        break;
                    }
                case SizeRatioCopy.LargerMatchSmaller:
                    {
                        float size = Mathf.Min(unscaledSize.x, unscaledSize.y);
                        scaledSize = new Vector2(size, size);
                        break;
                    }
                default:
                    break;
            }

            return CalculateSizeRatio(scaledSize);
        }

        private Vector2 CalculateSizeRatio(Vector2 size)
        {
            switch (sizeRatioScale)
            {
                case SizeRatioScale.ApplyToWidth:
                    return new Vector2(size.x * ratioScale, size.y);
                case SizeRatioScale.ApplyToHeight:
                    return new Vector2(size.x, size.y * ratioScale);
                case SizeRatioScale.ApplyToSmallest:
                    return unscaledSize.x < unscaledSize.y ? new Vector2(size.x * ratioScale, size.y) : new Vector2(size.x, size.y * ratioScale);
                case SizeRatioScale.ApplyToLargest:
                    return unscaledSize.x > unscaledSize.y ? new Vector2(size.x * ratioScale, size.y) : new Vector2(size.x, size.y * ratioScale);
                default:
                    return size;
            }
        }

    }

}
