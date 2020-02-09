using ArenaShooter.Networking.Protocols;
using Bolt;
using UdpKit;

namespace ArenaShooter.Networking
{

    [BoltGlobalBehaviour(BoltNetworkModes.Client)]
    class NetworkClientCallbacks : GlobalEventListener
    {

        public override void Connected(BoltConnection connection)
        {
            BoltLog.Info("Connection accepted!");

            AuthResultToken acceptToken = connection.AcceptToken as AuthResultToken;

            if (acceptToken != null)
            {
                BoltLog.Info("AcceptToken: " + acceptToken.GetType());
                BoltLog.Info("Connection accepted." + acceptToken.ToString());
            }
            else
            {
                BoltLog.Warn("AcceptToken is NULL");
            }
        }

        public override void ConnectRefused(UdpEndPoint endpoint, IProtocolToken token)
        {
            var authToken = token as AuthResultToken;

            if (authToken != null)
            {
                BoltLog.Warn("AuthToken: " + authToken.GetType());
                BoltLog.Warn("Connection refused. Token: " + authToken.ToString());
            }
        }

    }

}
