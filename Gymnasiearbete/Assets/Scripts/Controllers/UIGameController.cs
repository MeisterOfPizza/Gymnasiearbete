using ArenaShooter.Player;
using ArenaShooter.UI;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class UIGameController : Controller<UIGameController>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private RectTransform     enemyOverlayContainer;
        [SerializeField] private RectTransform     interactableOverlayContainer;
        [SerializeField] private UIPlayerGameStats uiPlayerGameStats;

        [Space]
        [SerializeField] private RectTransform uiForeignPlayerGameStatsContainer;
        [SerializeField] private GameObject    uiForeignPlayerGameStatsPrefab;

        [Space]
        [SerializeField] private RectTransform uiGameLogMessageContainer;
        [SerializeField] private GameObject    uiGameLogMessagePrefab;

        #endregion

        #region Private variables

        private List<PlayerController>                          foreignPlayerControllers = new List<PlayerController>();
        private Dictionary<PlayerController, UIPlayerGameStats> foreignPlayerGameStats   = new Dictionary<PlayerController, UIPlayerGameStats>();

        #endregion

        #region Public properties

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

        public UIPlayerGameStats UIPlayerGameStats
        {
            get
            {
                return uiPlayerGameStats;
            }
        }

        #endregion

        public UIPlayerGameStats RegisterForeignPlayerControllerForUI(PlayerController playerController)
        {
            if (!foreignPlayerControllers.Contains(playerController))
            {
                var gameStats = Instantiate(uiForeignPlayerGameStatsPrefab, uiForeignPlayerGameStatsContainer).GetComponent<UIPlayerGameStats>();
                gameStats.Initialize(playerController);

                foreignPlayerGameStats.Add(playerController, gameStats);

                foreignPlayerControllers.Add(playerController);

                return gameStats;
            }

            return null;
        }

        public void UnregisterForeignPlayerControllerForUI(PlayerController playerController)
        {
            if (foreignPlayerControllers.Contains(playerController))
            {
                foreignPlayerControllers.Remove(playerController);

                if (foreignPlayerGameStats[playerController] != null)
                {
                    // Destroy the UI element:
                    Destroy(foreignPlayerGameStats[playerController].gameObject);
                }
            }
        }

        public void GameLogMessageEvent(GameLogMessageEvent @event)
        {
            var ui = Instantiate(uiGameLogMessagePrefab, uiGameLogMessageContainer).GetComponent<UIGameLogMessage>();
            ui.Initialize(@event.Message);
        }

    }

}
