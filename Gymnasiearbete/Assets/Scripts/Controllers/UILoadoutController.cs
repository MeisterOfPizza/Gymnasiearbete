using ArenaShooter.Extensions;
using ArenaShooter.Player;
using ArenaShooter.Templates.Weapons;
using ArenaShooter.UI;
using TMPro;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class UILoadoutController : Controller<UILoadoutController>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private GameObject loadoutMenu;

        [Space]
        [SerializeField] private RectTransform uiLoadoutSlotContainer;
        [SerializeField] private GameObject    uiLoadoutSlotPrefab;

        [Space]
        [SerializeField] private TMP_Text loadoutSlotNameText;

        #endregion

        #region Private variables

        private GameObject goBackMenu;

        #endregion

        private void Start()
        {
            foreach (var slot in Profile.Inventory.LoadoutSlots)
            {
                var loadoutSlot = Instantiate(uiLoadoutSlotPrefab, uiLoadoutSlotContainer).GetComponent<UILoadoutSlot>();
                loadoutSlot.Initialize(slot);
            }
        }

        public void OpenLoadoutMenu()
        {
            loadoutMenu.SetActive(true);
        }

        public void OpenLoadoutMenu(GameObject goBackMenu)
        {
            this.goBackMenu = goBackMenu;

            loadoutMenu.SetActive(true);
        }

        public void CloseLoadoutMenu()
        {
            loadoutMenu.SetActive(false);

            if (goBackMenu.IsNull())
            {
                UIMainMenuController.Singleton.OpenMainMenu();
            }
            else
            {
                goBackMenu.SetActive(true);
            }
        }

        public void SetSelectedLoadoutSlot(LoadoutSlot selectedLoadoutSlot)
        {
            LoadoutController.Singleton.SetSelectedLoadoutSlot(selectedLoadoutSlot);

            loadoutSlotNameText.text = selectedLoadoutSlot.LoadoutName;
        }

    }

}
