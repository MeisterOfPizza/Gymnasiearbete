using ArenaShooter.Drops;
using ArenaShooter.Extensions.Attributes;
using ArenaShooter.Templates.Items;
using System.Linq;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    [Persistent]
    class WeaponPartItemController : Controller<WeaponPartItemController>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private WeaponPartItemTemplate[] weaponPartItemTemplates;

        [Header("Prefabs")]
        [SerializeField] private GameObject weaponPartItemDropPrefab;

        #endregion

        public WeaponPartItemTemplate GetWeaponPartItemTemplate(int id)
        {
            return weaponPartItemTemplates.FirstOrDefault(t => t.Id == id);
        }

        public void SpawnWeaponPartItemDrop(Vector3 position, WeaponPartItemTemplate weaponPartItemTemplate)
        {
            if (weaponPartItemTemplate != null)
            {
                var drop = Instantiate(weaponPartItemDropPrefab, position, Quaternion.identity).GetComponent<WeaponPartItemDrop>();
                drop.Initialize(weaponPartItemTemplate);
            }
        }

    }

}
