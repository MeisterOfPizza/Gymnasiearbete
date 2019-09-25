using ArenaShooter.Controllers;
using ArenaShooter.Networking.Protocols;
using Bolt;
using Bolt.Matchmaking;
using System;
using UdpKit;

#pragma warning disable 0649

namespace ArenaShooter.Networking
{
    
    class NetworkController : GlobalEventListener
    {

        #region Private variables
        
        private bool sessionListUpdated = false;

        #endregion

        public void StartServer()
        {  
            if (BoltNetwork.IsClient)
            {
                BoltLauncher.Shutdown();
            }

            BoltLauncher.StartServer();
        }

        public void StartClient()
        {
            if (!BoltNetwork.IsRunning)
            {
                sessionListUpdated = false;

                BoltLauncher.StartClient();
            }
        }

        public void Disconnect()
        {
            if (BoltNetwork.IsRunning)
            {
                BoltLauncher.Shutdown();
            }
        }

        public override void BoltStartBegin()
        {
            BoltNetwork.RegisterTokenClass<UserToken>();
            BoltNetwork.RegisterTokenClass<AuthResultToken>();
            BoltNetwork.RegisterTokenClass<ServerInfoToken>();
        }

        public override void BoltStartDone()
        {
            if (BoltNetwork.IsServer)
            {
                string sessionId = Guid.NewGuid().ToString();
                
                BoltMatchmaking.CreateSession(sessionId, UIServerSetupController.Singleton.GetServerInfoToken());
            }
        }

        public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
        {
            if (!sessionListUpdated)
            {
                sessionListUpdated = true;

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
            }
        }

    }

}
