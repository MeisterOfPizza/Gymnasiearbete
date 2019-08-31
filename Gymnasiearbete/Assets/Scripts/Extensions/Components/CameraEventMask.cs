using UnityEngine;

#pragma warning disable 0109
#pragma warning disable 0649

namespace ArenaShooter.Extensions.Components
{

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    class CameraEventMask : MonoBehaviour
    {

        #region Editor

        [SerializeField] private LayerMask eventMask;

        #endregion

        #region Public properties

        public LayerMask EventMask
        {
            get
            {
                return eventMask;
            }

            set
            {
                camera.eventMask = eventMask;
            }
        }

        #endregion

        #region Private variables

        private new Camera camera;

        #endregion

        private void Awake()
        {
            if (camera == null)
            {
                this.camera = GetComponent<Camera>();
            }

            camera.eventMask = eventMask;
        }

        private void OnValidate()
        {
            this.camera = GetComponent<Camera>();
        }

        private void Reset()
        {
            this.camera = GetComponent<Camera>();
        }

    }

}
