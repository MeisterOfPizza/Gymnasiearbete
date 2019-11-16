using ArenaShooter.Combat;
using ArenaShooter.Player;
using ArenaShooter.Templates.Items;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Drops
{

    sealed class WeaponPartItemDrop : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private MeshRenderer meshRenderer;

        #endregion

        #region Private variables

        private WeaponPartItemTemplate weaponPartItemTemplate;

        #endregion

        public void Initialize(WeaponPartItemTemplate weaponPartItemTemplate)
        {
            this.weaponPartItemTemplate = weaponPartItemTemplate;

            meshRenderer.material.EnableKeyword("_EMISSION");

            switch (weaponPartItemTemplate.Rarity)
            {
                case WeaponPartItemRarity.Standard:
                default:
                    meshRenderer.material.SetColor("_Emission", WeaponPartItemWrapper.StandardRarityColor);
                    break;
                case WeaponPartItemRarity.Common:
                    meshRenderer.material.SetColor("_Emission", WeaponPartItemWrapper.CommonRarityColor);
                    break;
                case WeaponPartItemRarity.Uncommon:
                    meshRenderer.material.SetColor("_Emission", WeaponPartItemWrapper.UncommonRarityColor);
                    break;
                case WeaponPartItemRarity.Rare:
                    meshRenderer.material.SetColor("_Emission", WeaponPartItemWrapper.RareRarityColor);
                    break;
                case WeaponPartItemRarity.Legendary:
                    meshRenderer.material.SetColor("_Emission", WeaponPartItemWrapper.LegendaryRarityColor);
                    break;
                case WeaponPartItemRarity.Ancient:
                    meshRenderer.material.SetColor("_Emission", WeaponPartItemWrapper.AncientRarityColor);
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;

            var item = weaponPartItemTemplate.CreateRandomWeaponPartItem();

            if (item != null)
            {
                Profile.Inventory.WeaponPartItems.Add(item);
            }

            Destroy(gameObject);
        }

    }

}
