using Bolt;
using UnityEngine;

namespace ArenaShooter.Entities
{

    interface IWeaponHolder : IEntity
    {

        GameObject    gameObject         { get; }
        Vector3       WeaponFirePosition { get; }
        Vector3       WeaponForward      { get; }
        LayerMask     WeaponHitLayerMask { get; }
        GlobalTargets WeaponTargets      { get; }

    }

}
