using ArenaShooter.Networking;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ArenaShooter.Controllers
{

    class UIMatchmakingController : Controller<UIMatchmakingController>
    {

        #region Editor

        [SerializeField] private Button            join;
        [SerializeField] private Button            host;
        [SerializeField] private NetworkController networkController;

        #endregion

        #region Private variables

        private float       waitTime = 5f;
        private IEnumerator method;

        #endregion

        #region Methods

        public void ButtonClick()
        {
            //method = ReActivateJoinButton();
            StartCoroutine(method);
        }

        /*
        IEnumerator ReActivateJoinButton()
        {
            while (!networkController.SessionIsFound && waitTime > 0)
            {
                waitTime -= Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            if (!networkController.SessionIsFound)
            {
                join.interactable = true;
                host.interactable = true;

                if (BoltNetwork.IsRunning)
                {
                    //BoltNetwork.Shutdown();
                }
            }
        }
        */

        #endregion

    }

}
