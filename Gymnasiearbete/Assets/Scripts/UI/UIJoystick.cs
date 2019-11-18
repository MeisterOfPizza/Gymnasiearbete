using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ArenaShooter.UI.Joystick
{

    class UIJoystick : MonoBehaviour, IDragHandler, IEndDragHandler 
    {

        #region Editor

        [SerializeField] private RectTransform container;
        [SerializeField] private RectTransform joystick;

        [Space]
        [SerializeField] private JoystickDeltaUpdated joystickDeltaUpdated;

        #endregion

        #region Private variables

        private Vector2 joysticksLastPosition;

        private float containerRadius;
        private float knobRadius;
        
        #endregion

        #region Classes

        [Serializable]
        private class JoystickDeltaUpdated : UnityEvent<Vector2> { }

        #endregion

        #region Methods

        private void Awake()
        {
#if UNITY_STANDALONE
            gameObject.SetActive(false);
#endif

            containerRadius = container.sizeDelta.x / 2f;
            knobRadius      = joystick.sizeDelta.x / 2f;
        }

        private Vector2 BorderChecker(Vector2 position)
        {
            float distance = position.magnitude;
            
            if (distance > containerRadius - knobRadius)
            {
                return position * (containerRadius - knobRadius) / distance;
            }

            return position;
        }

        public void DragPointer(Vector2 touchPoint)
        {
#if UNITY_STANDALONE
            joystick.localPosition = BorderChecker(transform.InverseTransformPoint(Input.mousePosition));
#elif UNITY_IOS || UNITY_ANDROID
            joystick.localPosition = BorderChecker(transform.InverseTransformPoint(touchPoint));
#endif

            float speed = joystick.localPosition.magnitude / containerRadius;
            joystickDeltaUpdated.Invoke(joystick.localPosition.normalized * speed);
        }

        public void ResetPosition()
        {
            joystick.localPosition = Vector3.zero;
            joystickDeltaUpdated.Invoke(Vector2.zero);
        }

        #endregion

        #region Event handler systems

        public void OnDrag(PointerEventData eventData)
        {
            DragPointer(eventData.pressPosition);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ResetPosition();
        }

       

        #endregion

    }

}
