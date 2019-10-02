using ArenaShooter.Networking;
using System;
using System.Collections.Generic;

namespace ArenaShooter.Extensions
{

    static class ServerUtils
    {

        #region Public constants

        public const string      DEFAULT_SERVER_NAME        = "Server";
        public const ServerFlags DEFAULT_SERVER_FLAGS       = ServerFlags.InLobby;
        public const int         MIN_SERVER_NAME_LENGTH     = 5;
        public const int         MAX_SERVER_NAME_LENGTH     = 20;
        public const int         MAX_SERVER_PASSWORD_LENGTH = 20;

        #endregion

        #region Public properties

        /// <summary>
        /// Used by the host when creating servers.
        /// </summary>
        public static ServerHostInfo CurrentServerHostInfo { get; set; }

        /// <summary>
        /// Used by the client when joining servers.
        /// </summary>
        public static ServerInfo CurrentServerInfo { get; set; }

        #endregion

        #region Private variables

        private static List<Guid> invitedUserIds = new List<Guid>();

        #endregion

        public static void ClearInvitedUsers()
        {
            invitedUserIds.Clear();
        }

        public static void AddInvitedUser(Guid userId)
        {
            invitedUserIds.Add(userId);
        }

        public static bool HasUserIdBeenInvited(Guid userId)
        {
            return invitedUserIds.Contains(userId);
        }

    }

}
