using ArenaShooter.UI;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class UIPlayerGameStatsController : Controller<UIPlayerGameStatsController>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private UIPlayerGameStats uiPlayerGameStats;

        #endregion

        #region Public properties

        public UIPlayerGameStats UIPlayerGameStats
        {
            get
            {
                return uiPlayerGameStats;
            }
        }

        #endregion

    }

}
