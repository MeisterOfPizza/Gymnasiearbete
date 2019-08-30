using Bolt;

namespace ArenaShooter.Networking
{

    [BoltGlobalBehaviour]
    class NetworkCallbacks : GlobalEventListener
    {

        public override void SceneLoadLocalDone(string scene)
        {
            BoltNetwork.Instantiate(BoltPrefabs.Game_Player);
        }

    }

}
