using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 5;
    public const int WEAKNESS = 1; // Fire? Lightning? Ice?
    public float speed = 2f; // Movement speed
    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        // Find the player using the "Player" tag
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the Player GameObject has the 'Player' tag.");
        }

        // Get Rigidbody2D component to apply velocity
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found! Make sure the Enemy GameObject has a Rigidbody2D attached.");
        }
    }

    void Update()
    {
        if (player == null) return; // Ensure player exists

        float stoppingDistance = 0.3f; // Distance where the enemy stops moving
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stoppingDistance)
        {
            // Calculate direction towards the player
            Vector3 direction = (player.position - transform.position).normalized;

            // Apply velocity towards the player using Rigidbody2D
            rb.linearVelocity = new Vector2(direction.x, direction.y) * speed;

            // Debug to check if movement is happening
            Debug.Log("Enemy moving towards player: " + direction);
        }
        else
        {
            // Stop moving when within stopping distance
            rb.linearVelocity = Vector2.zero;
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
