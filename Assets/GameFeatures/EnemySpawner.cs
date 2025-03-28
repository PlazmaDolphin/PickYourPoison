using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign your enemy prefab in the Inspector
    public float spawnInterval = 3f; // Time between spawns
    public float screenWidth = 10f; // Adjust based on your camera width
    public float screenHeight = 5f; // Adjust based on your camera height

    // The minimum and maximum X and Y coordinates for the boundary
    public float minX = -7.91f, maxX = 8.08f;
    public float minY = -4.96f, maxY = 2.51f;

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

        // Ensure the spawn position is outside the defined boundary box (above or below)
        float randomX = Random.Range(-screenWidth, screenWidth);

        // Check if random spawn position is within the Y boundary, adjust if needed
        float spawnY = (Random.value > 0.5f) ? (screenHeight + 2) : (-screenHeight - 2); 

        // If the spawnY is within the boundary box range, adjust accordingly
        if (spawnY > maxY)
        {
            spawnY = maxY + 2;  // Ensure spawn position is outside the upper boundary
        }
        else if (spawnY < minY)
        {
            spawnY = minY - 2;  // Ensure spawn position is outside the lower boundary
        }

        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity); // Instantiate enemy
    }
}
