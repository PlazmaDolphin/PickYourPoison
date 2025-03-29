using UnityEngine;
using System;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed = 2f; // Movement speed
    public Transform target; // Player's Transform
    public heartScript theHearts; // Reference to heartScript for health updates
    public Animator animator; // Animator for enemy

    private float stopDistance = 1f; // Distance at which the enemy stops moving toward the player
    private float hitRadius = 1.5f; // Radius for attack range
    private float punchDelay = 0.3f; // Delay before punching
    public event Action OnDeath; // Event triggered when enemy dies

    private bool hasReachedPlayer = false; // To check if enemy reached the player
    private bool isPunching = false; // Prevent multiple punch coroutines
    private bool flipped = false; // For sprite flipping

    void Start()
    {
        // Ensure enemy immediately targets and moves toward the player
        if (target == null)
        {
            Debug.LogError("Target is not assigned to the enemy!");
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
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
            else if (!isPunching)
            {
                // If the enemy reaches the player and isn't punching, start punching
                hasReachedPlayer = true;
                StartCoroutine(PunchAfterDelay());
            }

            // Flip sprite based on the player's position
            if (target.position.x < transform.position.x && !flipped ||
                target.position.x > transform.position.x && flipped)
            {
                flipped = !flipped;
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private IEnumerator PunchAfterDelay()
    {
        Debug.Log("Enemy is preparing to punch!");  
        isPunching = true; // Prevent multiple punch attempts
        yield return new WaitForSeconds(punchDelay);

        animator.SetTrigger("clobber");

        // Check if player is still in attack range
        if (target.CompareTag("Player") && Vector2.Distance(transform.position, target.position) <= hitRadius)
        {
            Debug.Log("Enemy punches the player!");
            target.GetComponent<PlayerMovement>().damagePlayer(1); // Call player's damage method
        }

        // Allow punch animation to finish and reset punching state
        yield return new WaitForSeconds(0.5f); // Adjust duration based on animation
        animator.SetTrigger("endClobber"); // Reset animation state

        isPunching = false;
        hasReachedPlayer = false; // Reset movement towards the player
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

        OnDeath?.Invoke(); // Notify spawner or other listeners
        Destroy(gameObject); // Remove the enemy from the scene
    }
}
