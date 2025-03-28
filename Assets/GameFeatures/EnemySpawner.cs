using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign your enemy prefab in the Inspector
    public float spawnInterval = 7f; // Time between spawns

    void Start()
    {
        // Start spawning after 2 seconds, then every spawnInterval seconds
        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned!");
            return;
        }

        // Spawn enemy inside the screen bounds using the static method in the Enemy class
        Enemy.SpawnEnemy(enemyPrefab);
    }
}
