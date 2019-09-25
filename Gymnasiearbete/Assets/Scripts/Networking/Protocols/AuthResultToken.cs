using Bolt;
using UdpKit;

namespace ArenaShooter.Networking.Protocols
{

    /// <summary>
    /// Sent by the server with the result of the join operation with <see cref="UserToken"/>.
    /// </summary>
    sealed class AuthResultToken : IProtocolToken
    {

        #region Static variables

        public static AuthResultToken invalid = new AuthResultToken("");

        private static int currentTicket = 0;

        #endregion

        #region Public properties
        
        public int Ticket
        {
            get
            {
                return ticket;
            }
        }

        public string Id
        {
            get
            {
                return id;
            }
        }

        #endregion

        #region Private variables

        private int    ticket;
        private string id;

        #endregion

        public AuthResultToken()
        {

        }

        public AuthResultToken(string id = "")
        {
            this.ticket = currentTicket++;
            this.id     = id;
        }

        public void Read(UdpPacket packet)
        {
            this.ticket = packet.ReadInt();
            this.id     = packet.ReadString();
        }

        public void Write(UdpPacket packet)
        {
            packet.WriteInt(this.ticket);
            packet.WriteString(this.id);
        }

    }

}
