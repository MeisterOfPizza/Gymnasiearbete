using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class UIGameController : Controller<UIGameController>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private RectTransform enemyOverlayContainer;
        [SerializeField] private RectTransform interactableOverlayContainer;

        #endregion

        public RectTransform EnemyOverlayContainer
        {
            get
            {
                return enemyOverlayContainer;
            }
        }

        public RectTransform InteractableOverlayContainer
        {
            get
            {
                return interactableOverlayContainer;
            }
        }

    }

}
