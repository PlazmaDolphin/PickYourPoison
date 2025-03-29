using UnityEngine;

public class iceSpike : MonoBehaviour
{
    private bool gotHit = false; // Flag to check if used
    private bool detecting = false;
    private float detectAt = 0.2f;
    private float createTime;
    private const float LIFETIME = 0.6f;
    void Start()
    {
        createTime = Time.time;

    }

    void Update()
    {
        if(Time.time - createTime > detectAt){
            detecting = true;
        }
        if(Time.time - createTime > LIFETIME)
        {
            Destroy(gameObject); // Destroy the fireball after its lifespan
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (gotHit || !detecting){return;}
        //Can only hit 1 enemy for 2 damage
        // Check if the fireball collided with an enemy
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Successful Ice Spike: " + collision.name);
            // Call TakeDamage on the enemy script
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(2);
            }
            gotHit = true;

        }
    }
}
