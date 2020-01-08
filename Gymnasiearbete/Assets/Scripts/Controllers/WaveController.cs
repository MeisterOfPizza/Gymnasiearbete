using ArenaShooter.Entities;
using ArenaShooter.Extensions;
using ArenaShooter.Templates.Enemies;
using Bolt;
using Bolt.Matchmaking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class WaveController : ServerController<WaveController>
    {

        #region Public constants

        public const float WAVE_COUNTDOWN_TIME  = 3f;
        public const float WAVE_START_WAIT_TIME = 3f;
        public const float WAVE_END_WAIT_TIME   = 3f;

        #endregion

        #region Private constants

        private const int MAX_ENEMIES_ON_MAP = 50;
        private const int MIN_ENEMIES_ON_MAP = 10;
        private const int ENEMIES_IN_POOLS   = 10;

        #endregion

        #region Editor

        [Header("References")]
        [SerializeField] private Transform[] spawnPoints;

        [Space]
        [SerializeField] private EnemyTemplate[] spawnableEnemyTemplates;

        #endregion

        #region Private variables

        private Dictionary<EnemyTemplate, GameObjectPool<Enemy>> enemyPools = new Dictionary<EnemyTemplate, GameObjectPool<Enemy>>();

        private int  currentWave = 0;
        private bool waveIsOngoing;

        private int spawnedEnemiesCount;

        private int currentSpawnCount;
        private int maxSpawnCount;

        #endregion

        public void BeginWaveController()
        {
            SetupEnemyPools();

            StartWave();
        }

        public void StartWave()
        {
            currentWave++;
            waveIsOngoing = true;

            spawnedEnemiesCount = 0;
            currentSpawnCount   = 0;
            maxSpawnCount       = CalculateMaxEnemyCount();

            WaveStartEvent waveStartEvent = WaveStartEvent.Create(GlobalTargets.Everyone);
            waveStartEvent.WaveNumber     = currentWave;
            waveStartEvent.EnemyCount     = maxSpawnCount;
            waveStartEvent.Send();

            StartCoroutine("WaveUpdate");
        }

        public void EndWave()
        {
            waveIsOngoing = false;

            WaveEndEvent waveEndEvent = WaveEndEvent.Create(GlobalTargets.Everyone);
            waveEndEvent.WaveNumber   = currentWave;
            waveEndEvent.Send();
        }

        public void ResetWaves()
        {
            currentWave = 0;
        }

        #region Wave helpers

        private IEnumerator WaveUpdate()
        {
            yield return new WaitForSecondsRealtime(WAVE_START_WAIT_TIME);

            float waveStartCountdown    = WAVE_COUNTDOWN_TIME;
            int   waveStartCountdownInt = Mathf.CeilToInt(waveStartCountdown);

            WaveCountdownEvent waveCountdownEvent = WaveCountdownEvent.Create(GlobalTargets.Everyone);
            waveCountdownEvent.Time               = waveStartCountdownInt;
            waveCountdownEvent.Send();

            while (waveStartCountdown > 0f)
            {
                waveStartCountdown -= Time.deltaTime;

                int newWaveStartCountdownInt = Mathf.CeilToInt(waveStartCountdown);

                if (waveStartCountdownInt != newWaveStartCountdownInt)
                {
                    waveStartCountdownInt = newWaveStartCountdownInt;

                    waveCountdownEvent      = WaveCountdownEvent.Create(GlobalTargets.Everyone);
                    waveCountdownEvent.Time = waveStartCountdownInt;
                    waveCountdownEvent.Send();
                }

                yield return new WaitForEndOfFrame();
            }

            while (waveIsOngoing && currentSpawnCount < maxSpawnCount && spawnedEnemiesCount < MAX_ENEMIES_ON_MAP)
            {
                SpawnEnemy();

                yield return new WaitForSeconds(CalculateEnemySpawnCooldown());
            }

            yield return new WaitUntil(() => spawnedEnemiesCount == 0);

            EndWave();

            yield return new WaitForSecondsRealtime(WAVE_END_WAIT_TIME);

            StartWave();
        }

        private void SpawnEnemy()
        {
            int tries    = 0;
            int index    = Random.Range(0, spawnableEnemyTemplates.Length);
            var template = spawnableEnemyTemplates[index];

            while (tries < spawnableEnemyTemplates.Length && enemyPools[template].PooledItemCount == 0)
            {
                tries++;
                index = (index + 1) % spawnableEnemyTemplates.Length;
                template = spawnableEnemyTemplates[index];
            }

            if (tries < spawnableEnemyTemplates.Length)
            {
                var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                currentSpawnCount++;

                var enemy = enemyPools[template].GetItem();
                enemy.transform.position = spawnPoint.position;

                EntityRevivedEvent @event = EntityRevivedEvent.Create(enemy.entity, EntityTargets.Everyone);
                @event.RevivedEntity      = enemy.entity;
                @event.Send();

                spawnedEnemiesCount++;
            }
        }

        public void DespawnEnemy(Enemy enemy)
        {
            enemyPools[enemy.EnemyTemplate].PoolItem(enemy);

            spawnedEnemiesCount--;
        }

        #endregion

        #region Helpers

        private void SetupEnemyPools()
        {
            enemyPools.Clear();

            foreach (var template in spawnableEnemyTemplates)
            {
                var pool = new GameObjectPool<Enemy>(null,
                                                     template.EnemyPrefab,
                                                     ENEMIES_IN_POOLS,
                                                     EntitySpawnController.Singleton.SpawnEntityOnServer<Enemy>,
                                                     Enemy.SetEntityActive);

                enemyPools.Add(template, pool);

                foreach (var enemy in pool.PooledItems)
                {
                    enemy.Initialize(template);
                }
            }
        }

        /// <summary>
        /// Calculates the spawn cooldown for the next enemy.
        /// </summary>
        private float CalculateEnemySpawnCooldown()
        {
            // Formula: ( 0.75 / d ) * ( 25 / (p * ( r + 1 ) ) )^0.25

            float difficulty = 1f + (int)ServerUtils.CurrentServerHostInfo.Difficulty * 0.25f; // 1 <= d <= 1.5 with 0.25 per player added
            float players    = BoltMatchmaking.CurrentSession.ConnectionsCurrent + 1;          // 1 <= p <= 4 where p is the current number of players connected

            return 0.75f / difficulty * Mathf.Pow(25 / (players * (currentWave + 1)), 0.25f);
        }

        /// <summary>
        /// Calculates the spawn count of enemies for this wave.
        /// </summary>
        private int CalculateEnemySpawnCount()
        {
            // Formula: d * ( 2p + pr + 6 )

            float difficulty = 1f + (int)ServerUtils.CurrentServerHostInfo.Difficulty * 0.25f; // 1 <= d <= 1.5 with 0.25 per player added
            float players    = BoltMatchmaking.CurrentSession.ConnectionsCurrent + 1;          // 1 <= p <= 4 where p is the current number of players connected

            return Mathf.RoundToInt(difficulty * (2 * players + players * currentWave + 6));
        }

        /// <summary>
        /// Calculates the max amount of enemies present on the map simultaneously.
        /// </summary>
        private int CalculateMaxEnemyCount()
        {
            /// Formula: min ( max ( dp * (r + 1) / 2.5 ), <see cref="MIN_ENEMIES_ON_MAP"/> ), <see cref="MAX_ENEMIES_ON_MAP"/> )

            float difficulty = 1f + (int)ServerUtils.CurrentServerHostInfo.Difficulty * 0.25f; // 1 <= d <= 1.5 with 0.25 per player added
            float players    = BoltMatchmaking.CurrentSession.ConnectionsCurrent + 1;          // 1 <= p <= 4 where p is the current number of players connected

            return (int)Mathf.Clamp(difficulty * players * (currentWave + 1) / 2.5f, MIN_ENEMIES_ON_MAP, MAX_ENEMIES_ON_MAP);
        }

        #endregion

        protected override void OnDestroy()
        {
            base.OnDestroy();

            enemyPools.Clear();
        }

    }

}
