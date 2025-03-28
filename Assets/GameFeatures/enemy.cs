using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 5;
    public float speed = 2f; // Movement speed
    private Rigidbody2D rb;
    private Vector2 movementDirection;
    
    private float changeDirectionInterval = 1f; // How often to change direction (in seconds)
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on enemy!");
        }

        SetRandomDirection();
        timer = changeDirectionInterval;
    }

    void Update()
    {
        // Update timer and change direction when needed
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SetRandomDirection();
            timer = changeDirectionInterval;
        }
        
        // Apply velocity based on the current random direction
        rb.linearVelocity = movementDirection * speed;

        // Keep the enemy within the screen bounds (optional)
        Vector3 pos = transform.position;
        float camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float camHeight = Camera.main.orthographicSize;
        pos.x = Mathf.Clamp(pos.x, -camWidth, camWidth);
        pos.y = Mathf.Clamp(pos.y, -camHeight, camHeight);
        transform.position = pos;
    }

    void SetRandomDirection()
    {
        // Choose a random normalized direction vector
        movementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    // Static spawn method: spawns an enemy inside the screen bounds
    public static void SpawnEnemy(GameObject enemyPrefab)
    {
        if (Camera.main == null) return;

        float camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float camHeight = Camera.main.orthographicSize;
        
        // Random spawn position inside the camera's view
        float randomX = Random.Range(-camWidth, camWidth);
        float randomY = Random.Range(-camHeight, camHeight);

        Vector3 spawnPosition = new Vector3(randomX, randomY, 0);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
