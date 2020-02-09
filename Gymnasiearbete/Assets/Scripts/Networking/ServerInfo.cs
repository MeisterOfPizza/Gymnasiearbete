using ArenaShooter.Networking.Protocols;
using UdpKit;

namespace ArenaShooter.Networking
{

    /// <summary>
    /// Used to list servers in the server browser.
    /// </summary>
    sealed class ServerInfo
    {

        #region Public properties

        public UdpSession UdpSession
        {
            get
            {
                return udpSession;
            }
        }

        public ServerInfoToken Info
        {
            get
            {
                return serverInfoToken;
            }
        }

        public string PlayerCount
        {
            get
            {
                return udpSession.ConnectionsCurrent + " / " + udpSession.ConnectionsMax;
            }
        }

        #endregion

        #region Private variables

        private UdpSession      udpSession;
        private ServerInfoToken serverInfoToken;

        #endregion

        public ServerInfo(UdpSession udpSession)
        {
            this.udpSession = udpSession.Clone();

            this.serverInfoToken = new ServerInfoToken(udpSession.HostData);
        }

    }

}
