using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Enemy prefab with the health bar
    public Transform playerTarget; // The player's Transform
    public float spawnY = 6f; // Y position for enemy spawning
    public float minX = -8f, maxX = 8f; // X boundaries for spawning enemies
    public float respawnDelay = 2f; // Delay before respawning the next wave

    private int currentEnemyCount = 0; // Tracks how many enemies are active
    private bool isRespawning = false; // Prevent overlapping respawn calls

    void Start()
    {
        SpawnWave(); // Spawn the first wave of enemies
    }

    private void Update()
    {
        // Check if all enemies are defeated and no respawn is in progress
        if (currentEnemyCount <= 0 && !isRespawning)
        {
            isRespawning = true; // Prevent multiple respawn calls
            Invoke(nameof(SpawnWave), respawnDelay);
        }
    }

    private void SpawnWave()
    {
        // Spawn a random number of enemies (1 to 3)
        int enemyCount = Random.Range(1, 4);
        currentEnemyCount = enemyCount;

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
        }

        isRespawning = false; // Reset respawning flag
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
            enemyScript.target = playerTarget; // Set the target for the enemy
            enemyScript.OnDeath += HandleEnemyDeath; // Attach callback for when this enemy dies
        }
    }

    private void HandleEnemyDeath()
    {
        // Reduce the active enemy count when an enemy is killed
        currentEnemyCount--;
    }
}
