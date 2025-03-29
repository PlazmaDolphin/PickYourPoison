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
        // If no enemies are alive, spawn the next wave
        if (activeEnemies.Count == 0)
        {
            SpawnWave();
        }
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
                enemyScript.target = playerTarget; // Assign player as target
                enemyScript.OnDeath += () => activeEnemies.Remove(enemy); // Remove from active enemies on death
                activeEnemies.Add(enemy); // Track this enemy
            }
        }
    }
}
