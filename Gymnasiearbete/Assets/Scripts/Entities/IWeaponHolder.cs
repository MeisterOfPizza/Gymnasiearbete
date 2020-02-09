using ArenaShooter.Combat;
using Bolt;
using UnityEngine;

namespace ArenaShooter.Entities
{

    interface IWeaponHolder : IEntity
    {

        Vector3       WeaponFirePosition { get; }
        Vector3       WeaponForward      { get; }
        LayerMask     WeaponHitLayerMask { get; }
        GlobalTargets WeaponTargets      { get; }
        Weapon        Weapon             { get; }

    }

}
