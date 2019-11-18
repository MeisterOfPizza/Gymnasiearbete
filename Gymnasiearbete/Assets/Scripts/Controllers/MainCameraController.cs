using UnityEngine;

namespace ArenaShooter.Controllers
{

    class MainCameraController : Controller<MainCameraController>
    {

        #region Public static properties

        public static Camera MainCamera
        {
            get
            {
                return Singleton.mainCamera;
            }
        }

        #endregion

        #region Private variables

        private Camera mainCamera;

        #endregion

        public void SetMainCamera(Camera mainCamera)
        {
            this.mainCamera = mainCamera;
        }

    }

}
