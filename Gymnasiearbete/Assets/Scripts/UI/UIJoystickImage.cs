using System;
using UnityEngine;
using UnityEngine.EventSystems;

using ArenaShooter.Controllers;

namespace ArenaShooter.UI.Joystick
{

    class UIJoystickImage : MonoBehaviour , IDragHandler , IEndDragHandler, IBeginDragHandler , IPointerDownHandler , IPointerUpHandler
    {

        #region Event Methods
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            MobileLookController.Singleton.CanLook = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            MobileLookController.Singleton.SetMobileLookAtPoint(eventData.position);

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            MobileLookController.Singleton.CanLook = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            MobileLookController.Singleton.CanLook = true;
            MobileLookController.Singleton.SetMobileLookAtPoint(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            MobileLookController.Singleton.CanLook = false;
        }

        #endregion



    }

}

