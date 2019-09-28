using ArenaShooter.Controllers;
using ArenaShooter.Networking.Protocols;
using Bolt;
using Bolt.Matchmaking;
using System;
using UdpKit;

namespace ArenaShooter.Networking
{

    [BoltGlobalBehaviour]
    class NetworkLobby : GlobalEventListener
    {

        #region Bolt life cycle

        public override void BoltStartBegin()
        {
            BoltNetwork.RegisterTokenClass<UserToken>();
            BoltNetwork.RegisterTokenClass<AuthResultToken>();
            BoltNetwork.RegisterTokenClass<ServerInfoToken>();

            UIMainMenuController.Singleton.BeginOnlineLoader();
        }

        public override void BoltStartDone()
        {
            UIMainMenuController.Singleton.CloseMainMenu();
            UIMainMenuController.Singleton.StopOnlineLoader();

            if (BoltNetwork.IsClient)
            {
                UIServerBrowserController.Singleton.OpenServerBrowser();
            }
            else if (BoltNetwork.IsServer)
            {
                string sessionId = Guid.NewGuid().ToString();

                BoltMatchmaking.CreateSession(sessionId, UIServerSetupController.Singleton.GetServerInfoToken());
            }
        }

        public override void BoltShutdownBegin(AddCallback registerDoneCallback, UdpConnectionDisconnectReason disconnectReason)
        {
            UIServerSetupController.Singleton.CloseServerSetup();
            UIServerLobbyController.Singleton.CloseLobby();
            UIServerBrowserController.Singleton.CloseServerBrowser();
            UIConnectAuthController.Singleton.CloseConnectAuthWindow();
            ServerLobbyController.Singleton.ClearLobbyPlayers();
            UIMainMenuController.Singleton.OpenMainMenu();
            UIMainMenuController.Singleton.StopOnlineLoader();

            switch (disconnectReason)
            {
                case UdpConnectionDisconnectReason.Unknown:
                    UIErrorMessageBoxController.Singleton.DisplayError("Something went wrong", "We could not identify the problem, please try again.");
                    break;
                case UdpConnectionDisconnectReason.Timeout:
                    UIErrorMessageBoxController.Singleton.DisplayError("Timed out", "It took too long to connect to the cloud, try increasing your internet speed.");
                    break;
                case UdpConnectionDisconnectReason.Error:
                    UIErrorMessageBoxController.Singleton.DisplayError("An error occured!", "Maybe the cloud is down, or your internet connection is. Please try again after you've checked both.");
                    break;
                case UdpConnectionDisconnectReason.MaxCCUReached:
                    UIErrorMessageBoxController.Singleton.DisplayError("Player count reached!", "The cloud's player capacity has been reached, please wait for some players to disconnect before trying again.");
                    break;
                case UdpConnectionDisconnectReason.Disconnected:
                case UdpConnectionDisconnectReason.Authentication:
                default:
                    break;
            }
        }

        #endregion

        #region Session life cycle

        public override void SessionCreated(UdpSession session)
        {
            UIServerSetupController.Singleton.CloseServerSetup();
            UIServerLobbyController.Singleton.OpenLobby();
            
            BoltNetwork.Instantiate(BoltPrefabs.Lobby_Player);
        }

        public override void SessionConnected(UdpSession session, IProtocolToken token)
        {
            UIServerBrowserController.Singleton.CloseServerBrowser();
            UIServerLobbyController.Singleton.OpenLobby();

            if (UIConnectAuthController.Singleton.IsAuthOpen)
            {
                UIConnectAuthController.Singleton.CloseConnectAuthWindow();
            }

            BoltNetwork.Instantiate(BoltPrefabs.Lobby_Player);
        }

        public override void SessionConnectFailed(UdpSession session, IProtocolToken token)
        {
            UIErrorMessageBoxController.Singleton.DisplayError("Server not found", "The server may have been shutdown.");

            if (UIConnectAuthController.Singleton.IsAuthOpen)
            {
                UIConnectAuthController.Singleton.CloseConnectAuthWindow();
            }
        }

        public override void ConnectRefused(UdpEndPoint endpoint, IProtocolToken token)
        {
            var authToken = token as AuthResultToken;

            if (UIConnectAuthController.Singleton.IsAuthOpen && authToken.Result == AuthResultToken.AuthResult.Refused)
            {
                UIConnectAuthController.Singleton.PasswordAuthFailed();
            }
        }

        #endregion

        #region Session updates

        public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
        {
            if (UIServerBrowserController.Singleton.HasRequestedUpdate && sessionList.Count > 0)
            {
                UIServerBrowserController.Singleton.ClearUIServerBrowser();

                foreach (var session in sessionList)
                {
                    UdpSession photonSession = session.Value;

                    if (photonSession.Source == UdpSessionSource.Photon && photonSession.ConnectionsCurrent < photonSession.ConnectionsMax)
                    {
                        ServerInfo serverInfo = new ServerInfo(photonSession);

                        // Don't show the server if it's invite only.
                        if (!serverInfo.Info.ServerIsInviteOnly)
                        {
                            UIServerBrowserController.Singleton.AddUIServerBrowserInfo(serverInfo);
                        }
                    }
                }

                UIServerBrowserController.Singleton.DoneLoading();
            }
        }

        #endregion

    }

}
