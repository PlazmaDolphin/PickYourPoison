using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign your enemy prefab in the Inspector
    public float spawnInterval = 3f; // Time between spawns
    public float screenWidth = 10f; // Adjust based on your camera width
    public float screenHeight = 5f; // Adjust based on your camera height

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnInterval); // Start spawning after 2 seconds
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        float randomX = Random.Range(-screenWidth / 2, screenWidth / 2);
        float spawnY = (Random.value > 0.5f) ? screenHeight / 2 + 1 : -screenHeight / 2 - 1; // Above or below screen

        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
