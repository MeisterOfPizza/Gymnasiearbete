using ArenaShooter.Extensions;
using ArenaShooter.Networking.Protocols;
using ArenaShooter.Templates.Maps;
using System;

namespace ArenaShooter.Networking
{

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

    /// <summary>
    /// Used by the host to change server info during server setup and gameplay.
    /// </summary>
    sealed class ServerHostInfo
    {

        #region Public properties

        public string      ServerName        { get; private set; }
        public string      ServerPassword    { get; private set; }
        public ServerFlags Flags             { get; private set; }
        public MapTemplate ServerMapTemplate { get; set; }

        public bool ServerHasPassword
        {
            get
            {
                return Flags.HasFlag(ServerFlags.Passworded);
            }
        }

        public bool ServerIsInviteOnly
        {
            get
            {
                return Flags.HasFlag(ServerFlags.InviteOnly);
            }
        }

        public bool ServerIsInLobby
        {
            get
            {
                return Flags.HasFlag(ServerFlags.InLobby);
            }
        }

        #endregion

        public ServerHostInfo(MapTemplate serverMapTemplate)
        {
            this.ServerName        = ServerUtils.DEFAULT_SERVER_NAME + UnityEngine.Random.Range(0, 10).ToString() + UnityEngine.Random.Range(0, 10).ToString() + UnityEngine.Random.Range(0, 10).ToString();
            this.ServerPassword    = "";
            this.Flags             = ServerFlags.InLobby;
            this.ServerMapTemplate = serverMapTemplate;
        }

        #region Setter methods

        public string SetServerName(string name)
        {
            ServerName = name.ToASCII().Clamp(ServerUtils.MIN_SERVER_NAME_LENGTH, ServerUtils.MAX_SERVER_NAME_LENGTH, "?");

            return ServerName;
        }

        public string SetServerPassword(string password)
        {
            ServerPassword = password.ToASCII().Truncate(ServerUtils.MAX_SERVER_PASSWORD_LENGTH);

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

        public void SetServerInviteOnly(bool inviteOnly)
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

        public void SetServerInLobby(bool inLobby)
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

        #endregion

        #region Helper methods

        public ServerInfoToken CreateServerInfoToken()
        {
            return new ServerInfoToken(
                this.ServerName, 
                UserUtils.GetUsername(), 
                ServerMapTemplate.TemplateId, 
                Flags.HasFlag(ServerFlags.Passworded), 
                Flags.HasFlag(ServerFlags.InviteOnly), 
                Flags.HasFlag(ServerFlags.InLobby)
                );
        }

        #endregion

    }

}
