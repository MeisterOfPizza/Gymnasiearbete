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

        private IAIAgentBehaviour agentEntity;

        private IEntity currentTarget;
        private IEntity potentialTarget;

        private Vector3 lastPositionOfTarget;

        private bool isInitialized;

        #endregion

        #region Initializing

        public void Initialize(IAIAgentBehaviour agentBehaviourEntity)
        {
            this.agentEntity = agentBehaviourEntity;

            agent.speed          = agentBehaviourEntity.MovementSpeed;
            searchTrigger.radius = agentBehaviourEntity.SearchThreshold;

            InvokeRepeating("Search", 0f, agentBehaviourEntity.SearchInterval);

            isInitialized = true;
        }

        #endregion

        #region AI

        private void Search()
        {
            if (!agentEntity.IsNull())
            {
                // Check if the potential target is the same as current target, or if a potential target does not exist.
                // If so, get the nearest target (that is not the current target).
                if (currentTarget.IsSame(potentialTarget) || potentialTarget.IsNull())
                {
                    potentialTarget = GetClosestTarget();
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
                    SetCurrentTarget(!potentialTarget.IsNull() ? potentialTarget : GetClosestTarget());
                }

                lastPositionOfTarget = !currentTarget.IsNull() ? currentTarget.BodyOriginPosition : lastPositionOfTarget;
            }
        }

        private void FixedUpdate()
        {
            if (!currentTarget.IsNull())
            {
                // Check if the player is outside the search area or if it can't turn to the target:
                if (Vector3.Distance(lastPositionOfTarget, currentTarget.BodyOriginPosition) > agentEntity.SearchThreshold || !CanTurnToTarget())
                {
                    agent.isStopped = false;
                    agent.SetDestination(currentTarget.BodyOriginPosition);
                    
                    agentEntity.Body.SetLowerBodyAsController(currentTarget.BodyOriginPosition);

                    lastPositionOfTarget = currentTarget.BodyOriginPosition;
                }

                // Check if the AI is too close and it can turn to the target, which should result in a stop:
                if (Vector3.Distance(agentEntity.BodyOriginPosition, currentTarget.BodyOriginPosition) <= agentEntity.StoppingDistance && CanTurnToTarget())
                {
                    agent.isStopped = true;

                    agentEntity.Body.SetUpperBodyAsController(currentTarget.BodyOriginPosition);
                }

                if (agent.isStopped)
                {
                    // Avoid for zero point rotation warning:
                    if (currentTarget.BodyOriginPosition - agentEntity.BodyOriginPosition != Vector3.zero)
                    {
                        agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, Quaternion.LookRotation(currentTarget.BodyOriginPosition - agentEntity.BodyOriginPosition), agentEntity.TurnSpeed);
                    }

                    // Can the AI see the target? If so, turn the upper body:
                    if (CanSeeTarget())
                    {
                        agentEntity.Body.SetUpperBodyTarget(currentTarget.BodyOriginPosition);
                    }
                }
                else
                {
                    agentEntity.Body.SetLowerBodyTarget(agentEntity.BodyOriginPosition + agent.velocity);

                    if (CanSeeTarget())
                    {
                        agentEntity.Body.SetUpperBodyTarget(currentTarget.BodyOriginPosition);
                    }
                    else
                    {
                        agentEntity.Body.SetUpperBodyTarget(agentEntity.BodyOriginPosition + agent.transform.forward);
                    }
                }
            }
        }

        #endregion

        #region Setting and removing targets

        private void SetCurrentTarget(IEntity target)
        {
            // Check if the agent still exists AND is placed on a NavMesh.
            // This is done because when exiting the match as a host, the current target will
            // invoke a OnDestroy callback, which calls this method (SetCurrentTarget) which will
            // result in several null reference errors.
            if (agent != null && agent.isOnNavMesh)
            {
                if (!currentTarget.IsNull())
                {
                    currentTarget.OnDeathCallback -= RemoveCurrentTarget;
                    currentTarget.OnDestroyCallback -= RemoveCurrentTarget;
                }

                potentialTarget = null;
                currentTarget = target;

                if (!currentTarget.IsNull())
                {
                    currentTarget.OnDeathCallback += RemoveCurrentTarget;
                    currentTarget.OnDestroyCallback += RemoveCurrentTarget;

                    if (agent.gameObject.activeInHierarchy)
                    {
                        agent.SetDestination(currentTarget.BodyOriginPosition);
                    }
                }
            }
        }

        private void RemoveCurrentTarget()
        {
            SetCurrentTarget(null);
            Search();
        }

        #endregion

        #region OnTrigger

        private void OnTriggerEnter(Collider other)
        {
            if (isInitialized)
            {
                var entity = other.GetComponent<IEntity>();

                if (!entity.IsSame(agentEntity) && entity.EntityTeam == agentEntity.SearchTargetTeam)
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
        }

        private void OnTriggerStay(Collider other)
        {
            if (isInitialized)
            {
                var entity = other.GetComponent<IEntity>();

                if (!entity.IsSame(agentEntity) && entity.EntityTeam == agentEntity.SearchTargetTeam)
                {
                    // Check if the entity that stayed is not the same as potential entity but also if it's closer than potential target:
                    if (!entity.IsSame(potentialTarget) && !potentialTarget.IsNull() && Vector3.Distance(potentialTarget.BodyOriginPosition, agentEntity.BodyOriginPosition) > Vector3.Distance(entity.BodyOriginPosition, agentEntity.BodyOriginPosition))
                    {
                        potentialTarget = entity;
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (isInitialized)
            {
                var entity = other.GetComponent<IEntity>();

                // If the potential target was the exited entity, then remove the reference.
                if (potentialTarget.IsSame(entity))
                {
                    potentialTarget = null;
                }
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Returns the closest target in regards to pathing distance.
        /// </summary>
        private IEntity GetClosestTarget()
        {
            IEntity selectedEntity  = null;
            float   minPathDistance = float.MaxValue;

            var allTargets = EntityController.Singleton.GetEntitiesOfTeam(agentEntity.SearchTargetTeam);

            // Check if there's only one target, to save computation power.
            if (allTargets.Length == 1 && allTargets[0] != agentEntity)
            {
                return allTargets[0];
            }

            // Loop through all potential targets:
            foreach (var target in allTargets)
            {
                if (target != agentEntity)
                {
                    NavMeshPath path = new NavMeshPath();

                    // Calculate the path that the agent needs to take:
                    if (NavMesh.CalculatePath(agentEntity.BodyOriginPosition, target.BodyOriginPosition, agent.areaMask, path))
                    {
                        Vector3[] corners      = path.corners; // Get all the corners.
                        Vector3   lastCorner   = target.BodyOriginPosition; // Set the first last corner as the target position.
                        float     pathDistance = 0f;

                        // Total the path travel distance:
                        for (int i = 0; i < corners.Length; i++)
                        {
                            pathDistance += Vector3.Distance(lastCorner, corners[i]);
                            lastCorner    = corners[i];
                        }

                        // If the new path distance is shorter than the selected one, update the references and values:
                        if (pathDistance < minPathDistance)
                        {
                            selectedEntity  = target;
                            minPathDistance = pathDistance;
                        }
                    }
                }
            }

            return selectedEntity;
        }

        /// <summary>
        /// Can the AI turn towards the target with the current body restrictions?
        /// </summary>
        private bool CanSeeTarget()
        {
            if (!currentTarget.IsNull())
            {
                Ray ray = new Ray(agentEntity.BodyOriginPosition, agentEntity.Body.RestrictNormal(currentTarget.BodyOriginPosition - agentEntity.BodyOriginPosition));
                var hit = Utils.Raycast(ray, agentEntity.SearchThreshold, Physics.AllLayers, agentEntity.gameObject, QueryTriggerInteraction.Ignore);
                
                return hit.NetworkHit && hit.Body.gameObject == currentTarget.gameObject;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if the AI can turn to the target, no restrictions.
        /// Shoots a ray from the AI's position to the target. If the ray
        /// hits anything other than the target, it returns false.
        /// </summary>
        private bool CanTurnToTarget()
        {
            if (!currentTarget.IsNull())
            {
                Ray ray = new Ray(agentEntity.BodyOriginPosition, currentTarget.BodyOriginPosition - agentEntity.BodyOriginPosition);
                var hit = Utils.Raycast(ray, agentEntity.SearchThreshold, Physics.AllLayers, agentEntity.gameObject, QueryTriggerInteraction.Ignore);
                
                return hit.NetworkHit && hit.Body.gameObject == currentTarget.gameObject;
            }
            else
            {
                return false;
            }
        }

        #endregion

    }

}
