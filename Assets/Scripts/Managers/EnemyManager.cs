using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    public bool testingEnemyManager = false;
    [Space(10)]
    public bool waveSpawnStart = true;
    public float timeBetweenSpawn = 10.0f;
    public Transform[] spawnpoints;

    public GameObject[] enemies;
    public GameObject[] enemyProjectiles;
    public GameObject[] enemyEffects;

    [Header("Waves")]
    public Waves[] easyWaves;
    public Waves testWave;

    private float waveSpawnTimer;
    private int spawnIterator = 0;

    private void Start()
    {
        for(int i = 0; i < enemies.Length; i++)
        {
            GameManager.Instance.poolManager.CreatePool(enemies[i], 30);
        }

        for(int i = 0; i < enemyProjectiles.Length; i++)
        {
            GameManager.Instance.poolManager.CreatePool(enemyProjectiles[i], 60);
        }

        for (int i = 0; i < enemyEffects.Length; i++)
        {
            GameManager.Instance.poolManager.CreatePool(enemyEffects[i], 20);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (testingEnemyManager)
            {
                StartCoroutine(SpawnWave(testWave));
            }
            else
            {
                waveSpawnStart = !waveSpawnStart;
            }
        }

        if (waveSpawnStart && Time.time > waveSpawnTimer)
        {
            waveSpawnTimer = Time.time + timeBetweenSpawn;
            if (spawnIterator > easyWaves.Length - 1)
            {
                spawnIterator = 0;
            }

            StartCoroutine(SpawnWave(easyWaves[spawnIterator]));
            spawnIterator++;
        }

    }

    public void StopSpawnWave()
    {
        waveSpawnStart = false;
        StopCoroutine("SpawnWave");
    }

    public IEnumerator SpawnWave(Waves wave)
    {
        int longestWave = wave.numberOfEnemies[0];
        
        float spawnRateOne = wave.queuedPatterns[0].spawnRate;
        float spawnRateTwo = 0.0f; float spawnRateThree = 0.0f;
        int spawnCounterOne = 0;
        int spawnCounterTwo = 0; int spawnCounterThree = 0;
        bool completedOne = false; bool completedTwo = false; bool completedThree = false;
        float nextSpawnOne = 0.0f; float nextSpawnTwo = 0.0f; float nextSpawnThree = 0.0f;

        if (wave.queuedPatterns.Length > 1)
        {
            longestWave = Mathf.Max(wave.numberOfEnemies[0], wave.numberOfEnemies[1]);
            spawnRateTwo = wave.queuedPatterns[1].spawnRate;
            
        }
        if (wave.queuedPatterns.Length > 2)
        {
            longestWave = Mathf.Max(longestWave, wave.numberOfEnemies[2]);
            spawnRateThree = wave.queuedPatterns[2].spawnRate;
            
        }

        while (!completedOne || !completedTwo || !completedThree)
        {
            if (spawnCounterOne >= wave.numberOfEnemies[0])
            {
                completedOne = true;
            }
            if (Time.time > nextSpawnOne && !completedOne)
            {
                nextSpawnOne = Time.time + spawnRateOne;
                GameManager.Instance.poolManager.ReuseObject(enemies[((wave.enemiesToSpawn[0] / (int)Mathf.Pow(10, spawnCounterOne)) % 10) - 1], spawnpoints[0].position, Quaternion.identity, wave.queuedPatterns[0], spawnCounterOne);
                spawnCounterOne++;
            } 

            if (wave.queuedPatterns.Length < 2 || spawnCounterTwo >= wave.numberOfEnemies[1])
            {
                completedTwo = true;
            }
            if (Time.time > nextSpawnTwo && !completedTwo)
            {
                nextSpawnTwo = Time.time + spawnRateTwo;
                GameManager.Instance.poolManager.ReuseObject(enemies[((wave.enemiesToSpawn[1] / (int)Mathf.Pow(10, spawnCounterTwo)) % 10) - 1], spawnpoints[wave.spawnPoints[1] - 1].position, Quaternion.identity, wave.queuedPatterns[1], spawnCounterTwo);
                spawnCounterTwo++;
            }

            if (wave.queuedPatterns.Length < 3 || spawnCounterThree >= wave.numberOfEnemies[2])
            {
                completedThree = true;
            }
            if (Time.time > nextSpawnThree && !completedThree)
            {
                nextSpawnThree = Time.time + spawnRateThree;
                GameManager.Instance.poolManager.ReuseObject(enemies[((wave.enemiesToSpawn[2] / (int)Mathf.Pow(10, spawnCounterThree)) % 10) - 1], spawnpoints[wave.spawnPoints[2] - 1].position, Quaternion.identity, wave.queuedPatterns[2], spawnCounterTwo);
                spawnCounterThree++;
            }

            yield return null;
        }
    }

    public void InitiateEnemySpawn()
    {
        spawnIterator = 0;
        waveSpawnStart = true;
    }
}