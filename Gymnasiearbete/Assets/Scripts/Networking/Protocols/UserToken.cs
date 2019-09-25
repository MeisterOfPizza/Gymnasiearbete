using Bolt;
using UdpKit;

namespace ArenaShooter.Networking.Protocols
{

    /// <summary>
    /// Sent by the client with it's authentication info.
    /// </summary>
    sealed class UserToken : IProtocolToken
    {

        #region Public properties

        public string UserId
        {
            get
            {
                return userId;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
        }

        #endregion

        #region Private variables

        private string userId;
        private string password;

        #endregion

        public UserToken()
        {

        }

        public UserToken(string userId, string password)
        {
            this.userId   = userId;
            this.password = password;
        }

        public void Read(UdpPacket packet)
        {
            this.userId   = packet.ReadString();
            this.password = packet.ReadString();
        }

        public void Write(UdpPacket packet)
        {
            packet.WriteString(this.userId);
            packet.WriteString(this.password);
        }

    }

}
