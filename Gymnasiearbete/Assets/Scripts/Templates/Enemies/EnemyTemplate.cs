using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Templates.Enemies
{

    [CreateAssetMenu(menuName = "Templates/Enemies/Enemy")]
    sealed class EnemyTemplate : ScriptableObject
    {

        #region Editor

        [Header("Values")]
        [SerializeField] private     ushort templateId;
        [SerializeField] private new string name = "Enemy";

        [Header("Stats")]
        [SerializeField] private int   health        = 100;
        [SerializeField] private float movementSpeed = 3.5f;

        [Header("Logic")]
        [SerializeField] private TargetSearchFrequencyType targetSearchFrequency = TargetSearchFrequencyType.Normal;

        [Space]
        [SerializeField] private bool  instantTurn = false;
        [SerializeField] private float turnSpeed   = 10f;

        [Header("References")]
        [SerializeField] private EnemyWeaponTemplate[] possibleWeaponTemplates;

        [Header("Prefabs")]
        [SerializeField] private GameObject enemyPrefab;

        #endregion

        #region Enums

        private enum TargetSearchFrequencyType : byte
        {
            Slow   = 5,
            Normal = 3,
            Fast   = 1
        }

        #endregion

        #region Getters

        public ushort TemplateId
        {
            get
            {
                return templateId;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public int Health
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
                return movementSpeed;
            }
        }

        public float TargetSearchFrequency
        {
            get
            {
                return (float)targetSearchFrequency;
            }
        }

        public float TurnSpeed
        {
            get
            {
                return instantTurn ? float.MaxValue : turnSpeed;
            }
        }

        public GameObject EnemyPrefab
        {
            get
            {
                return enemyPrefab;
            }
        }

        #endregion

        #region Helper methods

        public EnemyWeaponTemplate GetEnemyWeaponTemplate()
        {
            return possibleWeaponTemplates[Random.Range(0, possibleWeaponTemplates.Length)];
        }

        private void OnValidate()
        {
            if (possibleWeaponTemplates.Length == 0 || (possibleWeaponTemplates.Length == 1 && possibleWeaponTemplates[0] == null))
            {
                Debug.LogError(string.Format(@"EnemyTemplate ""{0}"" (custom name ""{1}"") does not have a valid EnemyWeaponTemplate. This will cause errors during runtime.", base.name, this.name));
            }
        }

        #endregion

    }

}
