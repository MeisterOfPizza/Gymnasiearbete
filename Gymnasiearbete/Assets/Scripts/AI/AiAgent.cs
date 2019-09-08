using ArenaShooter.Controllers;
using ArenaShooter.Entities;
using UnityEngine;
using UnityEngine.AI;

#pragma warning disable 0649

namespace ArenaShooter.AI
{

    [DisallowMultipleComponent]
    sealed class AiAgent : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private NavMeshAgent   agent;
        [SerializeField] private SphereCollider searchTrigger;

        #endregion

        #region Private variables

        private IEntity agentEntity;

        private EntityTeam searchTargetTeam;

        private float searchThreshold;
        private float stoppingDistance;

        private IEntity currentTarget;
        private IEntity potentialTarget;

        private Vector3 lastPositionOfTarget;

        #endregion

        public void Initialize(IEntity agentEntity, EntityTeam searchTargetTeam, float searchInterval, float searchThreshold, float stoppingDistance)
        {
            this.agentEntity      = agentEntity;
            this.searchTargetTeam = searchTargetTeam;
            this.searchThreshold  = searchThreshold;
            this.stoppingDistance = stoppingDistance;

            searchTrigger.radius = searchThreshold;

            InvokeRepeating("Search", 0f, searchInterval);
        }

        private void Search()
        {
            // Check if the potential target is the same as current target, or if a potential target does not exist.
            // If so, get the nearest target (that is not the current target).
            if (currentTarget == potentialTarget || potentialTarget == null)
            {
                potentialTarget = EntityController.Singleton.GetClosestEntity(agentEntity.BodyOriginPosition, Mathf.Infinity, currentTarget, searchTargetTeam);
            }

            // Check if the potential target is closer than the current target:
            if (currentTarget != null && potentialTarget != null && Vector3.Distance(currentTarget.BodyOriginPosition, agentEntity.BodyOriginPosition) > Vector3.Distance(potentialTarget.BodyOriginPosition, agentEntity.BodyOriginPosition))
            {
                currentTarget   = potentialTarget;
                potentialTarget = null;
            }

            // Final failsafe: if the current target is null, check if the potential target can be used. If not: get the closest target with no exceptions.
            if (currentTarget == null)
            {
                currentTarget = potentialTarget != null ? potentialTarget : EntityController.Singleton.GetClosestEntity(agentEntity.BodyOriginPosition, Mathf.Infinity, searchTargetTeam);
            }

            lastPositionOfTarget = currentTarget.BodyOriginPosition;
        }

        private void FixedUpdate()
        {
            if (currentTarget == null)
            {
                Search();
            }
            else
            {
                if (Vector3.Distance(lastPositionOfTarget, currentTarget.BodyOriginPosition) > searchThreshold)
                {
                    agent.isStopped = false;
                    agent.SetDestination(currentTarget.BodyOriginPosition);

                    lastPositionOfTarget = currentTarget.BodyOriginPosition;
                }

                if (Vector3.Distance(agentEntity.BodyOriginPosition, currentTarget.BodyOriginPosition) <= stoppingDistance)
                {
                    agent.isStopped = true;
                }
                else
                {
                    agent.isStopped = false;
                    agent.SetDestination(currentTarget.BodyOriginPosition);

                    lastPositionOfTarget = currentTarget.BodyOriginPosition;
                }

                if (agent.isStopped)
                {
                    agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, Quaternion.LookRotation(currentTarget.BodyOriginPosition - agentEntity.BodyOriginPosition), BoltNetwork.FrameDeltaTime * 250f);
                }
            }
        }

        #region OnTrigger

        private void OnTriggerEnter(Collider other)
        {
            var entity = other.GetComponent<IEntity>();

            if (entity != null && entity.EntityTeam == searchTargetTeam)
            {
                // Check if the entered entity is closer than the last potential target:
                if (potentialTarget != null && Vector3.Distance(potentialTarget.BodyOriginPosition, agentEntity.BodyOriginPosition) > Vector3.Distance(entity.BodyOriginPosition, agentEntity.BodyOriginPosition))
                {
                    potentialTarget = entity;
                }
                else if (potentialTarget == null)
                {
                    potentialTarget = entity;
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            var entity = other.GetComponent<IEntity>();

            if (entity != null && entity.EntityTeam == searchTargetTeam)
            {
                // Check if the entity that stayed is not the same as potential entity but also if it's closer than potential target:
                if (entity != potentialTarget && potentialTarget != null && Vector3.Distance(potentialTarget.BodyOriginPosition, agentEntity.BodyOriginPosition) > Vector3.Distance(entity.BodyOriginPosition, agentEntity.BodyOriginPosition))
                {
                    potentialTarget = entity;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var entity = other.GetComponent<IEntity>();

            // If the potential target was the exited entity, then remove the reference.
            if (potentialTarget == entity)
            {
                potentialTarget = null;
            }
        }

        #endregion

    }

}
