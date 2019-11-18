using ArenaShooter.Player;
using System.Collections.Generic;
using System.Linq;

namespace ArenaShooter.Controllers
{

    class ServerLobbyController : Controller<ServerLobbyController>
    {

        #region Public properties

        public LobbyPlayer[] LobbyPlayers
        {
            get
            {
                return lobbyPlayers.ToArray();
            }
        }

        #endregion

        #region Private variables

        private HashSet<LobbyPlayer> lobbyPlayers = new HashSet<LobbyPlayer>();

        #endregion

        public void AddLobbyPlayer(LobbyPlayer lobbyPlayer)
        {
            lobbyPlayers.Add(lobbyPlayer);
        }

        public void RemoveLobbyPlayer(LobbyPlayer lobbyPlayer)
        {
            lobbyPlayers.Remove(lobbyPlayer);
        }

        public void ClearLobbyPlayers()
        {
            lobbyPlayers.Clear();
        }

    }

}
