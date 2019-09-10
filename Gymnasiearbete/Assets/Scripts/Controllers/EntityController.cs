using ArenaShooter.Entities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArenaShooter.Controllers
{

    class EntityController : Controller<EntityController>
    {

        #region Private variables

        private HashSet<IEntity> entities = new HashSet<IEntity>();

        #endregion

        public void AddEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            entities.Remove(entity);
        }

        public IEntity FindEntityWithBoltEntity(BoltEntity boltEntity)
        {
            return entities.FirstOrDefault(e => e.entity.NetworkId.Equals(boltEntity.NetworkId));
        }

        public IEntity GetClosestEntity(Vector3 position, float maxDistance, IEntity exception, EntityTeam entityTeam)
        {
            var selectedEntities = entities.Where(e => e != exception && e.EntityTeam == entityTeam).ToList();

            if (selectedEntities.Count > 1)
            {
                IEntity entity = null;
                float distance = float.MaxValue;

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
            var selectedEntities = entities.Where(e => !exceptions.Contains(e) && e.EntityTeam == entityTeam).ToList();

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
