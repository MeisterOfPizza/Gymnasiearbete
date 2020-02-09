using ArenaShooter.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArenaShooter.Controllers
{

    class EntityController : Controller<EntityController>
    {

        #region Private variables

        private HashSet<IEntity> entities = new HashSet<IEntity>();

        private Dictionary<EntityTeam, HashSet<IEntity>> entityTeams = new Dictionary<EntityTeam, HashSet<IEntity>>();

        #endregion

        protected override void OnAwake()
        {
            var entityTeams = Enum.GetValues(typeof(EntityTeam)).Cast<EntityTeam>();

            foreach (var team in entityTeams)
            {
                this.entityTeams.Add(team, new HashSet<IEntity>());
            }
        }

        public void AddEntity(IEntity entity)
        {
            entities.Add(entity);
            entityTeams[entity.EntityTeam].Add(entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            entities.Remove(entity);
            entityTeams[entity.EntityTeam].Remove(entity);
        }

        public IEnumerable<IEntity> GetEntitiesOfTeam(EntityTeam entityTeam)
        {
            return entityTeams[entityTeam];
        }

        public IEntity FindEntityWithBoltEntity(BoltEntity boltEntity)
        {
            return entities.FirstOrDefault(e => e.entity.NetworkId.Equals(boltEntity.NetworkId));
        }

        public IEntity GetClosestEntity(Vector3 position, float maxDistance, EntityTeam entityTeam)
        {
            var selectedEntities = entities.Where(e => e.EntityTeam == entityTeam).ToList();

            if (selectedEntities.Count > 1)
            {
                IEntity entity   = null;
                float   distance = float.MaxValue;

                foreach (var selectedEntity in selectedEntities)
                {
                    float cDist = Vector3.Distance(selectedEntity.BodyOriginPosition, position);

                    if (cDist < distance && cDist <= maxDistance)
                    {
                        entity = selectedEntity;
                        distance = cDist;
                    }
                }

                return entity;
            }

            if (selectedEntities.Count == 1 && Vector3.Distance(position, selectedEntities[0].BodyOriginPosition) <= maxDistance)
            {
                return selectedEntities[0];
            }
            else
            {
                return null;
            }
        }

        public IEntity GetClosestEntity(Vector3 position, float maxDistance, IEntity exception, EntityTeam entityTeam)
        {
            var selectedEntities = entityTeams[entityTeam].Where(e => e != exception).ToList();

            if (selectedEntities.Count > 1)
            {
                IEntity entity   = null;
                float   distance = float.MaxValue;

                foreach (var selectedEntity in selectedEntities)
                {
                    float cDist = Vector3.Distance(selectedEntity.BodyOriginPosition, position);

                    if (cDist < distance && cDist <= maxDistance)
                    {
                        entity = selectedEntity;
                        distance = cDist;
                    }
                }

                return entity;
            }

            if (selectedEntities.Count == 1 && Vector3.Distance(position, selectedEntities[0].BodyOriginPosition) <= maxDistance)
            {
                return selectedEntities[0];
            }
            else
            {
                return null;
            }
        }

        public IEntity GetClosestEntity(Vector3 position, float maxDistance, IList<IEntity> exceptions, EntityTeam entityTeam)
        {
            var selectedEntities = entityTeams[entityTeam].Where(e => !exceptions.Contains(e)).ToList();

            if (selectedEntities.Count > 1)
            {
                IEntity entity   = null;
                float   distance = float.MaxValue;

                foreach (var selectedEntity in selectedEntities)
                {
                    float cDist = Vector3.Distance(selectedEntity.BodyOriginPosition, position);

                    if (cDist < distance && cDist <= maxDistance)
                    {
                        entity   = selectedEntity;
                        distance = cDist;
                    }
                }

                return entity;
            }

            if (selectedEntities.Count == 1 && Vector3.Distance(position, selectedEntities[0].BodyOriginPosition) <= maxDistance)
            {
                return selectedEntities[0];
            }
            else
            {
                return null;
            }
        }

    }

}
