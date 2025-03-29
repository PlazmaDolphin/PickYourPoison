using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Enemy prefab with the health bar
    public Transform playerTarget; // The player's Transform
    public float spawnY = 6f; // Y position for enemy spawning
    public float minX = -8f, maxX = 8f; // X boundaries for spawning enemies
    public float respawnDelay = 2f; // Delay before respawning the next wave

    private int currentEnemyCount = 0; // Tracks how many enemies are active

    void Start()
    {
        SpawnWave();
    }

    private void Update()
    {
        // Check if all enemies are defeated
        if (currentEnemyCount <= 0)
        {
            // Start respawning a new wave
            Invoke(nameof(SpawnWave), respawnDelay);
        }
    }

    private void SpawnWave()
    {
        // Cancel any redundant Invoke calls to prevent multiple waves spawning at once
        CancelInvoke(nameof(SpawnWave));

        // Spawn a random number of enemies (1 to 3)
        int enemyCount = Random.Range(1, 4);
        currentEnemyCount = enemyCount;

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

        // Assign the player's Transform as the enemy's target
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.target = playerTarget;
            enemyScript.GetComponent<Enemy>().OnDeath += HandleEnemyDeath; // Attach callback for when this enemy dies
        }
    }

    private void HandleEnemyDeath()
    {
        // Reduce the active enemy count when an enemy is killed
        currentEnemyCount--;
    }
}
