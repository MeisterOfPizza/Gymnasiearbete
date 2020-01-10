using ArenaShooter.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ArenaShooter.Combat.Weapon;

#pragma warning disable 0649

namespace ArenaShooter.UI
{

    class UIPlayerGameStats : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private TMP_Text      playerNameText;
        [SerializeField] private RectTransform healthBarRect;
        [SerializeField] private Image         healthBar;
        [SerializeField] private TMP_Text      ammoText;

        [Header("Values")]
        [SerializeField] private Gradient healthGradient;

        [Space]
        [SerializeField] private float healthBarUpdateSpeed = 10f;

        #endregion

        #region Private variables

        private PlayerController player;

        #endregion

        public void Initialize(PlayerController player)
        {
            this.player = player;

            UpdateUsernameUI();
        }

        private void Update()
        {
            if (player != null && player.entity.IsAttached && BoltNetwork.IsRunning)
            {
                healthBarRect.localPosition = Vector3.Lerp(healthBarRect.localPosition, Vector3.right * healthBarRect.rect.width * (1f - player.state.Health / (float)PlayerController.PLAYER_MAX_HEALTH), healthBarUpdateSpeed * Time.deltaTime);

                healthBar.color = healthGradient.Evaluate(player.state.Health / (float)PlayerController.PLAYER_MAX_HEALTH);
            }
        }

        public void UpdateAmmoUI(AmmoStatus ammoStatus)
        {
            ammoText.text = ammoStatus.FormatAmmoStatus();
        }

        public void UpdateUsernameUI()
        {
            if (playerNameText != null)
            {
                playerNameText.text = player.state.Name;
            }
        }

    }

}
