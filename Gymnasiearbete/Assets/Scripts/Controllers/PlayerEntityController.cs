using ArenaShooter.Player;
using System.Collections.Generic;
using UnityEngine;

namespace ArenaShooter.Controllers
{

    class PlayerEntityController : Controller<PlayerEntityController>
    {

        #region Private variables

        private List<PlayerController> playerControllers = new List<PlayerController>(4);

        #endregion

        public void AddPlayerController(PlayerController playerController)
        {
            playerControllers.Add(playerController);
        }

        public void RemovePlayerController(PlayerController playerController)
        {
            playerControllers.Remove(playerController);
        }

        public PlayerController GetClosestPlayer(PlayerController skip, Vector3 position, float maxDistance)
        {
            PlayerController closest  = null;
            float            distance = float.MaxValue;

            for (int i = 0; i < playerControllers.Count; i++)
            {
                float pcDist = Vector3.Distance(playerControllers[i].transform.position, position);

                if (pcDist < distance && pcDist <= maxDistance && playerControllers[i] != skip)
                {
                    closest  = playerControllers[i];
                    distance = pcDist;
                }
            }

            return closest;
        }

    }

}
