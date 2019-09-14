using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Entities
{

    class HumanoidBody : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private Transform container;
        [SerializeField] private Transform upperBody;
        [SerializeField] private Transform lowerBody;

        [Header("Logic")]
        [SerializeField] private bool  upperBodyCanTurnSeparately = true;
        [SerializeField] private float upperBodyTurnSpeed         = 3f;
        [SerializeField] private float lowerBodyTurnSpeed         = 3f;

        [Help("How far can the non-controller body part turn (in angles, deg) away from forward of the controller body part?")]
        [SerializeField, Range(0f, 135f)] private float bodyMaxAngleOffset = 45f;

        [Space]
        [SerializeField] private bool upperBodyOffsetReset;
        [SerializeField] private bool lowerBodyOffsetReset;

        #endregion

        #region Public properties

        /// <summary>
        /// Should the script NOT restrict body part movement?
        /// </summary>
        public bool ManualControls { get; set; } = false;

        /// <summary>
        /// Returns true if <see cref="lowerBody"/> decides where <see cref="upperBody"/> should look.
        /// </summary>
        public bool LowerBodyIsController
        {
            get
            {
                return lowerBodyIsController;
            }
        }

        public Vector3 UpperBodyCurrent
        {
            get
            {
                return upperBodyCurrent;
            }

            set
            {
                upperBodyCurrent = value;
            }
        }

        public Vector3 LowerBodyCurrent
        {
            get
            {
                return lowerBodyCurrent;
            }

            set
            {
                lowerBodyCurrent = value;
            }
        }

        #endregion

        #region Private variables

        private Vector3 upperBodyCurrent = Vector3.forward;
        private Vector3 lowerBodyCurrent = Vector3.forward;

        private Vector3 upperBodyTarget = Vector3.forward;
        private Vector3 lowerBodyTarget = Vector3.forward;

        private bool lowerBodyIsController = true;

        #endregion

        private void Start()
        {
            upperBodyCurrent = container.forward;
            lowerBodyCurrent = container.forward;

            upperBodyTarget = container.forward;
            lowerBodyTarget = container.forward;
        }

        #region Updating

        /// <summary>
        /// Sets the upper body target.
        /// </summary>
        /// <param name="target">Point to look at.</param>
        public void SetUpperBodyTarget(Vector3 target)
        {
            target.y        = container.position.y;
            Vector3 forward = target - container.position;
            
            if (lowerBodyIsController)
            {
                forward = RestrictNormal(forward, lowerBodyCurrent);
            }
            
            upperBodyTarget = forward;
        }
        
        /// <summary>
        /// Sets the lower body target.
        /// </summary>
        /// <param name="target">Point to look at.</param>
        public void SetLowerBodyTarget(Vector3 target)
        {
            target.y        = container.position.y;
            Vector3 forward = target - container.position;
            
            if (!lowerBodyIsController)
            {
                forward = RestrictNormal(forward, upperBodyCurrent);
            }
            
            lowerBodyTarget = forward;
        }

        /// <summary>
        /// Sets the upper body as the controller.
        /// </summary>
        /// <param name="target">Point to look at.</param>
        public void SetUpperBodyAsController(Vector3 target)
        {
            target.y = container.position.y;

            lowerBodyIsController = false;
            upperBodyTarget       = target - container.position;
        }

        /// <summary>
        /// Sets the lower body as the controller.
        /// </summary>
        /// <param name="target">Point to look at.</param>
        public void SetLowerBodyAsController(Vector3 target)
        {
            target.y = container.position.y;

            lowerBodyIsController = true;
            lowerBodyTarget       = target - container.position;
        }

        private void Update()
        {
            if (ManualControls)
            {
                upperBody.forward = upperBodyCurrent;
                lowerBody.forward = lowerBodyCurrent;
            }
            else
            {
                if (lowerBodyIsController)
                {
                    lowerBody.forward = Quaternion.Slerp(Quaternion.LookRotation(lowerBody.forward), Quaternion.LookRotation(container.forward), Time.deltaTime * lowerBodyTurnSpeed) * Vector3.forward;
                    lowerBodyCurrent  = lowerBodyTarget = lowerBody.forward;

                    if (upperBodyCanTurnSeparately)
                    {
                        upperBodyCurrent  = Quaternion.Slerp(Quaternion.LookRotation(upperBodyCurrent), Quaternion.LookRotation(upperBodyTarget), Time.deltaTime * upperBodyTurnSpeed) * Vector3.forward;
                        upperBody.forward = upperBodyCurrent;

                        RestrictUpperBody();
                    }
                    else
                    {
                        upperBody.forward = lowerBody.forward;
                    }
                }
                else
                {
                    upperBody.forward = Quaternion.Slerp(Quaternion.LookRotation(upperBody.forward), Quaternion.LookRotation(container.forward), Time.deltaTime * upperBodyTurnSpeed) * Vector3.forward;
                    upperBodyCurrent  = upperBodyTarget = upperBody.forward;

                    lowerBodyCurrent  = Quaternion.Slerp(Quaternion.LookRotation(lowerBodyCurrent), Quaternion.LookRotation(lowerBodyTarget), Time.deltaTime * lowerBodyTurnSpeed) * Vector3.forward;
                    lowerBody.forward = lowerBodyCurrent;

                    RestrictLowerBody();
                }
            }
        }

        #endregion

        #region Restricting

        private void RestrictUpperBody()
        {
            float angleDiff = Vector3.SignedAngle(lowerBodyCurrent, upperBodyCurrent, Vector3.up); // True angle between the normals.
            float angleDir = angleDiff > 0f ? 1f : -1f; // Which direction should we turn?

            if (Mathf.Abs(angleDiff) > bodyMaxAngleOffset)
            {
                Vector3 targetForward = Quaternion.Euler(0, bodyMaxAngleOffset * angleDir, 0) * lowerBodyCurrent;

                upperBodyCurrent = targetForward;
                upperBodyTarget = upperBodyOffsetReset ? lowerBodyCurrent : targetForward; // Reset the normal when outside or let it drag?
            }
        }

        private void RestrictLowerBody()
        {
            float angleDiff = Vector3.SignedAngle(upperBodyCurrent, lowerBodyCurrent, Vector3.up); // True angle between the normals.
            float angleDir  = angleDiff > 0f ? 1f : -1f; // Which direction should we turn?

            if (Mathf.Abs(angleDiff) > bodyMaxAngleOffset)
            {
                Vector3 targetForward = Quaternion.Euler(0, bodyMaxAngleOffset * angleDir, 0) * upperBodyCurrent;

                lowerBodyCurrent = targetForward;
                lowerBodyTarget  = lowerBodyOffsetReset ? upperBodyCurrent : targetForward; // Reset the normal when outside or let it drag?
            }
        }

        private Vector3 RestrictNormal(Vector3 normal, Vector3 restrictor)
        {
            float angleDiff = Vector3.SignedAngle(restrictor, normal, Vector3.up); // True angle between the normals.
            float angleDir  = angleDiff > 0f ? 1f : -1f; // Which direction should we turn?

            if (Mathf.Abs(angleDiff) > bodyMaxAngleOffset)
            {
                return Quaternion.Euler(0, bodyMaxAngleOffset * angleDir, 0) * restrictor;
            }

            return normal;
        }

        #endregion

    }

}
