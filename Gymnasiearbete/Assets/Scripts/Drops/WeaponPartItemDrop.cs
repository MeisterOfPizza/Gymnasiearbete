﻿using ArenaShooter.Combat;
using ArenaShooter.Extensions;
using ArenaShooter.Player;
using ArenaShooter.Templates.Items;
using Bolt;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Drops
{

    sealed class WeaponPartItemDrop : MonoBehaviour
    {

        #region Private statics

        private static string playerNameHTMLColor = ColorUtility.ToHtmlStringRGBA(new Color(1f, 0.5744222f, 0f));

        #endregion

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
                    meshRenderer.material.SetColor("_Color", WeaponPartItemWrapper.StandardRarityColor);
                    meshRenderer.material.SetColor("_Emission", WeaponPartItemWrapper.StandardRarityColor);
                    break;
                case WeaponPartItemRarity.Common:
                    meshRenderer.material.SetColor("_Color", WeaponPartItemWrapper.CommonRarityColor);
                    meshRenderer.material.SetColor("_Emission", WeaponPartItemWrapper.CommonRarityColor);
                    break;
                case WeaponPartItemRarity.Uncommon:
                    meshRenderer.material.SetColor("_Color", WeaponPartItemWrapper.UncommonRarityColor);
                    meshRenderer.material.SetColor("_Emission", WeaponPartItemWrapper.UncommonRarityColor);
                    break;
                case WeaponPartItemRarity.Rare:
                    meshRenderer.material.SetColor("_Color", WeaponPartItemWrapper.RareRarityColor);
                    meshRenderer.material.SetColor("_Emission", WeaponPartItemWrapper.RareRarityColor);
                    break;
                case WeaponPartItemRarity.Legendary:
                    meshRenderer.material.SetColor("_Color", WeaponPartItemWrapper.LegendaryRarityColor);
                    meshRenderer.material.SetColor("_Emission", WeaponPartItemWrapper.LegendaryRarityColor);
                    break;
                case WeaponPartItemRarity.Ancient:
                    meshRenderer.material.SetColor("_Color", WeaponPartItemWrapper.AncientRarityColor);
                    meshRenderer.material.SetColor("_Emission", WeaponPartItemWrapper.AncientRarityColor);
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && other.GetComponent<PlayerController>() is PlayerController playerController && playerController.entity.IsOwner)
            {
                var item = weaponPartItemTemplate.CreateRandomWeaponPartItem();

                if (item != null)
                {
                    GameLogMessageEvent itemDropEvent = GameLogMessageEvent.Create(GlobalTargets.Everyone);
                    itemDropEvent.Message             = $"<color=#{playerNameHTMLColor}>{UserUtils.GetUsername()}</color> picked up {item.GetRarityFormatted()}";
                    itemDropEvent.Send();

                    Profile.Inventory.WeaponPartItems.Add(item);
                }

                Destroy(gameObject);
            }
        }

    }

}
