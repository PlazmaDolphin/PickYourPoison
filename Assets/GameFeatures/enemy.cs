using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 5;
    public const int WEAKNESS = 1; // Fire? Lightning? Ice?
    public float speed = 2f; // Movement speed
    private Transform player;

    void Start()
    {
        // Find the player using the "Player" tag
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the Player GameObject has the 'Player' tag.");
        }
    }

void Update()
{
    if (player == null) return; // Ensure player exists

    float stoppingDistance = 0.3f; // Stop before hitting the player
    float distance = Vector3.Distance(transform.position, player.position);

    if (distance > stoppingDistance)
    {
        // Move towards the player
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Debug to check if movement is happening
        Debug.Log("Enemy moving towards player: " + direction);
    }
}


public static void SpawnEnemy(GameObject enemyPrefab)
{
    if (Camera.main == null) return;

    // Get screen bounds in world coordinates
    float screenWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
    float screenHeight = Camera.main.orthographicSize;

    // Pick a random X position within the screen width
    float randomX = Random.Range(-screenWidth, screenWidth);

    // Spawn above or below the screen
    float spawnY = (Random.value > 0.5f) ? screenHeight + 2 : -screenHeight - 2; 

    Vector3 spawnPosition = new Vector3(randomX, spawnY, 0);
    Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
}

}
