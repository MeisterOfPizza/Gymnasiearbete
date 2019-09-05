using UnityEngine;

namespace ArenaShooter.Controllers
{

    class MobileMovementController : Controller<MobileMovementController>
    {
        
        public Vector3 GetMovement()
        {
            return Vector3.zero;
        }

        public void UpdateMovement(Vector2 joystickDelta)
        {
            
        }

    }

}
