using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private float timeToStartSpawnEnemy = 10f;
    [SerializeField] private float timeToStartNewWave = 90f;
    [SerializeField] private int currentWave;
    private WaitForSeconds startSpawnEnemyWaitTime;
    private float startNewWaveTimer;
    private bool isWaveStarting;
    private bool waveEnable;

    [Header("Spawn Enemy Settings")]
    [SerializeField] private EnemyController enemyPrefab;
    [SerializeField] private int enemySpawnAmount = 5;
    [SerializeField] private Vector2 timeToSpawnEnemyRange = new(1f, 5f);
    [SerializeField] private List<EnemyController> aliveEnemies;
    private EnemySpawnPoint enemySpawnPoint;

    private void Awake()
    {
        startSpawnEnemyWaitTime = new WaitForSeconds(timeToStartSpawnEnemy);
        currentWave = 1;
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerDeath += HandlePlayerDeath;
        GameEvents.OnEnemyDeath += HandleEnemyDeath;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDeath -= HandlePlayerDeath;
        GameEvents.OnEnemyDeath -= HandleEnemyDeath;
    }

    private void Start()
    {
        enemySpawnPoint = FindAnyObjectByType<EnemySpawnPoint>();

        //StartCoroutine(SpawnWaveRoutine());
    }

    private void Update()
    {
        if (!waveEnable) return;

        startNewWaveTimer += Time.deltaTime;
        if (startNewWaveTimer > timeToStartNewWave && !isWaveStarting)
        {
            StartCoroutine(SpawnWaveRoutine(true));
        }
    }

    private IEnumerator SpawnWaveRoutine(bool isWaveTimeout = false)
    {
        isWaveStarting = true;
        if (isWaveTimeout)
            yield return null;
        else
            yield return startSpawnEnemyWaitTime;

        StartCoroutine(SpawnEnemyRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        for (int i = 0; i < enemySpawnAmount; i++)
        {
            if (!waveEnable) yield break;

            Vector3 spawnPosition = enemySpawnPoint.GetRandomSpawnPosition();
            EnemyController newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            aliveEnemies.Add(newEnemy);

            float randomWaitTime = Random.Range(timeToSpawnEnemyRange.x, timeToSpawnEnemyRange.y);
            bool lastEnemySpawn = i == enemySpawnAmount - 1;
            if (lastEnemySpawn)
                yield return null;
            else
                yield return new WaitForSeconds(randomWaitTime);
        }

        ResetStartNewWave();
    }

    private void HandlePlayerDeath()
    {
        waveEnable = false;
    }

    private void HandleEnemyDeath(EnemyController enemy)
    {
        if (enemy == null || enemy.gameObject == null) return;

        for (int i = aliveEnemies.Count - 1; i >= 0; i--)
        {
            if (aliveEnemies[i] != null && aliveEnemies[i] == enemy)
                aliveEnemies.RemoveAt(i);
        }

        if (aliveEnemies.Count <= 0)
        {
            GameEvents.RaiseWaveCompleted(currentWave);
            currentWave++;
            enemySpawnAmount++;
            ResetStartNewWave();
            StartCoroutine(SpawnWaveRoutine());
        }
    }

    private void ResetStartNewWave()
    {
        startNewWaveTimer = 0;
        isWaveStarting = false;
    }
}
