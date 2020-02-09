using ArenaShooter.Combat;
using ArenaShooter.Controllers;
using ArenaShooter.Player;
using ArenaShooter.Templates.Items;
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
            // Uncomment to use weapon template string which displays the template names:
            //string weaponText = string.Format("{0} | {1} | {2}", WeaponController.Singleton.GetStockTemplate((ushort)lobbyPlayer.state.Weapon.WeaponStockId).Name, WeaponController.Singleton.GetBodyTemplate((ushort)lobbyPlayer.state.Weapon.WeaponBodyId).Name, WeaponController.Singleton.GetBarrelTemplate((ushort)lobbyPlayer.state.Weapon.WeaponBarrelId).Name);

            string weaponText = "";

            switch (WeaponController.Singleton.GetStockTemplate((ushort)lobbyPlayer.state.Weapon.WeaponStockId).OutputType)
            {
                default:
                case Templates.Weapons.WeaponOutputType.Raycasting:
                    weaponText = "Playing with Kinetic weaponry";
                    break;
                case Templates.Weapons.WeaponOutputType.Projectile:
                    weaponText = "Playing with Projectile weaponry";
                    break;
                case Templates.Weapons.WeaponOutputType.Electric:
                    weaponText = "Playing with Electric weaponry";
                    break;
                case Templates.Weapons.WeaponOutputType.Support:
                    weaponText = "Playing as Support";
                    break;
            }

            this.weaponText.text  = weaponText;
            this.isReadyText.text = lobbyPlayer.state.Ready ? "Ready" : "Not ready";
        }

    }

}
