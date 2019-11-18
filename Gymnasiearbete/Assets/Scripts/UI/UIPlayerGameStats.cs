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
        [SerializeField] private TMP_Text ammoText;

        #endregion

        #region Private variables

        private PlayerController player;

        #endregion

        public void Initialize(PlayerController player)
        {
            this.player = player;
        }

        public void UpdateHealthUI()
        {
            healthText.text = "Health: " + player.state.Health;
        }

        public void UpdateAmmoUI(string ammoFormat)
        {
            ammoText.text = "Ammo: " + ammoFormat;
        }

    }

}
