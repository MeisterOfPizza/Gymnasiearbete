using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class UIEnemyGameStatsController : Controller<UIEnemyGameStatsController>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private RectTransform container;

        #endregion

        #region Public properties

        public RectTransform Container
        {
            get
            {
                return container;
            }
        }

        #endregion

    }

}
