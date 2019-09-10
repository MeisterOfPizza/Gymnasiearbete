using UnityEngine;

namespace ArenaShooter.Controllers
{

    class MobileLookController : Controller<MobileLookController>
    {

        #region Public Variables

        public Vector3 LookDelta { get; set; }
        public bool CanLook { get; set; }

        #endregion

        #region Variables

        // Create a plane at 0,0,0 whose normal points to +Y to be used by the raycast look at for mobile builds.
        private Plane mobileLookAtPlane = new Plane(Vector3.up, Vector3.zero);
        private Vector3 lastPositionToLookAt;

        #endregion

        #region Methods

        public Vector3 GetLookPoint()
        {
            return lastPositionToLookAt;

        }

        public void SetMobileLookAtPoint(Vector3 screenPoint)
        {
            Ray ray = MainCameraController.MainCamera.ScreenPointToRay(screenPoint);

            // If the ray hits the plane...
            if (mobileLookAtPlane.Raycast(ray, out float distance))
            {
                lastPositionToLookAt = ray.GetPoint(distance);
            }   

        }


        #endregion

    }

}
