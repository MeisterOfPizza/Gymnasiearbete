using ArenaShooter.Combat;
using ArenaShooter.Extensions;
using ArenaShooter.Extensions.Attributes;
using ArenaShooter.Templates.Enemies;
using ArenaShooter.Templates.Weapons;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    [Persistent]
    class WeaponController : Controller<WeaponController>
    {

        #region Private constants

        private const ushort PLAYER_WEAPON_TYPE_RAYCAST_DEFAULT_ID_OFFSET    = 000;
        private const ushort PLAYER_WEAPON_TYPE_PROJECTILE_DEFAULT_ID_OFFSET = 100;
        private const ushort PLAYER_WEAPON_TYPE_ELECTRIC_DEFAULT_ID_OFFSET   = 200;
        private const ushort PLAYER_WEAPON_TYPE_SUPPORT_DEFAULT_ID_OFFSET    = 300;

        private const ushort PLAYER_WEAPON_STOCK_TYPE_DEFAULT_ID_OFFSET  = 0000;
        private const ushort PLAYER_WEAPON_BODY_TYPE_DEFAULT_ID_OFFSET   = 1000;
        private const ushort PLAYER_WEAPON_BARREL_TYPE_DEFAULT_ID_OFFSET = 2000;

        #endregion

        #region Editor

        [Help(@"Player weapon part templates follow a specific ID pattern, where each output type begins with a 100 offset:
Raycast    = 000
Projectile = 100
Electric   = 200
Support    = 300

And each template part begins with a 1000 offset:
Stock  = 0000
Body   = 1000
Barrel = 2000

Which makes the final ID for any default weapon template part of any output type be calculated like this:
STOCK  = OUTPUT_TYPE + 0000
BODY   = OUTPUT_TYPE + 1000
BARREL = OUTPUT_TYPE + 2000

Which is this:
defaultTemplate = OUTPUT_TYPE + TEMPLATE_PART
")]

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

        public StockTemplate DefaultStockTemplate
        {
            get
            {
                return defaultStockTemplate;
            }
        }

        public BodyTemplate DefaultBodyTemplate
        {
            get
            {
                return defaultBodyTemplate;
            }
        }

        public BarrelTemplate DefaultBarrelTemplate
        {
            get
            {
                return defaultBarrelTemplate;
            }
        }

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
            SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
            {
                ClearContainers();
            };
        }

        #region Player WeaponPartTemplates

        public Weapon CreateWeapon(WeaponPartItem<StockTemplate> stockPartItem, WeaponPartItem<BodyTemplate> bodyPartItem, WeaponPartItem<BarrelTemplate> barrelPartItem, Transform parent)
        {
            if (stockPartItem.Template.OutputType == bodyPartItem.Template.OutputType && bodyPartItem.Template.OutputType == barrelPartItem.Template.OutputType)
            {
                GameObject weaponGameObject = new GameObject("Weapon");
                weaponGameObject.transform.SetParent(parent);
                Weapon weapon = null;

                switch (stockPartItem.Template.OutputType)
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

                weapon.Initialize(new PlayerWeaponStats(stockPartItem, bodyPartItem, barrelPartItem));
                return weapon;
            }
            else
            {
                Debug.LogWarning("Weapon could not be built with the three given part templates.");
                return null;
            }
        }

        public Weapon CreateBystanderWeapon(StockTemplate stockTemplate, BodyTemplate bodyTemplate, BarrelTemplate barrelTemplate, Transform parent)
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

                weapon.Initialize(new BystanderWeaponStats(stockTemplate, bodyTemplate, barrelTemplate));
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

        public Tuple<StockTemplate, BodyTemplate, BarrelTemplate> GetDefaultTemplatesOfType(WeaponOutputType weaponOutputType)
        {
            switch (weaponOutputType)
            {
                case WeaponOutputType.Raycasting:
                    return new Tuple<StockTemplate, BodyTemplate, BarrelTemplate>(
                        GetStockTemplate(PLAYER_WEAPON_TYPE_RAYCAST_DEFAULT_ID_OFFSET  + PLAYER_WEAPON_STOCK_TYPE_DEFAULT_ID_OFFSET),
                        GetBodyTemplate(PLAYER_WEAPON_TYPE_RAYCAST_DEFAULT_ID_OFFSET   + PLAYER_WEAPON_BODY_TYPE_DEFAULT_ID_OFFSET),
                        GetBarrelTemplate(PLAYER_WEAPON_TYPE_RAYCAST_DEFAULT_ID_OFFSET + PLAYER_WEAPON_BARREL_TYPE_DEFAULT_ID_OFFSET));
                case WeaponOutputType.Projectile:
                    return new Tuple<StockTemplate, BodyTemplate, BarrelTemplate>(
                        GetStockTemplate(PLAYER_WEAPON_TYPE_PROJECTILE_DEFAULT_ID_OFFSET  + PLAYER_WEAPON_STOCK_TYPE_DEFAULT_ID_OFFSET),
                        GetBodyTemplate(PLAYER_WEAPON_TYPE_PROJECTILE_DEFAULT_ID_OFFSET   + PLAYER_WEAPON_BODY_TYPE_DEFAULT_ID_OFFSET),
                        GetBarrelTemplate(PLAYER_WEAPON_TYPE_PROJECTILE_DEFAULT_ID_OFFSET + PLAYER_WEAPON_BARREL_TYPE_DEFAULT_ID_OFFSET));
                case WeaponOutputType.Electric:
                    return new Tuple<StockTemplate, BodyTemplate, BarrelTemplate>(
                        GetStockTemplate(PLAYER_WEAPON_TYPE_ELECTRIC_DEFAULT_ID_OFFSET  + PLAYER_WEAPON_STOCK_TYPE_DEFAULT_ID_OFFSET),
                        GetBodyTemplate(PLAYER_WEAPON_TYPE_ELECTRIC_DEFAULT_ID_OFFSET   + PLAYER_WEAPON_BODY_TYPE_DEFAULT_ID_OFFSET),
                        GetBarrelTemplate(PLAYER_WEAPON_TYPE_ELECTRIC_DEFAULT_ID_OFFSET + PLAYER_WEAPON_BARREL_TYPE_DEFAULT_ID_OFFSET));
                case WeaponOutputType.Support:
                    return new Tuple<StockTemplate, BodyTemplate, BarrelTemplate>(
                        GetStockTemplate(PLAYER_WEAPON_TYPE_SUPPORT_DEFAULT_ID_OFFSET  + PLAYER_WEAPON_STOCK_TYPE_DEFAULT_ID_OFFSET),
                        GetBodyTemplate(PLAYER_WEAPON_TYPE_SUPPORT_DEFAULT_ID_OFFSET   + PLAYER_WEAPON_BODY_TYPE_DEFAULT_ID_OFFSET),
                        GetBarrelTemplate(PLAYER_WEAPON_TYPE_SUPPORT_DEFAULT_ID_OFFSET + PLAYER_WEAPON_BARREL_TYPE_DEFAULT_ID_OFFSET));
                default:
                    return null;
            }
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

            weapon.Initialize(new EnemyWeaponStats(enemyWeaponTemplate));
            return weapon;
        }

        public EnemyWeaponTemplate GetEnemyWeaponTemplate(ushort id)
        {
            return enemyWeaponTemplates.FirstOrDefault(t => t.TemplateId == id);
        }

        #endregion

        #region Helpers

        public void ClearContainers()
        {
            projectileContainer.Clear();
        }

        #endregion

    }

}
