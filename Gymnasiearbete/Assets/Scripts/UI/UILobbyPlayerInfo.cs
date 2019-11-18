using ArenaShooter.Controllers;
using ArenaShooter.Player;
using TMPro;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.UI
{

    class UILobbyPlayerInfo : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private TMP_Text usernameText;
        [SerializeField] private TMP_Text weaponText;
        [SerializeField] private TMP_Text isReadyText;

        #endregion

        #region Private variable

        private LobbyPlayer lobbyPlayer;

        #endregion

        public void Initialize(LobbyPlayer lobbyPlayer)
        {
            this.lobbyPlayer = lobbyPlayer;

            this.usernameText.text = lobbyPlayer.state.Name;
        }

        public void UpdateUI()
        {
            string weaponText = string.Format("{0} | {1} | {2}", WeaponController.Singleton.GetStockTemplate((ushort)lobbyPlayer.state.Weapon.WeaponStockId).Name, WeaponController.Singleton.GetBodyTemplate((ushort)lobbyPlayer.state.Weapon.WeaponBodyId).Name, WeaponController.Singleton.GetBarrelTemplate((ushort)lobbyPlayer.state.Weapon.WeaponBarrelId).Name);

            this.weaponText.text  = weaponText;
            this.isReadyText.text = lobbyPlayer.state.Ready ? "Ready" : "Not ready";
        }

    }

}
