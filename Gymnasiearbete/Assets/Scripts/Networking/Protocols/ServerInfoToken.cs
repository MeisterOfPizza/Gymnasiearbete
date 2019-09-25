using ArenaShooter.Extensions;
using Bolt;
using Photon.Realtime;
using UdpKit;
using static ArenaShooter.Extensions.ServerUtils;

namespace ArenaShooter.Networking.Protocols
{

    sealed class ServerInfoToken : IProtocolToken
    {

        #region Public constants

        /// <summary>
        /// Byte
        /// </summary>
        public const string MAP_TEMPLATE_ID_KEY = "MapTemplateId";

        /// <summary>
        /// Boolean
        /// </summary>
        public const string SERVER_IS_PASSWORDED_KEY = "Passworded";

        /// <summary>
        /// Boolean
        /// </summary>
        public const string SERVER_IS_IN_LOBBY_KEY = "InLobby";

        /// <summary>
        /// Boolean
        /// </summary>
        public const string SERVER_IS_INVITE_ONLY_KEY = "InviteOnly";

        /// <summary>
        /// String
        /// </summary>
        public const string CUSTOM_SERVER_NAME_KEY = "ServerName";

        /// <summary>
        /// String
        /// </summary>
        public const string HOST_USERNAME_KEY = "HostUsername";

        /// <summary>
        /// Byte
        /// </summary>
        public const byte HOST_USERNAME_MAX_LENGTH = 20;

        #endregion

        #region Public properties

        public Room Room
        {
            get
            {
                return room;
            }
        }

        public byte MapTemplateId
        {
            get
            {
                return (byte)room.CustomProperties[MAP_TEMPLATE_ID_KEY];
            }
        }

        public string ServerName
        {
            get
            {
                return (string)room.CustomProperties[CUSTOM_SERVER_NAME_KEY];
            }
        }

        public string HostUsername
        {
            get
            {
                return (string)room.CustomProperties[HOST_USERNAME_KEY];
            }
        }

        public bool ServerIsPassworded
        {
            get
            {
                return (bool)room.CustomProperties[SERVER_IS_PASSWORDED_KEY];
            }
        }

        public bool ServerIsInLobby
        {
            get
            {
                return (bool)room.CustomProperties[SERVER_IS_IN_LOBBY_KEY];
            }
        }

        public bool ServerIsInviteOnly
        {
            get
            {
                return (bool)room.CustomProperties[SERVER_IS_INVITE_ONLY_KEY];
            }
        }

        #endregion

        #region Private variables

        private Room room;

        #endregion

        public ServerInfoToken()
        {

        }

        public ServerInfoToken(string serverName, string hostUsername, byte mapTemplateId, bool passworded, bool inviteOnly)
        {
            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = 4,
                CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
            };

            roomOptions.CustomRoomProperties.Add(MAP_TEMPLATE_ID_KEY, mapTemplateId);
            roomOptions.CustomRoomProperties.Add(SERVER_IS_PASSWORDED_KEY, passworded);
            roomOptions.CustomRoomProperties.Add(SERVER_IS_IN_LOBBY_KEY, true);
            roomOptions.CustomRoomProperties.Add(SERVER_IS_INVITE_ONLY_KEY, inviteOnly);
            roomOptions.CustomRoomProperties.Add(CUSTOM_SERVER_NAME_KEY, serverName);
            roomOptions.CustomRoomProperties.Add(HOST_USERNAME_KEY, hostUsername);

            room = new Room(serverName, roomOptions);
        }

        public ServerInfoToken(byte[] data)
        {
            Read(new UdpPacket(data, data.Length));
        }

        public void Read(UdpPacket packet)
        {
            /// Package size:
            /// [ 1B token id | 1B map template id | 1B server flags | (5...20 + 2)B server name | (5...20 + 2)B host username ]

            // Token id offset:
            packet.Position += 8;

            byte        mapTemplateId = packet.ReadByte();
            ServerFlags serverFlags   = (ServerFlags)packet.ReadByte();
            string      serverName    = packet.ReadString();
            string      hostUsername  = packet.ReadString();

            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = 4,
                CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
            };

            roomOptions.CustomRoomProperties.Add(MAP_TEMPLATE_ID_KEY, mapTemplateId);
            roomOptions.CustomRoomProperties.Add(SERVER_IS_PASSWORDED_KEY, serverFlags.HasFlag(ServerFlags.Passworded));
            roomOptions.CustomRoomProperties.Add(SERVER_IS_INVITE_ONLY_KEY, serverFlags.HasFlag(ServerFlags.InviteOnly));
            roomOptions.CustomRoomProperties.Add(SERVER_IS_IN_LOBBY_KEY, serverFlags.HasFlag(ServerFlags.InLobby));
            roomOptions.CustomRoomProperties.Add(CUSTOM_SERVER_NAME_KEY, serverName);
            roomOptions.CustomRoomProperties.Add(HOST_USERNAME_KEY, hostUsername);

            room = new Room(serverName, roomOptions);
        }

        public void Write(UdpPacket packet)
        {
            /// Package size:
            /// [ 1B token id | 1B map template id | 1B server flags | (5...20 + 2)B server name | (5...20 + 2)B host username ]

            string serverName = (string)room.CustomProperties[CUSTOM_SERVER_NAME_KEY];
            serverName = serverName.Clamp(ServerUtils.MIN_SERVER_NAME_LENGTH, ServerUtils.MAX_SERVER_NAME_LENGTH, "?");

            string hostUsername = (string)room.CustomProperties[HOST_USERNAME_KEY];
            hostUsername = hostUsername.Clamp(5, hostUsername.Length > HOST_USERNAME_MAX_LENGTH ? 17 : 20, hostUsername.Length > HOST_USERNAME_MAX_LENGTH ? "." : "?");

            ServerFlags serverFlags = ServerFlags.None;
            if ((bool)room.CustomProperties[SERVER_IS_PASSWORDED_KEY])  serverFlags |= ServerFlags.Passworded;
            if ((bool)room.CustomProperties[SERVER_IS_INVITE_ONLY_KEY]) serverFlags |= ServerFlags.InviteOnly;
            if ((bool)room.CustomProperties[SERVER_IS_IN_LOBBY_KEY])    serverFlags |= ServerFlags.InLobby;

            packet.WriteByte((byte)room.CustomProperties[MAP_TEMPLATE_ID_KEY]);
            packet.WriteByte((byte)serverFlags);
            packet.WriteString(serverName);
            packet.WriteString(hostUsername);
        }

    }

}
