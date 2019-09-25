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

            return (ServerUtils.ServerIsInviteOnly && id != null && ServerUtils.HasUserIdBeenInvited(id))||
                   (!ServerUtils.ServerIsInviteOnly && ServerUtils.ServerPassword.Equals(password)) ||
                   (!ServerUtils.ServerIsInviteOnly && !ServerUtils.ServerHasPassword);
        }

        public override void ConnectRequest(UdpEndPoint endpoint, IProtocolToken token)
        {
            BoltLog.Warn("Connect request");

            var userToken = token as UserToken;

            if (userToken != null)
            {
                if (AuthUser(userToken.UserId, userToken.Password))
                {
                    AuthResultToken resultToken = new AuthResultToken(Guid.NewGuid().ToString());

                    BoltNetwork.Accept(endpoint, resultToken);

                    return;
                }
            }

            BoltNetwork.Refuse(endpoint, AuthResultToken.invalid);
        }

    }

}
