using ArenaShooter.Controllers;
using ArenaShooter.Player;
using Bolt;
using UnityEngine.SceneManagement;

namespace ArenaShooter.Networking
{

    [BoltGlobalBehaviour(BoltScenes.Game, BoltScenes.DesertMap)]
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

            Profile.Save();
        }

        public override void OnEvent(WaveStartEvent evnt)
        {
            base.OnEvent(evnt);

            if (UIWaveController.Singleton != null)
            {
                UIWaveController.Singleton.WaveStartEvent(evnt);
            }
        }

        public override void OnEvent(WaveEndEvent evnt)
        {
            base.OnEvent(evnt);

            if (UIWaveController.Singleton != null)
            {
                UIWaveController.Singleton.WaveEndEvent(evnt);
            }

            if (PlayerController.Singleton != null)
            {
                /*
                */
                EntityRevivedEvent @event = EntityRevivedEvent.Create(GlobalTargets.Others);
                //EntityRevivedEvent @event = EntityRevivedEvent.Create(PlayerController.Singleton.entity, EntityTargets.Everyone);
                @event.RevivedEntity      = PlayerController.Singleton.entity;
                @event.Send();

                PlayerController.Singleton.Revive(@event);
            }
        }

        public override void OnEvent(WaveCountdownEvent evnt)
        {
            base.OnEvent(evnt);

            if (UIWaveController.Singleton != null)
            {
                UIWaveController.Singleton.WaveCountdownEvent(evnt);
            }
        }

    }

}
