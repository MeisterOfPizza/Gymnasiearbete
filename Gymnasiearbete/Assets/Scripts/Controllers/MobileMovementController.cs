using UnityEngine;
using UnityEngine.Events;

namespace ArenaShooter.Controllers
{

    class MobileMovementController : Controller<MobileMovementController>
    {

        #region Public variables

        public Vector2 MovementDelta { get; set; }

        #endregion

        public Vector3 GetMovement()
        {
            return new Vector3(MovementDelta.x, 0, MovementDelta.y);
        }

    }

}
