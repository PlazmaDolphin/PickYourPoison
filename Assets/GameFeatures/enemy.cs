using UnityEngine;

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

        if (movementBounds == null)
        {
            Debug.LogError("Movement Bounds not set! Please assign a BoxCollider.");
            return;
        }
    if (player == null) Debug.LogError("Player not found! Make sure the Player GameObject has the 'Player' tag.");

        // Calculate the min and max Y bounds based on the BoxCollider
        minBounds = new Vector3(transform.position.x, movementBounds.bounds.min.y, transform.position.z);
        maxBounds = new Vector3(transform.position.x, movementBounds.bounds.max.y, transform.position.z);
    }

void Update()
{
    if (player == null) return; // Ensure player is assigned

    // Move towards the player
    Vector3 direction = (player.position - transform.position).normalized;
    transform.position += direction * speed * Time.deltaTime;
}



    public static void SpawnEnemy(GameObject enemyPrefab, float screenWidth, float screenHeight)
    {
        float randomX = Random.Range(-screenWidth / 2, screenWidth / 2);
        float spawnY = (Random.value > 0.5f) ? screenHeight / 2 + 1 : -screenHeight / 2 - 1; // Above or below screen

        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
