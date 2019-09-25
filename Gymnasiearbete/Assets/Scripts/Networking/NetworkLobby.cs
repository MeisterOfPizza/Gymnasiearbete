using ArenaShooter.Controllers;
using Bolt;
using UdpKit;

namespace ArenaShooter.Networking
{

    [BoltGlobalBehaviour]
    class NetworkLobby : GlobalEventListener
    {

        public override void BoltStartDone()
        {
            UIMainMenuController.Singleton.CloseMainMenu();

            if (BoltNetwork.IsClient)
            {
                UIServerBrowserController.Singleton.OpenServerBrowser();
            }
        }

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
            if (UIConnectAuthController.Singleton.IsAuthOpen)
            {
                UIConnectAuthController.Singleton.CloseConnectAuthWindow();
            }
        }

        public override void BoltShutdownBegin(AddCallback registerDoneCallback)
        {
            UIServerSetupController.Singleton.CloseServerSetup();
            UIServerLobbyController.Singleton.CloseLobby();
            UIServerBrowserController.Singleton.CloseServerBrowser();
            UIConnectAuthController.Singleton.CloseConnectAuthWindow();
            UIMainMenuController.Singleton.OpenMainMenu();
            ServerLobbyController.Singleton.ClearLobbyPlayers();
        }

    }

}
