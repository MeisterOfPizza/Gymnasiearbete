using ArenaShooter.Combat;
using ArenaShooter.Controllers;
using TMPro;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.UI
{

    sealed class UIWeaponPartItem : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private TMP_Text itemName;
        [SerializeField] private TMP_Text itemRarity;
        [SerializeField] private TMP_Text itemStats;

        #endregion

        #region Public properties

        public WeaponPartItemWrapper WeaponPartItem
        {
            get
            {
                return weaponPartItem;
            }
        }

        #endregion

        #region Private variables

        private WeaponPartItemWrapper weaponPartItem;
        private bool                  isSelected;

        #endregion

        public void Initialize(WeaponPartItemWrapper weaponPartItem)
        {
            this.weaponPartItem = weaponPartItem;

            itemName.text   = weaponPartItem.BaseTemplate.Name;
            itemRarity.text = weaponPartItem.GetRarityFormatted();
            itemStats.text  = weaponPartItem.GetStatsFormatted();
        }

        public void Select()
        {
            if (!isSelected)
            {
                isSelected = true;

                itemName.text = weaponPartItem.BaseTemplate.Name + " (<color=orange>Selected</color>)";

                UILoadoutController.Singleton.SwitchWeaponPartItem(this);
            }
        }

        public void SetSelected(bool isSelected)
        {
            this.isSelected = isSelected;

            itemName.text = weaponPartItem.BaseTemplate.Name + (isSelected ? " (<color=orange>Selected</color>)" : "");
        }

    }

}
