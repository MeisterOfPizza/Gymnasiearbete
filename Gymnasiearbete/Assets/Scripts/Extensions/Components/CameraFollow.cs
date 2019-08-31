using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Extensions.Components
{

    class CameraFollow : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private new Camera camera;

        [Space]
        [SerializeField] private Transform target;
        [SerializeField] private Vector3   offset;
        [SerializeField] private float     followSpeed = 2.5f;

        [Space]
        [SerializeField] private CameraFollowType followType = CameraFollowType.Lerp;

        #endregion

        #region Public properties

        public Camera Camera
        {
            get
            {
                return camera;
            }
        }

        #endregion

        #region Enums

        private enum CameraFollowType
        {
            MoveTowards,
            Lerp,
            Slerp,
            Constant
        }

        #endregion

        public void Initialize(Transform target)
        {
            this.target = target;

            SetPosition(target.position);
            SetCameraLookAt(target.position);
        }

        private void LateUpdate()
        {
            if (target != null)
            {
                switch (followType)
                {
                    case CameraFollowType.MoveTowards:
                        transform.position = Vector3.MoveTowards(transform.position, target.position + offset, followSpeed * Time.deltaTime);
                        break;
                    case CameraFollowType.Lerp:
                        transform.position = Vector3.Lerp(transform.position, target.position + offset, followSpeed * Time.deltaTime);
                        break;
                    case CameraFollowType.Slerp:
                        transform.position = Vector3.Slerp(transform.position, target.position + offset, followSpeed * Time.deltaTime);
                        break;
                    case CameraFollowType.Constant:
                        transform.position = target.position + offset;
                        break;
                }
            }
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position + offset;
        }

        public void SetCameraLookAt(Vector3 position)
        {
            camera.transform.LookAt(position);
        }

    }

}
