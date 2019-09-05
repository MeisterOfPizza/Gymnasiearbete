using UnityEngine;

namespace ArenaShooter.Entities
{

    enum EntityTeam
    {
        Player,
        Enemy
    }

    interface IEntity : IDamagable, IHealable
    {

        EntityTeam EntityTeam { get; }

        BoltEntity entity { get; set; }

        Vector3 BodyOriginPosition { get; }

    }

}
