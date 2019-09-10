using System;
using UnityEngine;

namespace ArenaShooter.Entities
{

    enum EntityTeam : byte
    {
        Player,
        Enemy
    }

    interface IEntity : IDamagable, IHealable
    {

        Action OnDeathCallback   { get; set; }
        Action OnReviveCallback  { get; set; }
        Action OnDestroyCallback { get; set; }

        EntityTeam EntityTeam         { get; }
        Vector3    BodyOriginPosition { get; }
        BoltEntity entity             { get; set; }

    }

}
