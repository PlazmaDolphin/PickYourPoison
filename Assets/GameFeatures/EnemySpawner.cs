using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Enemy prefab
    public Transform playerTarget; // Player's Transform
    public float spawnY = 8f; // Off-screen spawn Y position
    public float minX = -8f, maxX = 8f; // X boundaries for spawning enemies

    private List<GameObject> activeEnemies = new List<GameObject>(); // Tracks active enemies
    private int currentWave = 0; // Tracks the current wave
    private bool isSpawningWave = false; // Prevents multiple simultaneous wave spawns

    void Update()
    {
        // Only spawn the next wave when all enemies are defeated
        if (activeEnemies.Count == 0 && !isSpawningWave)
        {
            StartCoroutine(StartNextWave()); // Use a coroutine for controlled wave spawning
        }
    }

    private IEnumerator StartNextWave()
    {
        isSpawningWave = true; // Prevent multiple wave spawns
        yield return new WaitForSeconds(2f); // Add a delay before the next wave starts

        SpawnWave(); // Call the wave spawning method
        isSpawningWave = false; // Reset spawning flag after the wave is spawned
    }

    private void SpawnWave()
    {
        currentWave++; // Increment the wave number
        int enemyCount = currentWave <= 3 ? currentWave + 1 : 1; // Number of enemies per wave

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        float spawnX = Random.Range(minX, maxX); // Random X position off-screen
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(spawnX, spawnY, 0), Quaternion.identity);

        if (enemy != null)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.target = playerTarget; // Assign the player as the target
                enemyScript.OnDeath += () => activeEnemies.Remove(enemy); // Remove from active enemies on death
                activeEnemies.Add(enemy); // Track this enemy
            }
        }
    }
}
