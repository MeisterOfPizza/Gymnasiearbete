using ArenaShooter.Templates.Weapons;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Templates.Enemies
{

    [CreateAssetMenu(menuName = "Templates/Enemies/Enemy Weapon")]
    sealed class EnemyWeaponTemplate : ScriptableObject
    {

        #region Editor

        [Header("Values")]
        [SerializeField] private ushort templateId;

        [Header("Stats")]
        [SerializeField]                 private float mobility;
        [SerializeField, Range(0f, 1f)]  private float accuracy       = 0.75f;
        [SerializeField, Range(0f, 90f)] private float maxAngleOffset = 5f;

        [Space]
        [SerializeField] private ushort damage         = 10;
        [SerializeField] private short  maxAmmoPerClip = 30;
        [SerializeField] private short  ammoPerFire    = 1;

        [Space]
        [SerializeField] private float fireCooldown = 0.1f;
        [SerializeField] private float reloadTime   = 1f;

        [Space]
        [SerializeField] private float burstFireInterval = 0.05f;
        [SerializeField] private sbyte burstShots        = 3;

        [Space]
        [Help(@"range and maxDistance has different uses depending on what type of weapon the enemy is using:

* If it's using a raycast weapon, only the range is used.
* Projectile weapons can shoot projectiles up to maxDistance but the explosion radius is only as wide as range.
* Electric weapons can hit its first target at the distance of maxDistance but only jump from target to target that are within range.
* Support weapons can only support targets that are within range.

NOTE: The enemy will only try to engage players that are within range (the variable).
")]

        [SerializeField] private float range       = 10f;
        [SerializeField] private float maxDistance = 50f;

        [Header("Logic")]
        [SerializeField] private FiringMode firingMode          = FiringMode.Automatic;
        [SerializeField] private bool       manualAmmoDepletion = false;

        [Space]
        [SerializeField] private WeaponOutputType outputType;

        [Header("Prefabs")]
        [Help(@"The prefab to be saved as firePrefab depends on what type of OutputType the weapon has:

* Raycast: Simple particle system or other reusable effect. No scripts needed.
* Projectile: Prefab with rigidbody and script (ProjectileShot) attached needed.
* Electric: Prefab with script (ElectricShot) attached needed.
* Support: Prefab with LineRenderer and script (SupportShot) attached needed."
)]
        [SerializeField] private GameObject firePrefab;

        [Space]
        [SerializeField] private GameObject modelPrefab;

        #endregion

        #region Getters

        public ushort TemplateId
        {
            get
            {
                return templateId;
            }
        }

        public float Mobility
        {
            get
            {
                return mobility;
            }
        }

        public float Accuracy
        {
            get
            {
                return accuracy;
            }
        }

        public float MaxAngleOffset
        {
            get
            {
                return maxAngleOffset;
            }
        }

        public ushort Damage
        {
            get
            {
                return damage;
            }
        }

        public short MaxAmmoPerClip
        {
            get
            {
                return maxAmmoPerClip;
            }
        }

        public short AmmoPerFire
        {
            get
            {
                return ammoPerFire;
            }
        }

        public float FireCooldown
        {
            get
            {
                return fireCooldown;
            }
        }

        public float ReloadTime
        {
            get
            {
                return reloadTime;
            }
        }

        public float BurstFireInterval
        {
            get
            {
                return burstFireInterval;
            }
        }

        public sbyte BurstShots
        {
            get
            {
                return burstShots;
            }
        }

        public float Range
        {
            get
            {
                return range;
            }
        }

        public float MaxDistance
        {
            get
            {
                return maxDistance;
            }

        }

        public FiringMode FiringMode
        {
            get
            {
                return firingMode;
            }
        }

        public bool ManualAmmoDepletion
        {
            get
            {
                return manualAmmoDepletion;
            }
        }

        public WeaponOutputType OutputType
        {
            get
            {
                return outputType;
            }
        }

        public GameObject FirePrefab
        {
            get
            {
                return firePrefab;
            }
        }

        public GameObject ModelPrefab
        {
            get
            {
                return modelPrefab;
            }
        }

        #endregion

    }

}
