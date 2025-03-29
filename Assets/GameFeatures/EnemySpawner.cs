using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Enemy prefab with the health bar
    public Transform playerTarget; // The player's Transform
    public float spawnY = 6f; // Y position for enemy spawning
    public float minX = -8f, maxX = 8f; // X boundaries for spawning enemies
    public float respawnDelay = 2f; // Delay before respawning the next wave

    private List<GameObject> activeEnemies = new List<GameObject>(); // Tracks active enemies

    void Start()
    {
        SpawnWave(); // Spawn the first wave of enemies
    }

    private void Update()
    {
        // Check for null enemies and recreate them
        for (int i = activeEnemies.Count - 1; i >= 0; i--) // Loop backward for safe removal
        {
            if (activeEnemies[i] == null)
            {
                activeEnemies.RemoveAt(i); // Remove the null reference
                SpawnEnemy(); // Spawn a replacement enemy
            }
        }

        // Check if all enemies are defeated and respawn the wave
        if (activeEnemies.Count <= 0)
        {
            Invoke(nameof(SpawnWave), respawnDelay);
        }
    }

    private void SpawnWave()
    {
        // Spawn a random number of enemies (1 to 3)
        int enemyCount = Random.Range(1, 4);
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        // Random X position for the enemy
        float spawnX = Random.Range(minX, maxX);

        // Instantiate the enemy at the random position
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(spawnX, spawnY, 0), Quaternion.identity);

        if (enemy == null)
        {
            Debug.LogError("Failed to instantiate enemyPrefab!");
            return; // Exit if instantiation fails
        }

        // Assign the player's Transform as the enemy's target
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.target = playerTarget; // Set the target for the enemy
            enemyScript.OnDeath += HandleEnemyDeath; // Attach callback for when this enemy dies
        }
        else
        {
            Debug.LogError("Enemy prefab is missing the Enemy script!");
        }

        // Add the newly spawned enemy to the active enemies list
        activeEnemies.Add(enemy);
    }

    private void HandleEnemyDeath()
    {
        // This method can remain as-is if you're handling the death event separately.
        // The Update() loop will ensure null enemies are removed and re-spawned.
    }
}
