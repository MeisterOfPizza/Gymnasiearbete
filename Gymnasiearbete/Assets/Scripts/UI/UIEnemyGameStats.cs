using ArenaShooter.Assets.Scripts.Entities;
using TMPro;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.UI
{

    class UIEnemyGameStats : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private TMP_Text healthText;

        #endregion

        #region Private variables

        private Enemy enemy;

        #endregion

        public void Initialize(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void UpdateUI()
        {
            healthText.text = "Health: " + enemy.state.Health;
        }

    }

}
