using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy Spawn Settings")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private float enemySpawnRateMax = 7f;
    [SerializeField] private float enemySpawnRateMin = 3f;
    private float enemySpawnRate;
    private float enemySpawnTimer = 0f;

    [Header("Item Spawn Settings")]
    [SerializeField] private GameObject[] itemPrefabs;
    [SerializeField] private Transform itemContainer;
    [SerializeField] private float itemSpawnRateMax = 40f;
    [SerializeField] private float itemSpawnRateMin = 30f;
    private float itemSpawnRate;
    private float itemSpawnTimer = 0f;

    [Header("Asteroid Spawn Settings")]
    [SerializeField] private GameObject[] asteroidPrefabs;
    [SerializeField] private Transform asteroidContainer;
    [SerializeField] private float asteroidSpawnRateMax = 20f;
    [SerializeField] private float asteroidSpawnRateMin = 10f;
    private float asteroidSpawnRate;
    private float asteroidSpawnTimer = 0f;

    private float spawnBorderXRange = 8f;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawnRate = 3f;    //Initial enemy spawn delay when starting the game
        itemSpawnRate = Random.Range(itemSpawnRateMin, itemSpawnRateMax);
        asteroidSpawnRate = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        #region Enemy Spawn
        enemySpawnTimer += Time.deltaTime;
        if (enemySpawnTimer > enemySpawnRate)
        {
            SpawnEnemy();
            enemySpawnTimer = 0;

            enemySpawnRate = Random.Range(enemySpawnRateMin, enemySpawnRateMax);
        }
        #endregion

        #region Item Spawn
        itemSpawnTimer += Time.deltaTime;
        if (itemSpawnTimer > itemSpawnRate)
        {
            SpawnItem();
            itemSpawnTimer = 0;

            itemSpawnRate = Random.Range(itemSpawnRateMin, itemSpawnRateMax);
        }
        #endregion

        #region Asteroid Spawn
        asteroidSpawnTimer += Time.deltaTime;
        if (asteroidSpawnTimer > asteroidSpawnRate)
        {
            SpawnAsteroid();
            asteroidSpawnTimer = 0;

            asteroidSpawnRate = Random.Range(asteroidSpawnRateMin, asteroidSpawnRateMax);
        }
        #endregion 
    }

    private void SpawnEnemy()
    {
        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyInstance = Instantiate(enemyPrefabs[randomEnemyIndex], GenerateSpawnPosition(), 
            Quaternion.identity, enemyContainer);
    }

    private void SpawnItem()
    {
        int randomItemIndex = Random.Range(0, itemPrefabs.Length);
        GameObject itemInstance = Instantiate(itemPrefabs[randomItemIndex], GenerateSpawnPosition(), 
            Quaternion.identity, itemContainer);
    }

    private void SpawnAsteroid()
    {
        int randomAsteroidIndex = Random.Range(0, asteroidPrefabs.Length);
        GameObject asteroidInstance = Instantiate(asteroidPrefabs[randomAsteroidIndex], GenerateSpawnPosition(), 
            Quaternion.identity, asteroidContainer);
    }

    private Vector3 GenerateSpawnPosition()
    {
        Vector3 spawnRange = new Vector3(Random.Range(-spawnBorderXRange, spawnBorderXRange), transform.position.y, 0);
        return spawnRange;
    }

    public void IncreaseEnemySpawnRate(float amount)    //Naming-Note: "Increase" means quicker/faster spawns -> logically, spawnRate interval numbers are actually decreased!
    {
        enemySpawnRateMax -= amount;
        enemySpawnRateMin -= amount;

        if (enemySpawnRateMin < 1)
        {
            enemySpawnRateMin = 1;
        }
        if (enemySpawnRateMax < 1)
        {
            enemySpawnRateMax = 1;
        }
    }
}