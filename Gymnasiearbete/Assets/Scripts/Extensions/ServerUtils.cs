using ArenaShooter.Networking;
using ArenaShooter.Templates.Maps;
using System;
using System.Collections.Generic;

namespace ArenaShooter.Extensions
{

    static class ServerUtils
    {

        #region Public constants

        public const int MIN_SERVER_NAME_LENGTH = 5;
        public const int MAX_SERVER_NAME_LENGTH = 20;
        
        public const int MAX_SERVER_PASSWORD_LENGTH = 20;

        #endregion

        #region Public properties

        public static string      ServerName        { get; private set; }
        public static string      ServerPassword    { get; private set; }
        public static ServerFlags Flags             { get; private set; }
        public static MapTemplate ServerMapTemplate { get; set; }

        public static bool ServerHasPassword
        {
            get
            {
                return Flags.HasFlag(ServerFlags.Passworded);
            }
        }

        public static bool ServerIsInviteOnly
        {
            get
            {
                return Flags.HasFlag(ServerFlags.InviteOnly);
            }
        }

        public static bool ServerIsInLobby
        {
            get
            {
                return Flags.HasFlag(ServerFlags.InLobby);
            }
        }

        public static ServerInfo CurrentServerInfo { get; set; }

        #endregion

        #region Private variables

        private static List<Guid> invitedUserIds = new List<Guid>();

        #endregion

        #region Enums

        [Flags]
        internal enum ServerFlags : byte
        {
            None = 0,
            Passworded = 1,
            InviteOnly = 2,
            InLobby = 4
        }

        #endregion

        public static void AddInvitedUser(Guid userId)
        {
            invitedUserIds.Add(userId);
        }

        public static bool HasUserIdBeenInvited(Guid userId)
        {
            return invitedUserIds.Contains(userId);
        }

        public static string SetServerName(string name)
        {
            ServerName = name.ToASCII().Clamp(MIN_SERVER_NAME_LENGTH, MAX_SERVER_NAME_LENGTH, "?");

            return ServerName;
        }

        public static string SetServerPassword(string password)
        {
            ServerPassword = password.ToASCII().Truncate(MAX_SERVER_PASSWORD_LENGTH);

            if (!string.IsNullOrEmpty(ServerPassword))
            {
                Flags |= ServerFlags.Passworded;
            }
            else
            {
                Flags &= ~ServerFlags.Passworded;
            }

            return ServerPassword;
        }

        public static void SetServerInviteOnly(bool inviteOnly)
        {
            if (inviteOnly)
            {
                Flags |= ServerFlags.InviteOnly;
            }
            else
            {
                Flags &= ~ServerFlags.InviteOnly;
            }
        }

        public static void SetServerInLobby(bool inLobby)
        {
            if (inLobby)
            {
                Flags |= ServerFlags.InLobby;
            }
            else
            {
                Flags &= ~ServerFlags.InLobby;
            }
        }

    }

}
