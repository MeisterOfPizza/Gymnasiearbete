using ArenaShooter.Extensions.Components;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Player
{

    /*
    [RequireComponent(typeof(CharacterController))]
    class PlayerMovementController : NetworkBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private MeshRenderer        meshRenderer;

        [Header("Prefabs")]
        [SerializeField] private GameObject cameraFollowPrefab;

        [Header("Values")]
        [SerializeField] private float moveSpeed             = 300f;
        [SerializeField] private float maxTurnSpeed          = 90f;
        [SerializeField] private float turnSpeedAcceleration = 30f;
        [SerializeField] private float turnSpeedDeceleration = 30f;

        #endregion

        #region Private variables

        private CameraFollow cameraFollow;

        [SyncVar(hook = nameof(SetColor))]
        private Color playerColor = Color.black;

        private Material materialClone;

        private float horizontal = 0f;
        private float vertical   = 0f;
        private float turn       = 0f;

        #endregion

        public override void OnStartServer()
        {
            base.OnStartServer();

            playerColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            // Deactivate the scene's main camera if it's active (and exists):
            Camera.main?.gameObject.SetActive(false);

            cameraFollow = Instantiate(cameraFollowPrefab).GetComponent<CameraFollow>();
            cameraFollow.Initialize(transform);
        }

        private void Update()
        {
            if (isLocalPlayer)
            {
                horizontal = Input.GetAxis("Horizontal");
                vertical = Input.GetAxis("Vertical");

                if (Input.GetKey(KeyCode.Q) && (turn > -maxTurnSpeed))
                {
                    turn -= turnSpeedAcceleration;
                }
                else if (Input.GetKey(KeyCode.E) && (turn < maxTurnSpeed))
                {
                    turn += turnSpeedAcceleration;
                }
                else
                {
                    if (turn > turnSpeedDeceleration)
                    {
                        turn -= turnSpeedDeceleration;
                    }
                    else if (turn < -turnSpeedDeceleration)
                    {
                        turn += turnSpeedDeceleration;
                    }
                    else
                    {
                        turn = 0f;
                    }
                }

                if (characterController != null)
                {
                    transform.Rotate(0f, turn * Time.deltaTime, 0f);

                    Vector3 direction = new Vector3(horizontal, 0f, vertical) * moveSpeed;
                    direction = transform.TransformDirection(direction);
                    characterController.SimpleMove(direction * Time.deltaTime);
                }
            }
        }

        #region Helper methods

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
    */

}
