using ArenaShooter.Player;
using TMPro;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.UI
{

    class UIPlayerGameStats : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private TMP_Text healthText;

        #endregion

        #region Private variables

        private PlayerController player;

        #endregion

        public void Initialize(PlayerController player)
        {
            this.player = player;
        }

        public void UpdateUI()
        {
            healthText.text = "Health: " + player.state.Health;
        }

    }

}
