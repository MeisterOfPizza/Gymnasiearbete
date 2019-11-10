using ArenaShooter.Extensions;
using Bolt;
using UdpKit;
using static ArenaShooter.Extensions.ServerUtils;

namespace ArenaShooter.Networking.Protocols
{

    sealed class ServerInfoToken : IProtocolToken
    {

        #region Private constants
        
        private const byte HOST_USERNAME_MAX_LENGTH = 20;

        #endregion

        #region Public properties

        public string ServerName
        {
            get
            {
                return serverName;
            }
        }

        public string HostUsername
        {
            get
            {
                return hostUsername;
            }
        }

        public byte MapTemplateId
        {
            get
            {
                return mapTemplateId;
            }
        }

        public bool ServerIsPassworded
        {
            get
            {
                return serverFlags.HasFlag(ServerFlags.Passworded);
            }
        }

        public bool ServerIsInviteOnly
        {
            get
            {
                return serverFlags.HasFlag(ServerFlags.InviteOnly);
            }
        }

        public bool ServerIsInLobby
        {
            get
            {
                return serverFlags.HasFlag(ServerFlags.InLobby);
            }
        }

        #endregion

        #region Private variables

        private string      serverName;
        private string      hostUsername;
        private byte        mapTemplateId;
        private ServerFlags serverFlags;

        #endregion

        public ServerInfoToken()
        {

        }

        public ServerInfoToken(string serverName, string hostUsername, byte mapTemplateId, bool passworded, bool inviteOnly, bool inLobby)
        {
            this.serverName = serverName;
            this.hostUsername = hostUsername;
            this.mapTemplateId = mapTemplateId;

            this.serverFlags = ServerFlags.None;

            if (passworded)
                serverFlags |= ServerFlags.Passworded;

            if (inviteOnly)
                serverFlags |= ServerFlags.InviteOnly;

            if (inLobby)
                serverFlags |= ServerFlags.InLobby;
        }

        public ServerInfoToken(byte[] data)
        {
            Read(new UdpPacket(data, data.Length));
        }

        public void Read(UdpPacket packet)
        {
            /// Package size:
            /// [ 1B token id | 1B map template id | 1B server flags | (5...20 + 2)B server name | (1...20 + 2)B host username ]

            // Token id offset:
            packet.Position += 8;

            this.mapTemplateId = packet.ReadByte();
            this.serverFlags   = (ServerFlags)packet.ReadByte();
            this.serverName    = packet.ReadString();
            this.hostUsername  = packet.ReadString();
        }

        public void Write(UdpPacket packet)
        {
            /// Package size:
            /// [ 1B token id | 1B map template id | 1B server flags | (5...20 + 2)B server name | (1...20 + 2)B host username ]
            
            serverName = serverName.Clamp(ServerUtils.MIN_SERVER_NAME_LENGTH, ServerUtils.MAX_SERVER_NAME_LENGTH, "?");
            
            hostUsername = hostUsername.Clamp(1, hostUsername.Length > HOST_USERNAME_MAX_LENGTH ? 17 : 20, hostUsername.Length > HOST_USERNAME_MAX_LENGTH ? "." : "?");

            packet.WriteByte(mapTemplateId);
            packet.WriteByte((byte)serverFlags);
            packet.WriteString(serverName);
            packet.WriteString(hostUsername);
        }

    }

}
