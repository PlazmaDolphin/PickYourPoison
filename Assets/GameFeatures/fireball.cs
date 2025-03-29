using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float direction = 0f; // Direction of the fireball in radians
    private Vector2 initPos; // Initial position of the fireball
    public const float RANGE = 10f; // Maximum range the fireball can travel
    public const float SPEED = 15f; // Speed of the fireball

    void Start()
    {
        initPos = transform.position;
        // Set the rotation of the fireball based on its direction
        transform.rotation = Quaternion.Euler(0, 0, direction * Mathf.Rad2Deg);
    }

    void Update()
    {
        // Move the fireball in the given direction
        transform.position += new Vector3(SPEED * Mathf.Cos(direction), SPEED * Mathf.Sin(direction), 0) * Time.deltaTime;

        // Destroy the fireball if it goes out of bounds
        if (transform.position.x < -10 || transform.position.x > 10 || transform.position.y < -6 || transform.position.y > 6)
        {
            Destroy(gameObject);
            Debug.Log("Fireball out of bounds");
        }

        // Destroy the fireball if it exceeds its range limit
        if (Vector2.Distance(initPos, transform.position) > RANGE)
        {
            Destroy(gameObject);
            Debug.Log("Fireball reached range limit");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the fireball collided with an enemy
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Successful Fireball Throw: " + collision.name);
            // Call TakeDamage on the enemy script
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage();
            }

            // Destroy the fireball after hitting an enemy
            Destroy(gameObject);
        }
    }
}
