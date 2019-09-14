using ArenaShooter.Entities;
using ArenaShooter.Templates.Enemies;
using System.Collections;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class WaveController : ServerController<WaveController>
    {

        #region Editor

        [Header("References")]
        [SerializeField] private Transform[] spawnPoints;

        [Space]
        [SerializeField] private EnemyTemplate[] spawnableEnemyTemplates;

        [Header("Values")]
        [SerializeField] private float firstSpawnOnWaveCooldown = 1f;

        #endregion

        #region Private variables

        private int  currentWave = 0;
        private bool waveIsOngoing;

        private int currentSpawnCount;

        #endregion

        protected override void OnAwake()
        {
            StartWave();
        }

        public void StartWave()
        {
            currentWave++;
            waveIsOngoing = true;

            currentSpawnCount = 0;

            StartCoroutine("WaveUpdate");

            // TODO: Send StartWave event to all clients.
        }

        public void EndWave()
        {
            waveIsOngoing = false;

            StopCoroutine("WaveUpdate");

            // TODO: Send EndWave event to all clients.
        }

        public void ResetWaves()
        {
            currentWave = 0;
        }

        #region Wave helpers

        private IEnumerator WaveUpdate()
        {
            yield return new WaitForSeconds(firstSpawnOnWaveCooldown);

            while (waveIsOngoing && currentSpawnCount < 1)
            {
                SpawnEnemy();

                yield return new WaitForSeconds(GetEnemySpawnCooldown());
            }
        }

        private void SpawnEnemy()
        {
            var template   = spawnableEnemyTemplates[Random.Range(0, spawnableEnemyTemplates.Length)];
            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            currentSpawnCount++;

            Enemy enemy = EntitySpawnController.Singleton.SpawnEntityOnServer<Enemy>(template.EnemyPrefab, null, spawnPoint.position, spawnPoint.rotation);
            enemy.Initialize(template.TemplateId);
        }

        #endregion

        #region Helpers

        private float GetEnemySpawnCooldown()
        {
            return 1f;
        }

        #endregion

    }

}
