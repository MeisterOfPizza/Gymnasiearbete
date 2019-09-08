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
        [SerializeField] private int   health       = 100;
        [SerializeField] private float movmentSpeed = 2f;

        [Header("References")]
        [SerializeField] private EnemyWeaponTemplate[] possibleWeaponTemplates;

        [Header("Prefabs")]
        [SerializeField] private GameObject enemyPrefab;

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
                return movmentSpeed;
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
