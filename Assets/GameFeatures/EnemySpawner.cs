using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Enemy prefab with the health bar
    public Transform playerTarget; // The player's Transform
    public float spawnY = 8f; // Off-screen spawn Y position
    public float minX = -8f, maxX = 8f; // X boundaries for spawning enemies
    public float moveInDelay = 1f; // Time delay before enemies move onto the screen
    public float respawnDelay = 2f; // Delay before respawning the next wave

    private List<GameObject> activeEnemies = new List<GameObject>(); // Tracks active enemies
    private int currentWave = 0; // Tracks the current wave (two enemies, three enemies, one enemy)

    void Start()
    {
        // Start with no enemies on screen
        Invoke(nameof(SpawnWave), respawnDelay);
    }

    private void Update()
    {
        // Check for null enemies and remove them
        for (int i = activeEnemies.Count - 1; i >= 0; i--) // Loop backward for safe removal
        {
            if (activeEnemies[i] == null)
            {
                activeEnemies.RemoveAt(i); // Remove the null reference
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
        currentWave++; // Increment the wave

        int enemyCount = 0;

        // Define the number of enemies based on the wave
        switch (currentWave)
        {
            case 1: // Wave 1: Spawn 2 enemies
                enemyCount = 2;
                break;
            case 2: // Wave 2: Spawn 3 enemies
                enemyCount = 3;
                break;
            case 3: // Wave 3: Spawn 1 enemy
                enemyCount = 1;
                break;
            default: // Additional waves can follow a different pattern
                enemyCount = 1; // Default single enemy per wave
                break;
        }

        // Spawn the defined number of enemies
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        // Random X position for the enemy
        float spawnX = Random.Range(minX, maxX);

        // Instantiate the enemy at the random position off-screen
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
            StartCoroutine(MoveEnemyOntoScreen(enemy)); // Move the enemy onto the screen
        }
        else
        {
            Debug.LogError("Enemy prefab is missing the Enemy script!");
        }

        // Add the newly spawned enemy to the active enemies list
        activeEnemies.Add(enemy);
    }

    private IEnumerator MoveEnemyOntoScreen(GameObject enemy)
    {
        yield return new WaitForSeconds(moveInDelay); 

        // Gradually move the enemy onto the screen
        Vector3 newPosition = enemy.transform.position;
        newPosition.y -= spawnY; 
        enemy.transform.position = newPosition; 
    }

    private void HandleEnemyDeath()
    {
        // Reduce the active enemy count when an enemy is killed
        // This is handled indirectly by Update() removing null references
    }
}
