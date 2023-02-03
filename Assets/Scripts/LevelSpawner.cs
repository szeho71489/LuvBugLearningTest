using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField]
    private float fishSpawnInterval;
    [SerializeField]
    private float enemySpawnInterval;

    [SerializeField]
    private Fish[] fishPrefabs;
    [SerializeField]
    private Fish[] enemyPrefabs;

    [SerializeField]
    private Vector2 spawnRangeVertical;
    [SerializeField]
    private Vector2 spawnPointHorizontal;


    private Coroutine spawnFishCoroutine, spawnEnemyCoroutine;

    private void Start()
    {
        spawnFishCoroutine = StartCoroutine(SpawnFishCoroutine());
        spawnEnemyCoroutine = StartCoroutine(SpawnEnemyCoroutine());
    }

    private IEnumerator SpawnFishCoroutine()
    {
        WaitForSeconds waitInterval = new WaitForSeconds(fishSpawnInterval);

        while (true)
        {
            yield return waitInterval;

            // Random spawn on left or right side
            float spawnX = Random.Range(0, 2) == 0 ? spawnPointHorizontal.x : spawnPointHorizontal.y;
            Vector3 spawnPosition = new Vector3(spawnX, Random.Range(spawnRangeVertical.x, spawnRangeVertical.y), 0);

            // Spawn and initialise fish
            Fish randomPrefab = fishPrefabs[Random.Range(0, fishPrefabs.Length)];
            Fish spawnedFish = Instantiate(randomPrefab, spawnPosition, Quaternion.identity);
            spawnedFish.Move(new Vector3(-spawnPosition.x, spawnPosition.y, spawnPosition.z));
        }
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        WaitForSeconds waitInterval = new WaitForSeconds(enemySpawnInterval);

        while (true)
        {
            yield return waitInterval;

            // Random spawn on left or right side
            float spawnX = Random.Range(0, 2) == 0 ? spawnPointHorizontal.x : spawnPointHorizontal.y;
            Vector3 spawnPosition = new Vector3(spawnX, Random.Range(spawnRangeVertical.x, spawnRangeVertical.y), 0);

            // Spawn and initialise fish
            Fish randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Fish spawnedEnemy = Instantiate(randomPrefab, spawnPosition, Quaternion.identity);
            spawnedEnemy.Move(new Vector3(-spawnPosition.x, spawnPosition.y, spawnPosition.z));
        }
    }

    public void StopLevel()
    {
        StopCoroutine(spawnFishCoroutine);
        StopCoroutine(spawnEnemyCoroutine);
    }
}
