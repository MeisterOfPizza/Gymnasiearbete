using ArenaShooter.Controllers;
using ArenaShooter.Player;
using TMPro;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.UI
{

    class UILoadoutSlot : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private GameObject newIconGameObject;
        [SerializeField] private GameObject createdContainer;
        [SerializeField] private TMP_Text   loadoutNameText;
        [SerializeField] private TMP_Text   loadoutSlotInfoText;

        #endregion

        #region Public properties

        public LoadoutSlot LoadoutSlot
        {
            get
            {
                return loadoutSlot;
            }
        }

        #endregion

        #region Private variables

        private LoadoutSlot loadoutSlot;

        private bool isSelected;

        #endregion

        public void Initialize(LoadoutSlot loadoutSlot)
        {
            this.loadoutSlot = loadoutSlot;

            newIconGameObject.SetActive(!loadoutSlot.IsUnlocked);
            createdContainer.SetActive(loadoutSlot.IsUnlocked);

            UpdateText();
        }

        private void CreateNew()
        {
            loadoutSlot.Unlock();

            loadoutSlot.Rename(LoadoutSlot.GetRandomLoadoutName());

            UpdateText();
        }

        public void Select()
        {
            if (!isSelected)
            {
                if (!loadoutSlot.IsUnlocked)
                {
                    CreateNew();
                }

                isSelected = true;

                UpdateText();

                UILoadoutController.Singleton.SetSelectedLoadoutSlot(this);
            }
        }

        public void UpdateText()
        {
            loadoutNameText.text     = loadoutSlot.LoadoutName;
            loadoutSlotInfoText.text = isSelected ? "<color=orange>Selected</color>" : loadoutSlot.WeaponOutputTypeString();
        }

        public void SetSelected(bool isSelected)
        {
            this.isSelected = isSelected;

            UpdateText();
        }

    }

}
