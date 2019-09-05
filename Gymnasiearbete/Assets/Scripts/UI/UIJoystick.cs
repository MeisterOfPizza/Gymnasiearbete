using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

namespace ArenaShooter.UI.Joystick
{

    class UIJoystick : MonoBehaviour
    {

        #region Editor

        [SerializeField] private RectTransform        container;
        [SerializeField] private Image                joystick;
        [SerializeField] private JoystickDeltaUpdated joystickDeltaUpdated;

        #endregion

        #region Private variables

        private Vector2 joysticksLastPosition = new Vector2(0,0);
        
        #endregion

        #region Classes

        [Serializable]
        private class JoystickDeltaUpdated : UnityEvent<Vector2> { }

        #endregion

        #region Methods

        private Vector2 BorderChecker(Vector2 position)
        {
            float radius          = joystick.rectTransform.sizeDelta.x/2;
            float containerRadius = container.sizeDelta.x / 2;

            float distance        = Vector2.Distance(position, new Vector2(0, 0));

            Vector2 newPosition = position;
            
            if(distance > containerRadius)
            {
                newPosition *= containerRadius / distance;
                return newPosition;
            }

            

            return newPosition;
        }

        public void OnDrag()
        {
#if UNITY_STANDALONE
            joystick.transform.localPosition = BorderChecker(transform.InverseTransformPoint(Input.mousePosition));
#elif UNITY_IOS || UNITY_ANDROID
            if(Input.touchCount > 0)
            {
                joystick.transform.localPosition = BorderChecker(transform.InverseTransformPoint(Input.GetTouch(0).position));
            }
#endif
            joystickDeltaUpdated.Invoke((Vector2)joystick.transform.localPosition - joysticksLastPosition);
            joysticksLastPosition = joystick.transform.localPosition;
        }

        private void Start()
        {
            container.position = new Vector2(container.sizeDelta.x,container.sizeDelta.y);
        }

        public void OnPointerUp()
        {
            joystick.transform.localPosition = Vector3.zero;
        }

        #endregion

    }

}
