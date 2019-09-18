using Bolt;
using Bolt.Matchmaking;
using System;
using UdpKit;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Networking
{

    class NetworkController : GlobalEventListener
    {

        #region Editor

        [Header("References")]
        [SerializeField] private string gameScene;

        #endregion

        #region Private Variables

        private bool serverIsOnline = false;
        private bool sessionIsFound = false;

        #endregion

        public void StartServer()
        {  
            BoltLauncher.StartServer(new BoltConfig() { serverConnectionLimit = 4 });
            serverIsOnline = true;
        }

        public void StartClient()
        {
            if (serverIsOnline)
            {
                BoltLauncher.StartClient();
            }
        }

        public override void BoltStartDone()
        {
            if (BoltNetwork.IsServer)
            {
                string sessionId = Guid.NewGuid().ToString();

                BoltMatchmaking.CreateSession(sessionId, sceneToLoad: gameScene);
            }
        }

        public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
        {
            foreach (var session in sessionList)
            {
                UdpSession photonSession = session.Value;

                if (photonSession.Source == UdpSessionSource.Photon && photonSession.ConnectionsCurrent < photonSession.ConnectionsMax)
                {
                    BoltNetwork.Connect(photonSession);
                    sessionIsFound = true;
                }
            }
        }

        #region Getters

        public bool SessionIsFound
        {
            get
            {
                return sessionIsFound;
            }
        }

        #endregion

    }

}
