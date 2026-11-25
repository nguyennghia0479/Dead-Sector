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
    [SerializeField] private int startEnemySpawnAmount = 5;
    [SerializeField] private Vector2 timeToSpawnEnemyRange = new(1f, 5f);
    [SerializeField] private List<EnemyController> aliveEnemies;
    private int enemySpawnAmount;
    private EnemySpawnPoint enemySpawnPoint;

    private Coroutine spawnWaveRoutine;
    private Coroutine spawnEnemyRoutine;
    private PlayerController player;

    private void Awake()
    {
        startSpawnEnemyWaitTime = new WaitForSeconds(timeToStartSpawnEnemy);
        currentWave = 1;
    }

    private void OnEnable()
    {
        UIEvents.OnNewGameBtnClicked += HandleNewGameButtonClicked;
        UIEvents.OnMainMenuBtnClicked += HandleMainGameButtonClicked;

        GameEvents.OnPlayerDeath += HandlePlayerDeath;
        GameEvents.OnEnemyDeath += HandleEnemyDeath;
    }

    private void OnDisable()
    {
        UIEvents.OnNewGameBtnClicked -= HandleNewGameButtonClicked;
        UIEvents.OnMainMenuBtnClicked -= HandleMainGameButtonClicked;

        GameEvents.OnPlayerDeath -= HandlePlayerDeath;
        GameEvents.OnEnemyDeath -= HandleEnemyDeath;
    }

    private void Start()
    {
        enemySpawnPoint = FindAnyObjectByType<EnemySpawnPoint>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        player = playerObject.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!waveEnable) return;

        startNewWaveTimer += Time.deltaTime;
        if (startNewWaveTimer > timeToStartNewWave && !isWaveStarting)
        {
            spawnWaveRoutine = StartCoroutine(SpawnWaveRoutine(true));
        }
    }

    private IEnumerator SpawnWaveRoutine(bool isWaveTimeout = false)
    {
        isWaveStarting = true;
        if (isWaveTimeout)
            yield return null;
        else
            yield return startSpawnEnemyWaitTime;

        GameEvents.RaiseWaveStarted();
        spawnEnemyRoutine = StartCoroutine(SpawnEnemyRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        for (int i = 0; i < enemySpawnAmount; i++)
        {
            if (!waveEnable) yield break;

            Vector3 spawnPosition = enemySpawnPoint.GetRandomSpawnPosition();
            EnemyController newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.Initialized(player);
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

    private void DestroyEnemies()
    {
        for (int i = aliveEnemies.Count - 1; i >= 0; i--)
        {
            if (aliveEnemies[i] != null && aliveEnemies[i].gameObject != null)
                Destroy(aliveEnemies[i].gameObject);
        }

        aliveEnemies.Clear();
    }

    private void HandleNewGameButtonClicked()
    {
        if (spawnWaveRoutine != null)
            StopCoroutine(spawnWaveRoutine);

        if (spawnEnemyRoutine != null)
            StopCoroutine(spawnEnemyRoutine);

        DestroyEnemies();
        currentWave = 1;
        enemySpawnAmount = startEnemySpawnAmount;
        startNewWaveTimer = 0;
        waveEnable = true;
        spawnWaveRoutine = StartCoroutine(SpawnWaveRoutine(false));
    }

    private void HandleMainGameButtonClicked()
    {
        waveEnable = false;
        DestroyEnemies();
    }

    private void HandlePlayerDeath()
    {
        if (spawnWaveRoutine != null)
            StopCoroutine(spawnWaveRoutine);

        if (spawnEnemyRoutine != null)
            StopCoroutine(spawnEnemyRoutine);

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
            spawnWaveRoutine = StartCoroutine(SpawnWaveRoutine(false));
        }
    }

    private void ResetStartNewWave()
    {
        startNewWaveTimer = 0;
        isWaveStarting = false;
    }
}
