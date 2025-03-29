using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed = 2f; // Movement speed
    public Transform target; // Target to follow (usually the player)
    public float stopDistance = 0.5f; // Distance at which the enemy stops moving towards the target
    public float punchDelay = 1f; // Delay before punching the player
    public int health = 3; // Enemy health
    public Transform healthBar; // The health bar UI element

    private bool hasReachedPlayer = false; // Check if the enemy has reached the player
    private bool isPunching = false; // Prevent multiple punch coroutines from running

    void Start()
    {
        // Dynamically assign target (player) if not set in the Inspector
        if (target == null && GameObject.FindGameObjectWithTag("Player") != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (target != null && !hasReachedPlayer)
        {
            float distance = Vector2.Distance(transform.position, target.position);

            if (distance > stopDistance)
            {
                // Move towards the player
                Vector2 direction = (target.position - transform.position).normalized;
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
            else if (!isPunching)
            {
                // If the enemy reaches the player and isn't punching, start punching
                hasReachedPlayer = true;
                StartCoroutine(PunchAfterDelay());
            }
        }

        // Update health bar UI scale
        if (healthBar != null)
        {
            healthBar.localScale = new Vector3(health / 5f, 1f, 1f); // Scale based on remaining health
        }
    }

    private IEnumerator PunchAfterDelay()
    {
        isPunching = true; // Prevent multiple coroutines from starting
        yield return new WaitForSeconds(punchDelay);

        if (target != null)
        {
            Debug.Log("Enemy punches the player!");
            // Add logic here to reduce the player's health or trigger an effect
        }

        // Allow the enemy to punch again if needed
        isPunching = false;
        hasReachedPlayer = false; // Reset to allow movement again
    }

    public void TakeDamage()
    {
        health--; // Reduce health by 1
        Debug.Log("Enemy hit! Remaining health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy defeated!");
        Destroy(gameObject); // Destroy the enemy object
    }
}
