using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArenaShooter.Templates.Enemies
{
    enum AttackType
    {
        Raycast,
        Projectile,
        Electric,
        Support,
        Melee
    }
    [CreateAssetMenu(menuName = "Templates/Enemies/Enemy")]
    class EnemyTemplate : ScriptableObject
    {
        #region inputs
        [Header("Inputs")]
        [SerializeField] private ushort id;
        [SerializeField] private string nameEnemy;
        [SerializeField] private GameObject prefab;
        [SerializeField] private short health;
        [SerializeField] private float movmentSpeed;
        [SerializeField] private float engageRange;
        #endregion

        #region getters
        public ushort Id
        {
            get
            {
                return id;
            }
        }

        public string NameEnemy
        {
            get
            {
                return nameEnemy;
            }
        }

        public GameObject Prefab
        {
            get
            {
                return prefab;
            }
        }

        public short Health
        {
            get
            {
                return health;
            }
        }

        public float MovementSpeed
        {
            get
            {
                return movmentSpeed;
            }
        }

        public float EngageRange
        {
            get
            {
                return engageRange;
            }
        }
        #endregion
    }
}
    


