using ArenaShooter.Entities;
using UnityEngine;

namespace ArenaShooter.Controllers
{

    class EntitySpawnController : ServerController<EntitySpawnController>
    {

        public EntityType SpawnEntityOnServer<EntityType>(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent) where EntityType : MonoBehaviour, IEntity
        {
            BoltEntity boltEntity = BoltNetwork.Instantiate(prefab, position, rotation);
            boltEntity.transform.SetParent(parent);

            return boltEntity.GetComponent<EntityType>();
        }

    }

}
