using System.Collections.Generic;
using UnityEngine;

public class fireWall : MonoBehaviour
{
    public float direction = 0f; // Direction of the fireball in radians
    public const float SPEED = 10f; // Speed of the fireball
    private float initTime;
    private const float LIFESPAN = 1f;
    private List<Collider2D> colliders = new List<Collider2D>(); // List to store colliders

    void Start()
    {
        initTime = Time.time; // Store the initial time
        // Set the rotation of the fireball based on its direction
        transform.rotation = Quaternion.Euler(0, 0, direction * Mathf.Rad2Deg + 90);
    }

    void Update()
    {
        // Move the fireball in the given direction
        transform.position += new Vector3(SPEED * Mathf.Cos(direction), SPEED * Mathf.Sin(direction), 0) * Time.deltaTime;
        if(Time.time - initTime > LIFESPAN)
        {
            Destroy(gameObject); // Destroy the fireball after its lifespan
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the fireball collided with an enemy (and not the same one multiple times)
        if (collision.CompareTag("Enemy") && !colliders.Contains(collision))
        {
            Debug.Log("Successful Fireball Throw: " + collision.name);
            // Call TakeDamage on the enemy script
            Enemy enemy = collision.GetComponent<Enemy>();
            colliders.Add(collision); // Add the collider to the list
            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }
            //If enemy is dead, remove from colliders list
            if (enemy == null)
            {
                colliders.Remove(collision); // Remove the collider from the list
            }
        }
    }
}
