using ArenaShooter.Combat;
using ArenaShooter.Extensions;
using ArenaShooter.Player;
using ArenaShooter.Templates.Weapons;
using ArenaShooter.UI;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private GameObject    weaponPartItemScrollRect;
        [SerializeField] private RectTransform uiStockWeaponPartItemContainer;
        [SerializeField] private RectTransform uiBodyWeaponPartItemContainer;
        [SerializeField] private RectTransform uiBarrelWeaponPartItemContainer;
        [SerializeField] private GameObject    uiWeaponPartItemPrefab;

        [Space]
        [SerializeField] private GameObject showWeaponButton;

        [Space]
        [SerializeField] private RectTransform weaponOutputTypeWheel;
        [SerializeField] private Image[]       weaponOutputTypeWheelIcons;

        [Space]
        [SerializeField] private Transform weaponPartsDisplayContainer;

        [Space]
        [SerializeField] private TMP_InputField loadoutSlotNameInputField;

        [Header("Values")]
        [SerializeField] private float weaponPartFocusTime = 0.25f;

        [Space]
        [SerializeField] private float weaponOutputTypeWheelSpinTime      = 0.3f;
        [SerializeField] private Color weaponOutputTypeWheelSelectedColor = Color.white;
        [SerializeField] private Color weaponOutputTypeWheelNormalColor   = new Color(1f, 1f, 1f, 0.25f);

        #endregion

        #region Private variables

        private GameObject goBackMenu;

        private UILoadoutSlot selectedUILoadoutSlot;

        private UIWeaponPartItem selectedUIStockItem;
        private UIWeaponPartItem selectedUIBodyItem;
        private UIWeaponPartItem selectedUIBarrelItem;

        private WeaponPart selectedWeaponPart;

        private WeaponOutputType selectedWeaponOutputType;

        #endregion

        #region Start

        private void Start()
        {
            foreach (var slot in Profile.Inventory.LoadoutSlots)
            {
                var uiLoadoutSlot = Instantiate(uiLoadoutSlotPrefab, uiLoadoutSlotContainer).GetComponent<UILoadoutSlot>();
                uiLoadoutSlot.Initialize(slot);

                if (slot == Profile.SelectedLoadoutSlot)
                {
                    selectedUILoadoutSlot = uiLoadoutSlot;
                    selectedUILoadoutSlot.SetSelected(true);

                    // Update the selected weapon output type:
                    selectedWeaponOutputType = Profile.SelectedLoadoutSlot.Loadout.StockPartItem.Template.OutputType;

                    UpdateSelectedWeaponType(true);
                }
            }

            loadoutSlotNameInputField.onSubmit.AddListener((string input) =>
            {
                loadoutSlotNameInputField.SetTextWithoutNotify(Profile.SelectedLoadoutSlot?.Rename(input));
                selectedUILoadoutSlot.UpdateText();
            });
            loadoutSlotNameInputField.characterLimit      = LoadoutSlot.MAX_LOADOUT_SLOT_NAME_LENGTH;
            loadoutSlotNameInputField.characterValidation = TMP_InputField.CharacterValidation.None;
            loadoutSlotNameInputField.keyboardType        = TouchScreenKeyboardType.ASCIICapable;
            loadoutSlotNameInputField.contentType         = TMP_InputField.ContentType.Standard;

            UpdateWeaponPartItemLists();
            DisplaySelectedLoadout();
            weaponPartsDisplayContainer.gameObject.SetActive(false);
            showWeaponButton.SetActive(false);
        }

        #endregion

        #region Opening and closing

        public void OpenLoadoutMenu()
        {
            loadoutMenu.SetActive(true);

            weaponPartsDisplayContainer.gameObject.SetActive(true);
        }

        public void OpenLoadoutMenu(GameObject goBackMenu)
        {
            this.goBackMenu = goBackMenu;

            loadoutMenu.SetActive(true);

            weaponPartsDisplayContainer.gameObject.SetActive(true);
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

            weaponPartsDisplayContainer.gameObject.SetActive(false);
        }

        #endregion

        #region Selecting loadouts

        public void SetSelectedLoadoutSlot(UILoadoutSlot selected)
        {
            LoadoutController.Singleton.SetSelectedLoadoutSlot(selected.LoadoutSlot);

            selectedUILoadoutSlot.SetSelected(false);
            selectedUILoadoutSlot = selected;

            UpdateWeaponPartItemLists();
            DisplaySelectedLoadout();
            DeselectWeaponPart();

            // Update the selected weapon output type:
            selectedWeaponOutputType = Profile.SelectedLoadoutSlot.Loadout.StockPartItem.Template.OutputType;

            UpdateSelectedWeaponType(false);
        }

        private void DisplaySelectedLoadout()
        {
            loadoutSlotNameInputField.text = Profile.SelectedLoadoutSlot.LoadoutName;

            weaponPartsDisplayContainer.gameObject.SetActive(true);
            weaponPartsDisplayContainer.Clear();

            var stockPart  = Instantiate(Profile.SelectedLoadoutSlot.Loadout.StockPartItem.Template.WeaponPartPrefab, weaponPartsDisplayContainer).GetComponent<WeaponPart>();
            var bodyPart   = Instantiate(Profile.SelectedLoadoutSlot.Loadout.BodyPartItem.Template.WeaponPartPrefab, weaponPartsDisplayContainer).GetComponent<WeaponPart>();
            var barrelPart = Instantiate(Profile.SelectedLoadoutSlot.Loadout.BarrelPartItem.Template.WeaponPartPrefab, weaponPartsDisplayContainer).GetComponent<WeaponPart>();

            stockPart.Initialize(Profile.SelectedLoadoutSlot.Loadout.StockPartItem, true);
            bodyPart.Initialize(Profile.SelectedLoadoutSlot.Loadout.BodyPartItem, true);
            barrelPart.Initialize(Profile.SelectedLoadoutSlot.Loadout.BarrelPartItem, true);

            bodyPart.AttachStock(stockPart);
            bodyPart.AttachBarrel(barrelPart);
        }

        #endregion

        #region Selecting loadout weapon output type

        public void SelectLoadoutWeaponOutputType(int weaponOutputType)
        {
            if ((WeaponOutputType)weaponOutputType != selectedWeaponOutputType)
            {
                selectedWeaponOutputType = (WeaponOutputType)weaponOutputType;

                UpdateSelectedWeaponType(false);

                Profile.SelectedLoadoutSlot.SetLoadout(Loadout.CreateRandomLoadoutOfType(selectedWeaponOutputType));

                UpdateWeaponPartItemLists();
                DisplaySelectedLoadout();
                DeselectWeaponPart();
            }
        }

        #endregion

        #region Selecting weapon parts

        public void FocusWeaponPart(WeaponPart weaponPart)
        {
            if (selectedWeaponPart == null)
            {
                this.selectedWeaponPart = weaponPart;

                uiStockWeaponPartItemContainer.gameObject.SetActive(false);
                uiBodyWeaponPartItemContainer.gameObject.SetActive(false);
                uiBarrelWeaponPartItemContainer.gameObject.SetActive(false);

                switch (selectedWeaponPart.WeaponPartItem.BaseTemplate.Type)
                {
                    case WeaponPartTemplateType.Stock:
                        uiStockWeaponPartItemContainer.gameObject.SetActive(true);
                        break;
                    case WeaponPartTemplateType.Body:
                        uiBodyWeaponPartItemContainer.gameObject.SetActive(true);
                        break;
                    case WeaponPartTemplateType.Barrel:
                        uiBarrelWeaponPartItemContainer.gameObject.SetActive(true);
                        break;
                }

                weaponPartItemScrollRect.SetActive(true);

                weaponPartsDisplayContainer.SetChildrenActive(false);
                showWeaponButton.SetActive(true);
                weaponPart.gameObject.SetActive(true);

                StartCoroutine("FocusOnWeaponPart");
            }
        }

        public void DeselectWeaponPart()
        {
            selectedWeaponPart = null;

            weaponPartItemScrollRect.SetActive(false);
            showWeaponButton.SetActive(false);
            weaponPartsDisplayContainer.SetChildrenActive(true);

            StartCoroutine("FocusOnWeapon");
        }

        public void SwitchWeaponPartItem(UIWeaponPartItem uiWeaponPartItem)
        {
            switch (uiWeaponPartItem.WeaponPartItem.BaseTemplate.Type)
            {
                case WeaponPartTemplateType.Stock:
                    selectedUIStockItem.SetSelected(false);
                    selectedUIStockItem = uiWeaponPartItem;
                    break;
                case WeaponPartTemplateType.Body:
                    selectedUIBodyItem.SetSelected(false);
                    selectedUIBodyItem = uiWeaponPartItem;
                    break;
                case WeaponPartTemplateType.Barrel:
                    selectedUIBarrelItem.SetSelected(false);
                    selectedUIBarrelItem = uiWeaponPartItem;
                    break;
            }

            Profile.SelectedLoadoutSlot.Loadout.SwitchWeaponPartItem(uiWeaponPartItem.WeaponPartItem);
        }

        #endregion

        #region Helpers

        private void UpdateSelectedWeaponType(bool instant)
        {
            if (instant)
            {
                weaponOutputTypeWheel.localRotation = Quaternion.Euler(0, 0, 90f * (int)selectedWeaponOutputType);

                for (int i = 0; i < weaponOutputTypeWheelIcons.Length; i++)
                {
                    weaponOutputTypeWheelIcons[i].transform.up = Vector3.up;
                }
            }
            else
            {
                StopCoroutine("SpinWeaponTypeWheel");
                StartCoroutine("SpinWeaponTypeWheel");
            }

            for (int i = 0; i < weaponOutputTypeWheelIcons.Length; i++)
            {
                weaponOutputTypeWheelIcons[i].color = weaponOutputTypeWheelNormalColor;
            }

            weaponOutputTypeWheelIcons[(int)selectedWeaponOutputType].color = weaponOutputTypeWheelSelectedColor;
        }

        private void UpdateWeaponPartItemLists()
        {
            uiStockWeaponPartItemContainer.Clear();
            uiBodyWeaponPartItemContainer.Clear();
            uiBarrelWeaponPartItemContainer.Clear();

            uiStockWeaponPartItemContainer.gameObject.SetActive(false);
            uiBodyWeaponPartItemContainer.gameObject.SetActive(false);
            uiBarrelWeaponPartItemContainer.gameObject.SetActive(false);

            foreach (var stockItem in Profile.Inventory.GetStockItems(Profile.SelectedLoadoutSlot.Loadout.StockPartItem.BaseTemplate.OutputType))
            {
                var uiWeaponPartItem = Instantiate(uiWeaponPartItemPrefab, uiStockWeaponPartItemContainer).GetComponent<UIWeaponPartItem>();
                uiWeaponPartItem.Initialize(stockItem);

                if (stockItem == Profile.SelectedLoadoutSlot.Loadout.StockPartItem)
                {
                    selectedUIStockItem = uiWeaponPartItem;
                    selectedUIStockItem.SetSelected(true);
                }
            }

            foreach (var bodyItem in Profile.Inventory.GetBodyItems(Profile.SelectedLoadoutSlot.Loadout.BodyPartItem.BaseTemplate.OutputType))
            {
                var uiWeaponPartItem = Instantiate(uiWeaponPartItemPrefab, uiBodyWeaponPartItemContainer).GetComponent<UIWeaponPartItem>();
                uiWeaponPartItem.Initialize(bodyItem);

                if (bodyItem == Profile.SelectedLoadoutSlot.Loadout.BodyPartItem)
                {
                    selectedUIBodyItem = uiWeaponPartItem;
                    selectedUIBodyItem.SetSelected(true);
                }
            }

            foreach (var barrelItem in Profile.Inventory.GetBarrelItems(Profile.SelectedLoadoutSlot.Loadout.BarrelPartItem.BaseTemplate.OutputType))
            {
                var uiWeaponPartItem = Instantiate(uiWeaponPartItemPrefab, uiBarrelWeaponPartItemContainer).GetComponent<UIWeaponPartItem>();
                uiWeaponPartItem.Initialize(barrelItem);

                if (barrelItem == Profile.SelectedLoadoutSlot.Loadout.BarrelPartItem)
                {
                    selectedUIBarrelItem = uiWeaponPartItem;
                    selectedUIBarrelItem.SetSelected(true);
                }
            }
        }

        private IEnumerator FocusOnWeaponPart()
        {
            Vector3 offset  = -selectedWeaponPart.transform.position;
            float   elapsed = 0f;

            while (elapsed < weaponPartFocusTime)
            {
                weaponPartsDisplayContainer.position = Vector3.Lerp(weaponPartsDisplayContainer.position, offset, elapsed / weaponPartFocusTime);

                elapsed += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator FocusOnWeapon()
        {
            float elapsed = 0f;

            while (elapsed < weaponPartFocusTime)
            {
                weaponPartsDisplayContainer.position = Vector3.Lerp(weaponPartsDisplayContainer.position, Vector3.zero, elapsed / weaponPartFocusTime);

                elapsed += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator SpinWeaponTypeWheel()
        {
            Quaternion target = Quaternion.Euler(0, 0, 90f * (int)selectedWeaponOutputType);
            float elapsed = 0f;

            while (elapsed < weaponOutputTypeWheelSpinTime)
            {
                weaponOutputTypeWheel.localRotation = Quaternion.Slerp(weaponOutputTypeWheel.localRotation, target, elapsed / weaponOutputTypeWheelSpinTime);

                for (int i = 0; i < weaponOutputTypeWheelIcons.Length; i++)
                {
                    weaponOutputTypeWheelIcons[i].transform.up = Vector3.up;
                }

                elapsed += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
        }

        #endregion

    }

}
