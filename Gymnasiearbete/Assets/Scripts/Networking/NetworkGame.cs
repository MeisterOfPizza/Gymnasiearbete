using ArenaShooter.Controllers;
using ArenaShooter.Extensions;
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

            GameLogMessageEvent playerJoinedMessageEvent = GameLogMessageEvent.Create(GlobalTargets.Others);
            playerJoinedMessageEvent.Message             = $"{UserUtils.GetUsername()} connected";
            playerJoinedMessageEvent.Send();
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
                PlayerController.Singleton.Revive(null);

                SetEntityActiveEvent setEntityActive = SetEntityActiveEvent.Create(GlobalTargets.Others);
                setEntityActive.Entity               = PlayerController.Singleton.entity;
                setEntityActive.Active               = true;
                setEntityActive.Position             = PlayerController.Transform.position;
                setEntityActive.Send();
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

        public override void OnEvent(WaveNumberEvent evnt)
        {
            base.OnEvent(evnt);

            if (UIWaveController.Singleton != null)
            {
                UIWaveController.Singleton.WaveNumberEvent(evnt);
            }
        }

        public override void OnEvent(WaveProgressEvent evnt)
        {
            base.OnEvent(evnt);

            if (UIWaveController.Singleton != null)
            {
                UIWaveController.Singleton.WaveProgressEvent(evnt);
            }
        }

        public override void OnEvent(SetEntityActiveEvent evnt)
        {
            base.OnEvent(evnt);

            evnt.Entity.gameObject.SetActive(evnt.Active);
            evnt.Entity.transform.position = evnt.Position;
        }

        public override void OnEvent(GameLogMessageEvent evnt)
        {
            base.OnEvent(evnt);

            if (UIGameController.Singleton != null)
            {
                UIGameController.Singleton.GameLogMessageEvent(evnt);
            }
        }

        public override void OnEvent(PlayerKilledEnemyEvent evnt)
        {
            base.OnEvent(evnt);

            if (PlayerController.Singleton != null)
            {
                PlayerController.Singleton.PlayerKilledEnemyEvent(evnt);
            }
        }

    }

}
