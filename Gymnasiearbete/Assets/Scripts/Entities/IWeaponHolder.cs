using Bolt;
using UnityEngine;

namespace ArenaShooter.Entities
{

    interface IWeaponHolder
    {

        GameObject    gameObject         { get; }
        BoltEntity    entity             { get; set; }
        Vector3       WeaponFirePosition { get; }
        Vector3       WeaponForward      { get; }
        LayerMask     WeaponHitLayerMask { get; }
        GlobalTargets WeaponTargets      { get; }

    }

}
