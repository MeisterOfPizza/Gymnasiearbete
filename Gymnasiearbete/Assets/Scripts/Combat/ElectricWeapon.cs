using UnityEngine;

namespace ArenaShooter.Combat
{

    class ElectricWeapon : Weapon
    {

        #region Editor

        [Header("References")]
        [SerializeField] private LineRenderer lineRenderer; // TEST: Test data.

        #endregion

        protected override void OnFire()
        {

        }

    }

}
