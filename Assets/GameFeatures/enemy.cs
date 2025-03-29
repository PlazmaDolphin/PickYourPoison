using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed = 2f; // Movement speed
    public Transform target; // Player's Transform
    public heartScript theHearts; // Reference to heartScript for health updates
    public Animator animator; // Animator for enemy
    private float stopDistance = 1f, hitRadius = 1.5f, throwDistanceMax = 7f, throwDistanceMin = 3f; 
    private float punchDelay = 0.5f;
    public event Action OnDeath; // When enemy dies?
    public bool rushing = true; // If the enemy is rushing
    private float separationRadius = 0.5f; // Minimum distance between enemies to avoid overlap
    private bool isPunching = false; // Prevent multiple punch coroutines
    private bool flipped = false; // For sprite flipping
    private bool hasReachedPlayer = false; // Check if the enemy has reached the player

    // Reference to all enemies (shared by the spawner)
    private static List<Enemy> allEnemies = new List<Enemy>();

    private void OnEnable()
    {
        // Add this enemy to the global list when it spawns
        allEnemies.Add(this);
    }

    private void OnDisable()
    {
        // Remove this enemy from the global list when it is destroyed
        allEnemies.Remove(this);
    }

    void Update()
    {
        if (target != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, target.position);

            if (distanceToPlayer > stopDistance)
            {
                // Move toward the player while avoiding other enemies
                MoveTowardsTargetWithSeparation();
            }
            else
            {
                // Start punching if within range
                if (!isPunching)
                {
                    // Move towards the player
                    transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                }
                else if (!isPunching)
                {
                    // If the enemy reaches the player and isn't punching, start punching
                    hasReachedPlayer = true;
                    StartCoroutine(PunchAfterDelay());
                }
            }
            /*
                if (distanceToPlayer > throwDistanceMax)
                {
                    // Move towards the player
                    transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                }
                else if (distanceToPlayer < throwDistanceMin && !isPunching)
                {
                    // If the enemy is close enough to the player, start punching
                    //hasReachedPlayer = true;
                    //StartCoroutine(ThrowAfterDelay());
                }
                else{
                    // Back up
                    transform.position = Vector2.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);
                }
            }*/
            // Flip sprite based on target position
            if (target.position.x < transform.position.x && !flipped ||
                target.position.x > transform.position.x && flipped)
            {
                flipped = !flipped;
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }

            // Flip sprite to face the player
            FlipSprite();
        }
    }

    private void MoveTowardsTargetWithSeparation()
    {
        Vector2 position = transform.position;
        Vector2 targetDirection = ((Vector2)target.position - position).normalized;
        Vector2 separationForce = Vector2.zero;

        // Separation logic: Avoid overlapping with other enemies
        foreach (Enemy otherEnemy in allEnemies)
        {
            if (otherEnemy != this) // Don't check self
            {
                float distance = Vector2.Distance(position, otherEnemy.transform.position);
                if (distance < separationRadius)
                {
                    // Calculate avoidance force
                    Vector2 avoidDirection = (position - (Vector2)otherEnemy.transform.position).normalized;
                    separationForce += avoidDirection / distance; // Stronger force when closer
                }
            }
        }

        // Combine movement toward player with separation force
        Vector2 combinedForce = (targetDirection + separationForce).normalized;

        // Apply the movement
        transform.position += (Vector3)(combinedForce * speed * Time.deltaTime);
    }

    private void FlipSprite()
    {
        // Flip sprite based on the target's position
        if (target.position.x < transform.position.x && !flipped ||
            target.position.x > transform.position.x && flipped)
        {
            flipped = !flipped;
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    private IEnumerator PunchAfterDelay()
    {
        Debug.Log("Enemy is preparing to punch!");
        animator.SetTrigger("clobber");
        isPunching = true; // Prevent multiple coroutines from starting
        //Do windup here
        yield return new WaitForSeconds(punchDelay);
        animator.SetTrigger("clobber2");
        if (target.CompareTag("Player") && Vector2.Distance(transform.position, target.position) <= hitRadius)
        {
            Debug.Log("Enemy punches the player!");
            target.GetComponent<PlayerMovement>().damagePlayer(1); // Call the player's damage method
        }

        // Allow time for the punch animation to finish
        yield return new WaitForSeconds(0.5f); // Adjust duration based on animation length
        animator.SetTrigger("clobber2");
        isPunching = false;
        hasReachedPlayer = false; 
    }

    public void TakeDamage(int damageAmount = 1)
    {
        theHearts.hp -= damageAmount; // Update the health
        theHearts.updateHeartSprite(); // Update the heart display
        if (theHearts.hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy defeated!");

        OnDeath?.Invoke(); // Notify listeners (like the spawner)
        Destroy(gameObject); // Remove the enemy from the scene
    }
}
