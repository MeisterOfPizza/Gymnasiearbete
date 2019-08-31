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

        public void StartServer()
        {
            BoltLauncher.StartServer(new BoltConfig() { serverConnectionLimit = 4 });
        }

        public void StartClient()
        {
            BoltLauncher.StartClient();
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

                if (photonSession.Source == UdpSessionSource.Photon)
                {
                    BoltNetwork.Connect(photonSession);
                }
            }
        }

    }

}
