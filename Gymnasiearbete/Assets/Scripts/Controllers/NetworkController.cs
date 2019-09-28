using ArenaShooter.Controllers;
using ArenaShooter.Extensions;

namespace ArenaShooter.Networking
{

    class NetworkController : Controller<NetworkController>
    {

        public void StartSingleplayer()
        {
            if (!BoltNetwork.IsRunning)
            {
                BoltLauncher.StartSinglePlayer();
            }
        }

        public void StartClient()
        {
            if (!BoltNetwork.IsRunning)
            {
                BoltLauncher.StartClient();
            }
        }

        public void StartServer()
        {  
            if (BoltNetwork.IsClient)
            {
                BoltLauncher.Shutdown();
            }

            ServerUtils.ClearInvitedUsers();
            BoltLauncher.StartServer();
        }

        public void Disconnect()
        {
            if (BoltNetwork.IsRunning)
            {
                BoltLauncher.Shutdown();
            }
        }

    }

}
