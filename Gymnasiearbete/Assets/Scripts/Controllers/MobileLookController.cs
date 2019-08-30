using UnityEngine;

namespace ArenaShooter.Controllers
{

    class MobileLookController : Controller<MobileLookController>
    {

        public Vector3 GetLookDirection()
        {
            return Vector3.forward;
        }

    }

}
