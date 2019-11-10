using ArenaShooter.Controllers;
using Bolt;
using UnityEngine.SceneManagement;

namespace ArenaShooter.Networking
{

    [BoltGlobalBehaviour(BoltScenes.Game)]
    class NetworkGame : GlobalEventListener
    {

        public override void SceneLoadLocalDone(string scene)
        {
            BoltNetwork.Instantiate(BoltPrefabs.Game_Player);
        }

        public override void BoltShutdownBegin(AddCallback registerDoneCallback)
        {
            WeaponController.Singleton.ClearContainers();

            if (BoltNetwork.IsClient)
            {
                SceneManager.LoadScene(0);
            }
        }

    }

}
