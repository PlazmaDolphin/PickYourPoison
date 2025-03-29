using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Enemy prefab
    public Transform playerTarget; // Player's Transform
    public float spawnY = 8f; // Off-screen spawn Y position
    public float minX = -8f, maxX = 8f; // X boundaries for spawning enemies

    private List<GameObject> activeEnemies = new List<GameObject>(); // Tracks active enemies
    private int currentWave = 0; // Tracks the current wave

    void Update()
    {
        // Check if all enemies are defeated before spawning the next wave
        if (activeEnemies.Count <= 0)
        {
            // Start the next wave
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        currentWave++; // Increment the wave number

        int enemyCount = 0;

        // Define the number of enemies per wave
        switch (currentWave)
        {
            case 1:
                enemyCount = 2; // Wave 1: Spawn 2 enemies
                break;
            case 2:
                enemyCount = 3; // Wave 2: Spawn 3 enemies
                break;
            case 3:
                enemyCount = 1; // Wave 3: Spawn 1 enemy
                break;
            default:
                enemyCount = 1; // Default additional waves spawn 1 enemy
                break;
        }

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        float spawnX = Random.Range(minX, maxX); // Random X position off-screen

        // Instantiate the enemy off-screen
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(spawnX, spawnY, 0), Quaternion.identity);

        if (enemy == null)
        {
            Debug.LogError("Failed to instantiate enemyPrefab!");
            return;
        }

        // Assign the player's Transform as the enemy's target
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.target = playerTarget; // Set the target for the enemy
            enemyScript.OnDeath += HandleEnemyDeath; // Attach callback for enemy death
        }
        else
        {
            Debug.LogError("Enemy prefab is missing the Enemy script!");
        }

        activeEnemies.Add(enemy); // Add to active enemies list
    }

    private void HandleEnemyDeath()
    {
        // This callback is triggered when an enemy dies
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] == null)
            {
                activeEnemies.RemoveAt(i); // Clean up null references
            }
        }
    }
}
