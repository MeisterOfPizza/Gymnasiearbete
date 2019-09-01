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

        public PlayerController GetClosestPlayer(Vector3 position, float maxDistance)
        {
            PlayerController closest  = playerControllers[0];
            float            distance = float.MaxValue;

            for (int i = 1; i < playerControllers.Count; i++)
            {
                float pcDist = Vector3.Distance(playerControllers[i].transform.position, closest.transform.position);

                if (pcDist < distance && pcDist <= maxDistance)
                {
                    closest  = playerControllers[i];
                    distance = pcDist;
                }
            }
            /*
            foreach (var pc in playerControllers)
            {
                float pcDist = Vector3.Distance(pc.transform.position, closest.transform.position);

                if (pcDist < distance && pcDist <= maxDistance)
                {
                    closest  = pc;
                    distance = pcDist;
                }
            }
            */

            return closest;
        }

    }

}
