using UnityEngine.Events;
using UnityEngine;
using System;


namespace ArenaShooter.UI.Joystick
{

    class UIJoystick : MonoBehaviour
    {
        [SerializeField] private JoystickDeltaUpdated joystickDeltaUpdated;
        #region Classes
        [Serializable]
        private class JoystickDeltaUpdated : UnityEvent<Vector2> { }
        #endregion










        void OnDeath()
        {

        }
    }
}

