using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 5; // FIXME
    public const int WEAKNESS = 1; // Fire? Lightning? Ice?
    public BoxCollider movementBounds; // Assign a BoxCollider in the Inspector
    public float speed = 2f; // Movement speed
    private Vector3 minBounds, maxBounds;
    private bool movingUp = true;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // Finds the player if tagged correctly

        // Calculate the min and max Y bounds based on the BoxCollider
        minBounds = new Vector3(transform.position.x, movementBounds.bounds.min.y, transform.position.z);
        maxBounds = new Vector3(transform.position.x, movementBounds.bounds.max.y, transform.position.z);
    }

void Update()
{
    if (player == null) return;

    float stoppingDistance = 1.0f; // Distance where the enemy stops moving

    // Get direction to player
    Vector3 direction = (player.position - transform.position).normalized;
    float distance = Vector3.Distance(player.position, transform.position);

    // Move only if not within stopping distance
    if (distance > stoppingDistance)
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}




    public static void SpawnEnemy(GameObject enemyPrefab, float screenWidth, float screenHeight)
    {
        float randomX = Random.Range(-screenWidth / 2, screenWidth / 2);
        float spawnY = (Random.value > 0.5f) ? screenHeight / 2 + 1 : -screenHeight / 2 - 1; // Above or below screen

        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
