using ArenaShooter.Combat;
using ArenaShooter.Templates.Enemies;
using ArenaShooter.Templates.Weapons;
using System.Linq;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class WeaponController : Controller<WeaponController>
    {

        #region Editor

        [Header("Player Weapon Part Templates")]
        [SerializeField] private StockTemplate   defaultStockTemplate;
        [SerializeField] private StockTemplate[] stockTemplates;

        [Space]
        [SerializeField] private BodyTemplate   defaultBodyTemplate;
        [SerializeField] private BodyTemplate[] bodyTemplates;

        [Space]
        [SerializeField] private BarrelTemplate   defaultBarrelTemplate;
        [SerializeField] private BarrelTemplate[] barrelTemplates;

        [Header("Enemy Weapon Templates")]
        [SerializeField] private EnemyWeaponTemplate[] enemyWeaponTemplates;

        [Header("References")]
        [SerializeField] private Transform projectileContainer;

        #endregion

        #region Public properties

        public Transform ProjectileContainer
        {
            get
            {
                return projectileContainer;
            }
        }

        #endregion

        protected override void OnAwake()
        {
            DontDestroyOnLoad(gameObject);
        }

        #region Player WeaponPartTemplates

        public Weapon CreateWeapon(StockTemplate stockTemplate, BodyTemplate bodyTemplate, BarrelTemplate barrelTemplate, Transform parent)
        {
            if (stockTemplate.OutputType == bodyTemplate.OutputType && bodyTemplate.OutputType == barrelTemplate.OutputType)
            {
                GameObject weaponGameObject = new GameObject("Weapon");
                weaponGameObject.transform.SetParent(parent);
                Weapon weapon = null;

                switch (stockTemplate.OutputType)
                {
                    case WeaponOutputType.Raycasting:
                        weapon = weaponGameObject.AddComponent<RaycastWeapon>();
                        break;
                    case WeaponOutputType.Projectile:
                        weapon = weaponGameObject.AddComponent<ProjectileWeapon>();
                        break;
                    case WeaponOutputType.Electric:
                        weapon = weaponGameObject.AddComponent<ElectricWeapon>();
                        break;
                    case WeaponOutputType.Support:
                        weapon = weaponGameObject.AddComponent<SupportWeapon>();
                        break;
                    default:
                        Debug.LogWarning("Weapon could not be built with the three given part templates.");
                        return null;
                }

                weapon.Initialize(new WeaponStats(stockTemplate, bodyTemplate, barrelTemplate));
                return weapon;
            }
            else
            {
                Debug.LogWarning("Weapon could not be built with the three given part templates.");
                return null;
            }
        }

        public StockTemplate GetStockTemplate(ushort id)
        {
            var template = stockTemplates.FirstOrDefault(t => t.TemplateId == id);

            return template ?? defaultStockTemplate;
        }

        public BodyTemplate GetBodyTemplate(ushort id)
        {
            var template = bodyTemplates.FirstOrDefault(t => t.TemplateId == id);

            return template ?? defaultBodyTemplate;
        }

        public BarrelTemplate GetBarrelTemplate(ushort id)
        {
            var template = barrelTemplates.FirstOrDefault(t => t.TemplateId == id);

            return template ?? defaultBarrelTemplate;
        }

        #endregion

        #region EnemyWeaponTemplates

        public Weapon CreateWeapon(EnemyWeaponTemplate enemyWeaponTemplate, Transform parent)
        {
            GameObject weaponGameObject = new GameObject("Weapon");
            weaponGameObject.transform.SetParent(parent, false);
            Weapon weapon = null;

            switch (enemyWeaponTemplate.OutputType)
            {
                case WeaponOutputType.Raycasting:
                    weapon = weaponGameObject.AddComponent<RaycastWeapon>();
                    break;
                case WeaponOutputType.Projectile:
                    weapon = weaponGameObject.AddComponent<ProjectileWeapon>();
                    break;
                case WeaponOutputType.Electric:
                    weapon = weaponGameObject.AddComponent<ElectricWeapon>();
                    break;
                case WeaponOutputType.Support:
                    weapon = weaponGameObject.AddComponent<SupportWeapon>();
                    break;
                default:
                    Debug.LogWarning("Weapon could not be built with the given enemy weapon template.");
                    return null;
            }

            weapon.Initialize(new WeaponStats(enemyWeaponTemplate));
            return weapon;
        }

        public EnemyWeaponTemplate GetEnemyWeaponTemplate(ushort id)
        {
            return enemyWeaponTemplates.FirstOrDefault(t => t.TemplateId == id);
        }

        #endregion

    }

}
