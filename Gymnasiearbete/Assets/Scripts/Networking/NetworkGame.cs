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
                PlayerController.Singleton.Revive(null);

                SetEntityActive setEntityActive = SetEntityActive.Create(GlobalTargets.Others);
                setEntityActive.Entity          = PlayerController.Singleton.entity;
                setEntityActive.Active          = true;
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

        public override void OnEvent(SetEntityActive evnt)
        {
            base.OnEvent(evnt);

            evnt.Entity.gameObject.SetActive(evnt.Active);
        }

    }

}
