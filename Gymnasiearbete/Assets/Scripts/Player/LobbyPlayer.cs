using ArenaShooter.Controllers;
using ArenaShooter.Extensions;
using ArenaShooter.UI;
using Bolt;
using System.Collections;
using UnityEngine;

namespace ArenaShooter.Player
{

    [RequireComponent(typeof(BoltEntity))]
    sealed class LobbyPlayer : EntityEventListener<ILobbyPlayerInfoState>
    {

        #region Private variables

        private UILobbyPlayerInfo uiLobbyPlayerInfo;

        #endregion

        private void Start()
        {
            uiLobbyPlayerInfo = UIServerLobbyController.Singleton.CreateUILobbyPlayerInfo(this);
            uiLobbyPlayerInfo.Initialize(this);
            uiLobbyPlayerInfo.UpdateUI();

            state.AddCallback("Ready", () =>
            {
                uiLobbyPlayerInfo.UpdateUI();
                OnReadyToggled();
            });

            state.AddCallback("Weapon", () =>
            {
                uiLobbyPlayerInfo.UpdateUI();
            });
        }

        public override void Attached()
        {
            if (entity.IsOwner)
            {
                state.Name = UserUtils.GetUsername() + (BoltNetwork.IsServer ? " (Host)" : "");

                // Set ready state to ready if the local machine is the server/host:
                state.Ready = BoltNetwork.IsServer;

                UIServerLobbyController.Singleton.SetLobbyPlayer(this);

                LoadoutController.Singleton.OnLoadoutChanged += LoadoutChanged;

                entity.TakeControl();
            }

            if (BoltNetwork.IsServer)
            {
                ServerLobbyController.Singleton.AddLobbyPlayer(this);
                UIServerLobbyController.Singleton.CheckIfClientsAreReady();
            }
        }

        public override void Detached()
        {
            if (BoltNetwork.IsServer)
            {
                ServerLobbyController.Singleton?.RemoveLobbyPlayer(this);
            }

            UIServerLobbyController.Singleton?.SetLobbyPlayer(null);

            if (uiLobbyPlayerInfo != null)
            {
                Destroy(uiLobbyPlayerInfo.gameObject);
            }

            if (entity.IsOwner)
            {
                LoadoutController.Singleton.OnLoadoutChanged -= LoadoutChanged;
            }
        }

        #region Events

        public void ToggleReady()
        {
            state.Ready = !state.Ready;
        }

        private void OnReadyToggled()
        {
            if (BoltNetwork.IsServer)
            {
                UIServerLobbyController.Singleton.CheckIfClientsAreReady();
            }
        }

        public void HostStartMatchCountdown()
        {
            LobbyMatchStartEvent @event = LobbyMatchStartEvent.Create(entity);
            @event.Send();

            StartCoroutine("StartMatchCountdown");
        }

        private IEnumerator StartMatchCountdown()
        {
            float countdown    = 5f;
            int   countDownInt = (int)countdown;

            LobbyCountdownEvent countdownEvent = LobbyCountdownEvent.Create(entity);
            countdownEvent.Time = countDownInt;
            countdownEvent.Send();

            while (countdown > 0 && BoltNetwork.IsServer)
            {
                countdown -= Time.deltaTime;

                // Decrease the amount of network event calls in order to decrease bandwidth usage:
                int tmpCountdown = Mathf.CeilToInt(countdown);

                if (tmpCountdown != countDownInt)
                {
                    countDownInt = tmpCountdown;
                    
                    countdownEvent.Time = tmpCountdown;
                    countdownEvent.Send();
                }

                yield return new WaitForEndOfFrame();
            }

            if (BoltNetwork.IsServer)
            {
                UIServerLobbyController.Singleton.StartMatch();
            }
        }

        private void LoadoutChanged()
        {
            state.Weapon.WeaponStockId  = LoadoutController.Singleton.CurrentLoadout.StockTemplate.TemplateId;
            state.Weapon.WeaponBodyId   = LoadoutController.Singleton.CurrentLoadout.BodyTemplate.TemplateId;
            state.Weapon.WeaponBarrelId = LoadoutController.Singleton.CurrentLoadout.BarrelTemplate.TemplateId;
        }

        #endregion

        #region Event callbacks

        public override void OnEvent(LobbyMatchStartEvent evnt)
        {
            UIServerLobbyController.Singleton.LockLobbyScreen();
        }

        public override void OnEvent(LobbyCountdownEvent evnt)
        {
            UIServerLobbyController.Singleton.UpdateMatchStartCountdown(evnt.Time);
        }

        #endregion

    }

}
