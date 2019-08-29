using UnityEngine;
using UnityEngine.Events;

#pragma warning disable 0649

namespace ArenaShooter.Extensions.Components
{

    [DisallowMultipleComponent]
    class EventTrigger3D : MonoBehaviour
    {

        #region Editor

        [SerializeField] private UnityEvent onMouseDown;
        [SerializeField] private UnityEvent onMouseUp;
        [SerializeField] private UnityEvent onMouseUpAsButton;
        [SerializeField] private UnityEvent onMouseEnter;
        [SerializeField] private UnityEvent onMouseExit;
        [SerializeField] private UnityEvent onMouseOver;
        [SerializeField] private UnityEvent onMouseDrag;
        [SerializeField] private UnityEvent onMouseHold;

        #endregion

        #region Private variables

        private bool isHolding;

        #endregion

        #region MonoBehaviour methods

        private void Update()
        {
            OnMouseHold();
        }

        #endregion

        #region Default events

        private void OnMouseDown()
        {
            onMouseDown.Invoke();

            isHolding = true;
        }

        private void OnMouseUp()
        {
            onMouseUp.Invoke();

            isHolding = false;
        }

        private void OnMouseUpAsButton()
        {
            onMouseUpAsButton.Invoke();

            isHolding = false;
        }

        private void OnMouseEnter()
        {
            onMouseEnter.Invoke();
        }

        private void OnMouseExit()
        {
            onMouseExit.Invoke();
        }

        private void OnMouseOver()
        {
            onMouseOver.Invoke();
        }

        private void OnMouseDrag()
        {
            onMouseDrag.Invoke();
        }

        #endregion

        #region Custom events

        private void OnMouseHold()
        {
            if (isHolding)
            {
                onMouseHold.Invoke();
            }
        }

        #endregion

    }

}
