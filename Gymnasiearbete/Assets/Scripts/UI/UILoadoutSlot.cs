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

        #endregion

        #region Private variables

        private LoadoutSlot loadoutSlot;

        #endregion

        public void Initialize(LoadoutSlot loadoutSlot)
        {
            this.loadoutSlot = loadoutSlot;

            newIconGameObject.SetActive(!loadoutSlot.IsUnlocked);
            createdContainer.SetActive(loadoutSlot.IsUnlocked);
            loadoutNameText.text = loadoutSlot.LoadoutName;
        }

        private void CreateNew()
        {
            loadoutSlot.Unlock();

            string[] loadoutNamePresets = new string[] { "Ranger", "Sniper", "Assassin", "Spy", "Soldier" };
            loadoutSlot.Rename(loadoutNamePresets[Random.Range(0, loadoutNamePresets.Length)]);

            UpdateName();
        }

        public void Select()
        {
            if (!loadoutSlot.IsUnlocked)
            {
                CreateNew();
            }

            LoadoutController.Singleton.SetSelectedLoadoutSlot(loadoutSlot);
        }

        public void UpdateName()
        {
            loadoutNameText.text = loadoutSlot.LoadoutName;
        }

    }

}
