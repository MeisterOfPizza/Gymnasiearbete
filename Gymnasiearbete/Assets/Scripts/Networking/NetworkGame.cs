using Bolt;

namespace ArenaShooter.Networking
{

    [BoltGlobalBehaviour(BoltScenes.Game)]
    class NetworkGame : GlobalEventListener
    {

        public override void SceneLoadLocalDone(string scene)
        {
            BoltNetwork.Instantiate(BoltPrefabs.Game_Player);
        }

    }

}
