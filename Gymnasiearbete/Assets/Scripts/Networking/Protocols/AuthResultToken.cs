using Bolt;
using UdpKit;

#pragma warning disable 0660
#pragma warning disable 0661

namespace ArenaShooter.Networking.Protocols
{

    /// <summary>
    /// Sent by the server with the result of the join operation with <see cref="UserToken"/>.
    /// </summary>
    sealed class AuthResultToken : IProtocolToken
    {

        #region Public static properties

        public static AuthResultToken Accepted
        {
            get
            {
                return new AuthResultToken(AuthResult.Accepted);
            }
        }

        public static AuthResultToken Refused
        {
            get
            {
                return new AuthResultToken(AuthResult.Refused);
            }
        }

        #endregion

        #region Private static variables

        private static int currentTicketCount = 0;

        #endregion

        #region Public properties

        public AuthResult Result
        {
            get
            {
                return authResult;
            }
        }

        #endregion

        #region Private variables

        private AuthResult authResult;
        private int        ticketCount;

        #endregion

        #region Internal enum

        internal enum AuthResult : byte
        {
            Accepted,
            Refused
        }

        #endregion

        public AuthResultToken()
        {
            
        }

        public AuthResultToken(AuthResult authResult)
        {
            this.authResult  = authResult;
            this.ticketCount = currentTicketCount++;
        }

        public void Read(UdpPacket packet)
        {
            this.authResult  = (AuthResult)packet.ReadByte();
            this.ticketCount = packet.ReadInt();
        }

        public void Write(UdpPacket packet)
        {
            packet.WriteByte((byte)authResult);
            packet.WriteInt(ticketCount);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", authResult, ticketCount);
        }

    }

}
