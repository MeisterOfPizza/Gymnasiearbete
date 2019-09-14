using ArenaShooter.Controllers;
using ArenaShooter.Entities;
using ArenaShooter.Extensions;
using UnityEngine;
using UnityEngine.AI;

#pragma warning disable 0649

namespace ArenaShooter.AI
{

    [DisallowMultipleComponent]
    sealed class AIAgent : MonoBehaviour
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
        private float movementSpeed;
        private float turnSpeed;

        private HumanoidBody humanoidBody;

        private IEntity currentTarget;
        private IEntity potentialTarget;

        private Vector3 lastPositionOfTarget;

        #endregion

        public void Initialize(IEntity agentEntity, EntityTeam searchTargetTeam, float searchInterval, float searchThreshold, float stoppingDistance, float movementSpeed, float turnSpeed, HumanoidBody humanoidBody)
        {
            this.agentEntity      = agentEntity;
            this.searchTargetTeam = searchTargetTeam;
            this.searchThreshold  = searchThreshold;
            this.stoppingDistance = stoppingDistance;
            this.movementSpeed    = movementSpeed;
            this.turnSpeed        = turnSpeed;
            this.humanoidBody     = humanoidBody;

            agent.speed          = movementSpeed;
            searchTrigger.radius = searchThreshold;

            InvokeRepeating("Search", 0f, searchInterval);
        }

        private void Search()
        {
            if (!agentEntity.IsNull())
            {
                // Check if the potential target is the same as current target, or if a potential target does not exist.
                // If so, get the nearest target (that is not the current target).
                if (currentTarget.IsSame(potentialTarget) || potentialTarget.IsNull())
                {
                    potentialTarget = EntityController.Singleton.GetClosestEntity(agentEntity.BodyOriginPosition, Mathf.Infinity, currentTarget, searchTargetTeam);
                }

                // Check if the potential target is closer than the current target:
                if (!currentTarget.IsNull() && !potentialTarget.IsNull() && Vector3.Distance(currentTarget.BodyOriginPosition, agentEntity.BodyOriginPosition) > Vector3.Distance(potentialTarget.BodyOriginPosition, agentEntity.BodyOriginPosition))
                {
                    SetCurrentTarget(potentialTarget);
                    potentialTarget = null;
                }

                // Final failsafe: if the current target is null, check if the potential target can be used. If not: get the closest target with no exceptions.
                if (currentTarget.IsNull())
                {
                    SetCurrentTarget(!potentialTarget.IsNull() ? potentialTarget : EntityController.Singleton.GetClosestEntity(agentEntity.BodyOriginPosition, Mathf.Infinity, searchTargetTeam));
                }

                lastPositionOfTarget = !currentTarget.IsNull() ? currentTarget.BodyOriginPosition : lastPositionOfTarget;
            }
        }

        private void FixedUpdate()
        {
            if (!currentTarget.IsNull())
            {
                // Check if the player is outside the search area:
                if (Vector3.Distance(lastPositionOfTarget, currentTarget.BodyOriginPosition) > searchThreshold)
                {
                    agent.isStopped = false;
                    agent.SetDestination(currentTarget.BodyOriginPosition);
                    
                    humanoidBody.SetLowerBodyAsController(currentTarget.BodyOriginPosition);

                    lastPositionOfTarget = currentTarget.BodyOriginPosition;
                }

                // Check if the AI is too close and should stop:
                if (Vector3.Distance(agentEntity.BodyOriginPosition, currentTarget.BodyOriginPosition) <= stoppingDistance)
                {
                    agent.isStopped = true;
                    
                    humanoidBody.SetUpperBodyAsController(currentTarget.BodyOriginPosition);
                }

                if (agent.isStopped)
                {
                    agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, Quaternion.LookRotation(currentTarget.BodyOriginPosition - agentEntity.BodyOriginPosition), turnSpeed);
                }
                else
                {
                    humanoidBody.SetLowerBodyTarget(agentEntity.BodyOriginPosition + agent.velocity);
                }

                humanoidBody.SetUpperBodyTarget(currentTarget.BodyOriginPosition);
            }
        }

        private void SetCurrentTarget(IEntity target)
        {
            if (!currentTarget.IsNull())
            {
                currentTarget.OnDeathCallback   -= RemoveCurrentTarget;
                currentTarget.OnDestroyCallback -= RemoveCurrentTarget;
            }

            potentialTarget = null;
            currentTarget   = target;

            if (!currentTarget.IsNull())
            {
                currentTarget.OnDeathCallback   += RemoveCurrentTarget;
                currentTarget.OnDestroyCallback += RemoveCurrentTarget;

                agent.SetDestination(currentTarget.BodyOriginPosition);
            }
        }

        private void RemoveCurrentTarget()
        {
            SetCurrentTarget(null);
            Search();
        }

        #region OnTrigger

        private void OnTriggerEnter(Collider other)
        {
            var entity = other.GetComponent<IEntity>();

            if (!entity.IsNull() && entity.EntityTeam == searchTargetTeam)
            {
                // Check if the entered entity is closer than the last potential target:
                if (!potentialTarget.IsNull() && Vector3.Distance(potentialTarget.BodyOriginPosition, agentEntity.BodyOriginPosition) > Vector3.Distance(entity.BodyOriginPosition, agentEntity.BodyOriginPosition))
                {
                    potentialTarget = entity;
                }
                else if (potentialTarget.IsNull())
                {
                    potentialTarget = entity;
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            var entity = other.GetComponent<IEntity>();

            if (!entity.IsNull() && entity.EntityTeam == searchTargetTeam)
            {
                // Check if the entity that stayed is not the same as potential entity but also if it's closer than potential target:
                if (!entity.IsSame(potentialTarget) && !potentialTarget.IsNull() && Vector3.Distance(potentialTarget.BodyOriginPosition, agentEntity.BodyOriginPosition) > Vector3.Distance(entity.BodyOriginPosition, agentEntity.BodyOriginPosition))
                {
                    potentialTarget = entity;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var entity = other.GetComponent<IEntity>();

            // If the potential target was the exited entity, then remove the reference.
            if (potentialTarget.IsSame(entity))
            {
                potentialTarget = null;
            }
        }

        #endregion

    }

}
