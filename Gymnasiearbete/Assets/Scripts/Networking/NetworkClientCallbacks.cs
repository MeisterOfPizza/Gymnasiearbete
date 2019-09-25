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
                BoltLog.Info("Connection accepted. Token: {0}-{1}", acceptToken.Ticket, acceptToken.Id);
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
                BoltLog.Warn("Connection refused. Token: {0}-{1}", authToken.Ticket, authToken.Id);
            }
        }

    }

}
