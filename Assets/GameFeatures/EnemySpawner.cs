using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign your enemy prefab in the Inspector
    public float spawnInterval = 3f; // Time between spawns
    public float screenWidth = 10f; // Adjust based on your camera width
    public float screenHeight = 5f; // Adjust based on your camera height

    void Start()
    {
        // Start spawning after 2 seconds, repeat every spawnInterval
        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnInterval);
    }

    void SpawnEnemy()
    {
        // Ensure enemyPrefab is assigned
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned!");
            return;
        }

        // Get screen bounds in world coordinates
        float screenWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        float screenHeight = Camera.main.orthographicSize;

        // Pick a random X position within the screen width
        float randomX = Random.Range(-screenWidth, screenWidth);

        // Spawn above or below the screen
        float spawnY = (Random.value > 0.5f) ? screenHeight + 2 : -screenHeight - 2; 

        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity); // Instantiate enemy
    }
}
