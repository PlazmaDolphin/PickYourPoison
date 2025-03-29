using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Enemy prefab with the health bar
    public Transform playerTarget; // The player's Transform
    public float spawnY = 6f; // Y position for enemy spawning
    public float minX = -8f, maxX = 8f; // X boundaries for spawning enemies

    void Start()
    {
        int enemyCount = Random.Range(1, 4); // Random number of enemies (1 to 3)

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
        }
    }
}
