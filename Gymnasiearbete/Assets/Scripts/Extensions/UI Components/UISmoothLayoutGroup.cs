using UnityEngine;
using UnityEngine.UI;

namespace ArenaShooter.UI
{

    [ExecuteInEditMode]
    public class UISmoothLayoutGroup : HorizontalOrVerticalLayoutGroup
    {

        #region Editor

        [SerializeField] private Direction direction = Direction.Up;

        [Space]
        [SerializeField] private Vector2 size        = new Vector2(100, 100);
        [SerializeField] private bool    dynamicSize = false;

        [Space]
        [SerializeField] private float travelSpeed = 3f;

        #endregion

        #region Private variables

        private int  childCount;
        private bool isDirty;

        #endregion

        #region Enums

        private enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        #endregion

        protected override void Awake()
        {
            base.Awake();

            childCount = transform.childCount;
        }

#if UNITY_EDITOR
        protected override void Update()
        {
            base.Update();

            if (isDirty || (dynamicSize && CheckDynamicSizeChange()))
            {
                UpdateChildren();
            }
        }
#else
        private void Update()
        {
            if (isDirty || (dynamicSize && CheckDynamicSizeChange()))
            {
                UpdateChildren();
            }
        }
#endif

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        private void OnValidate()
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        {
            isDirty = true;
        }

        protected override void OnTransformChildrenChanged()
        {
            base.OnTransformChildrenChanged();

            if (transform.childCount != childCount)
            {
                // The child count was changed:
                childCount = transform.childCount;
                isDirty    = childCount > 0;
            }
        }

        private void UpdateChildren()
        {
            bool placingDone = true;

            for (int i = 0; i < childCount; i++)
            {
                Transform child  = transform.GetChild(i);
                Vector3   target = GetChildTargetPosition(i);
                child.hasChanged = false;

                child.localPosition = Vector3.Lerp(child.localPosition, target, travelSpeed * Time.deltaTime);

                if (placingDone)
                {
                    placingDone = Mathf.Abs(child.localPosition.magnitude - target.magnitude) < 0.01f;
                }
            }

            isDirty = !placingDone;
        }

        private Vector3 GetChildTargetPosition(int index)
        {
            if (dynamicSize)
            {
                Vector2 sizeOffset = GetTotalChildSize(index);

                switch (direction)
                {
                    case Direction.Up:
                        return new Vector3(padding.horizontal, spacing * index + sizeOffset.y + padding.bottom);
                    case Direction.Down:
                        return new Vector3(padding.horizontal, -spacing * index - sizeOffset.y - padding.top);
                    case Direction.Left:
                        return new Vector3(-spacing * index - sizeOffset.x - padding.right, padding.vertical);
                    case Direction.Right:
                        return new Vector3(spacing * index + sizeOffset.x + padding.left, padding.vertical);
                    default:
                        return Vector3.zero;
                }
            }
            else
            {
                switch (direction)
                {
                    case Direction.Up:
                        return new Vector3(padding.horizontal, spacing * index + size.y * index + padding.bottom);
                    case Direction.Down:
                        return new Vector3(padding.horizontal, -spacing * index - size.y * index - padding.top);
                    case Direction.Left:
                        return new Vector3(-spacing * index - size.x * index - padding.right, padding.vertical);
                    case Direction.Right:
                        return new Vector3(spacing * index + size.x * index + padding.left, padding.vertical);
                    default:
                        return Vector3.zero;
                }
            }
        }

        private Vector2 GetTotalChildSize(int fromTo)
        {
            if (fromTo < 0)
            {
                return Vector2.zero;
            }
            else
            {
                Vector2 size = Vector2.zero;

                for (int i = 0; i < Mathf.Min(fromTo, childCount - 1); i++)
                {
                    size += transform.GetChild(i).GetComponent<RectTransform>().sizeDelta;
                }

                return size;
            }
        }

        private bool CheckDynamicSizeChange()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).hasChanged)
                {
                    return true;
                }
            }

            return false;
        }

        public override void CalculateLayoutInputVertical()
        {
            Update();
        }

        public override void SetLayoutHorizontal()
        {
            isDirty = true;

            Update();
        }

        public override void SetLayoutVertical()
        {
            isDirty = true;

            Update();
        }

    }

}
