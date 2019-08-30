using ArenaShooter.Entities;
using UnityEngine;

namespace ArenaShooter.Controllers
{

    class EntitySpawnController : ServerController<EntitySpawnController>
    {

        public IEntity SpawnEntityOnServer<EntityType>(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation) where EntityType : IEntity
        {
            BoltEntity boltEntity = BoltNetwork.Instantiate(prefab, position, rotation);
            boltEntity.transform.SetParent(parent);

            return boltEntity.GetComponent<IEntity>();
        }

    }

}
