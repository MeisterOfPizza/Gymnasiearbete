using ArenaShooter.Controllers;
using ArenaShooter.Extensions.Components;
using Bolt;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Player
{

    [RequireComponent(typeof(CharacterController))]
    class PlayerMovementController : EntityBehaviour<IPlayerState>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private MeshRenderer        meshRenderer;

        [Header("Prefabs")]
        [SerializeField] private GameObject cameraFollowPrefab;

        [Header("Values")]
        [SerializeField] private float moveSpeed = 10f;

        [Space]
        [SerializeField] private LayerMask lookRayLayerMask;

        [Space]
        public bool canMove = true;
        public bool canLook = true;

        #endregion

        #region Private variables

        private CameraFollow cameraFollow;
        
        private Color playerColor = Color.black;

        private Material materialClone;

        // Create a plane at 0,0,0 whose normal points to +Y to be used by the raycast look at for standalone builds.
#if UNITY_STANDALONE
        private Plane mouseLookAtPlane = new Plane(Vector3.up, Vector3.zero);
#endif

        #endregion

        private void Start()
        {
            if (!entity.IsOwner)
            {
                playerColor = state.Color;
            }

            SetColor(playerColor);
        }

        // Start
        public override void Attached()
        {
            state.SetTransforms(state.Transform, transform);

            if (entity.IsOwner)
            {
                playerColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                state.Color = playerColor;

                // Deactivate the scene's main camera if it's active (and exists):
                Camera.main?.gameObject.SetActive(false);

                cameraFollow = Instantiate(cameraFollowPrefab).GetComponent<CameraFollow>();
                cameraFollow.Initialize(transform);
                MainCameraController.Singleton.SetMainCamera(cameraFollow.Camera);
            }
        }

        // Update
        public override void SimulateOwner()
        {
            Look();
        }

        private void FixedUpdate()
        {
            if (entity.IsOwner)
            {
                Move();
            }
        }

        private void Look()
        {
            if (canLook)
            {
#if UNITY_STANDALONE
                Vector3 lookAtPoint = GetMouseLookAtPoint();
                lookAtPoint.y = transform.position.y;
                
                transform.forward = lookAtPoint - transform.position;
#elif UNITY_IOS || UNITY_ANDROID
                Vector3 direction = Controllers.MobileLookController.Singleton.CanLook ? Controllers.MobileLookController.Singleton.GetLookPoint() - transform.position : transform.forward;
#endif
            }
        }

        private void Move()
        {
            if (canMove)
            {
#if UNITY_STANDALONE
                Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
#elif UNITY_IOS || UNITY_ANDROID
                Vector3 movement = Controllers.MobileMovementController.Singleton.GetMovement();
#endif

                characterController.Move(movement * moveSpeed * BoltNetwork.FrameDeltaTime);
            }
        }

        #region Helper methods

        private Vector3 GetMouseLookAtPoint()
        {
            Ray ray = cameraFollow.Camera.ScreenPointToRay(Input.mousePosition);

            // If the ray hits the plane...
            if (mouseLookAtPlane.Raycast(ray, out float distance))
            {
                // Get the hit point:
                return ray.GetPoint(distance);
            }

            return Vector3.zero;
        }

        private void SetColor(Color color)
        {
            if (materialClone == null)
            {
                materialClone = meshRenderer.material;
            }

            materialClone.color = color;
        }

        private void OnDestroy()
        {
            Destroy(materialClone);
        }

        #endregion

    }

}
