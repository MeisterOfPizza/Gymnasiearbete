using UnityEngine;

namespace ArenaShooter.Entities
{

    abstract class Body : MonoBehaviour
    {

        #region Public properties

        /// <summary>
        /// Should the script NOT restrict body part movement?
        /// </summary>
        public bool ManualControls { get; set; } = false;

        public abstract Vector3 UpperBodyCurrent { get; set; }
        public abstract Vector3 LowerBodyCurrent { get; set; }

        #endregion

        #region Updating

        /// <summary>
        /// Sets the upper body target.
        /// </summary>
        /// <param name="target">Point to look at.</param>
        public abstract void SetUpperBodyTarget(Vector3 target);

        /// <summary>
        /// Sets the lower body target.
        /// </summary>
        /// <param name="target">Point to look at.</param>
        public abstract void SetLowerBodyTarget(Vector3 target);

        /// <summary>
        /// Sets the upper body as the controller.
        /// </summary>
        /// <param name="target">Point to look at.</param>
        public abstract void SetUpperBodyAsController(Vector3 target);

        /// <summary>
        /// Sets the lower body as the controller.
        /// </summary>
        /// <param name="target">Point to look at.</param>
        public abstract void SetLowerBodyAsController(Vector3 target);

        #endregion

        #region Helpers

        /// <summary>
        /// Restricts the direction that is legal with the current body part controller.
        /// </summary>
        /// <param name="direction">Normal to restrict.</param>
        /// <returns>Returns a clamp/restricted normal.</returns>
        public abstract Vector3 RestrictNormal(Vector3 direction);

        #endregion

    }

}
