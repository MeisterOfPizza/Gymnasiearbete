using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Extensions.Components
{

    [DisallowMultipleComponent]
    class FollowAndTrack : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private Transform target;

        [Header("Values")]
        [SerializeField] private Vector3 followOffset = Vector3.zero;
        [SerializeField] private Vector3 trackOffset  = Vector3.zero;

        [Space]
        [SerializeField] private float followSpeed = 2.5f;
        [SerializeField] private float trackSpeed  = 2.5f;

        [Space]
        [SerializeField] private FollowType followType = FollowType.Lerp;
        [SerializeField] private TrackType  trackType  = TrackType.Slerp;

        #endregion

        #region Enums

        private enum FollowType
        {
            NoFollow,
            MoveTowards,
            Lerp,
            Slerp,
            Constant
        }

        private enum TrackType
        {
            NoTrack,
            Lerp,
            Slerp,
            Instant
        }

        #endregion

        private void Awake()
        {
            if (target != null)
            {
                SetPosition(target.position);
                SetLookAt(target.position);
            }
        }

        public void Initialize(Transform target)
        {
            this.target = target;

            SetPosition(target.position);
            SetLookAt(target.position);
        }

        private void FixedUpdate()
        {
            if (target != null)
            {
                switch (followType)
                {
                    case FollowType.MoveTowards:
                        transform.position = Vector3.MoveTowards(transform.position, target.position + followOffset, followSpeed * Time.deltaTime);
                        break;
                    case FollowType.Lerp:
                        transform.position = Vector3.Lerp(transform.position, target.position + followOffset, followSpeed * Time.deltaTime);
                        break;
                    case FollowType.Slerp:
                        transform.position = Vector3.Slerp(transform.position, target.position + followOffset, followSpeed * Time.deltaTime);
                        break;
                    case FollowType.Constant:
                        transform.position = target.position + followOffset;
                        break;
                    case FollowType.NoFollow:
                    default:
                        break;
                }
                
                switch (trackType)
                {
                    case TrackType.Lerp:
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.position + trackOffset - transform.position), trackSpeed * Time.deltaTime);
                        break;
                    case TrackType.Slerp:
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position + trackOffset - transform.position), trackSpeed * Time.deltaTime);
                        break;
                    case TrackType.Instant:
                        transform.LookAt(target.position + trackOffset);
                        break;
                    case TrackType.NoTrack:
                    default:
                        break;
                }
            }
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position + followOffset;
        }

        public void SetLookAt(Vector3 position)
        {
            transform.LookAt(position + trackOffset);
        }

    }

}
