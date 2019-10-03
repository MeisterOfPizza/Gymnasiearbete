using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArenaShooter.Extensions;
using ArenaShooter.Combat.Pickup;
using ArenaShooter.Templates.Interactable;

namespace ArenaShooter.Controllers
{
    class InteractableController : Controller<InteractableController>
    {
        #region Editor

        [SerializeField] private int             amountOfLargeMedkits;
        [SerializeField] private int             amountOfSmallMedkits;
        [SerializeField] private GameObject      largeMedkitPrefab;
        [SerializeField] private GameObject      smallMedKitPrefab;
        [SerializeField] private Transform       medKitContainer;
        [SerializeField] private Transform[]     spawnPoints;

        #endregion

        #region Private Variables

        private GameObjectPool<Interactable> largeMedkitsPool;
        private GameObjectPool<Interactable> smallMedkitsPool;
        private int positionInSpawnPoint = 0;

        #endregion

        #region Methods

        private void Start()
        {
            largeMedkitsPool = new GameObjectPool<Interactable>(medKitContainer, largeMedkitPrefab, amountOfLargeMedkits);
            smallMedkitsPool = new GameObjectPool<Interactable>(medKitContainer, smallMedKitPrefab, amountOfSmallMedkits);
           
            
        }

        private void SpawnLargeMedkit()
        {
            if(positionInSpawnPoint > spawnPoints.Length - 1)
            {
                positionInSpawnPoint = 0;
            }
            var Interactable = largeMedkitsPool.GetItem();
            Interactable.transform.position = spawnPoints[positionInSpawnPoint].position;
        }

        private void SpawnSmallMedkit()
        {
            if (positionInSpawnPoint > spawnPoints.Length - 1)
            {
                positionInSpawnPoint = 0;
            }
            var Interactable = smallMedkitsPool.GetItem();
            Interactable.transform.position = spawnPoints[positionInSpawnPoint].position;
        }

        #endregion
    }
}

