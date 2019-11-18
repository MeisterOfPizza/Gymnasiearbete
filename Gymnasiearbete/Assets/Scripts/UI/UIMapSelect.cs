using ArenaShooter.Templates.Maps;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#pragma warning disable 0649

namespace ArenaShooter.Assets.Scripts.UI
{

    [RequireComponent(typeof(Image))]
    sealed class UIMapSelect : Selectable
    {

        #region Editor

        [Header("References")]
        [SerializeField] private MapTemplate mapTemplate;

        [Header("Values")]
        [SerializeField] private OnMapSelected onClick;

        #endregion

        #region Public properties

        public MapTemplate MapTemplate
        {
            get
            {
                return mapTemplate;
            }
        }

        #endregion

        #region Private classes

        [Serializable]
        private class OnMapSelected : UnityEvent<UIMapSelect> { }

        #endregion

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (interactable && currentSelectionState != SelectionState.Disabled)
            {
                onClick.Invoke(this);
            }
        }

    }

}
