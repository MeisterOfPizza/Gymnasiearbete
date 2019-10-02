using ArenaShooter.Extensions;
using ArenaShooter.Networking.Protocols;
using Bolt;
using System;
using UdpKit;

namespace ArenaShooter.Networking
{

    [BoltGlobalBehaviour(BoltNetworkModes.Server)]
    class NetworkServerCallbacks : GlobalEventListener
    {

        private bool AuthUser(string userId, string password)
        {
            Guid.TryParse(userId, out Guid id);

            return (ServerUtils.CurrentServerHostInfo.ServerIsInviteOnly && id != null && ServerUtils.HasUserIdBeenInvited(id))||
                   (!ServerUtils.CurrentServerHostInfo.ServerIsInviteOnly && ServerUtils.CurrentServerHostInfo.ServerPassword.Equals(password)) ||
                   (!ServerUtils.CurrentServerHostInfo.ServerIsInviteOnly && !ServerUtils.CurrentServerHostInfo.ServerHasPassword);
        }

        public override void ConnectRequest(UdpEndPoint endpoint, IProtocolToken token)
        {
            BoltLog.Warn("Connect request");

            var userToken = token as UserToken;

            if (userToken != null)
            {
                if (AuthUser(userToken.UserId, userToken.Password))
                {
                    AuthResultToken resultToken = AuthResultToken.Accepted;

                    BoltNetwork.Accept(endpoint, resultToken);

                    return;
                }
            }

            BoltNetwork.Refuse(endpoint, AuthResultToken.Refused);
        }

    }

}
