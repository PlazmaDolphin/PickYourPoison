using UnityEngine;
using System;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed = 2f; // Movement speed
    public Transform target; // Player's Transform
    public heartScript theHearts; // Reference to heartScript for health updates
    public Animator animator; // Animator for enemy

    private float stopDistance = 1f; // Distance at which the enemy stops punching
    private float hitRadius = 1.5f; // Radius for attack range
    private float punchDelay = 0.3f; // Delay before punching
    public event Action OnDeath; // Event triggered when the enemy dies

    private bool isPunching = false; // Prevents multiple punch coroutines
    private bool flipped = false; // For sprite flipping

    void Update()
    {
        if (target != null)
        {
            float distance = Vector2.Distance(transform.position, target.position);

            if (distance > stopDistance)
            {
                // Continue moving toward the player if out of range
                MoveTowardsTarget();
            }
            else
            {
                // Start punching if within range
                if (!isPunching)
                {
                    StartCoroutine(PunchAfterDelay());
                }
            }

            // Flip sprite to face the player
            FlipSprite();
        }
    }

    private void MoveTowardsTarget()
    {
        // Move toward the player's position
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
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
        isPunching = true; // Prevent multiple punch attempts
        Debug.Log("Enemy is preparing to punch!");
        yield return new WaitForSeconds(punchDelay);

        // Trigger punch animation and deal damage if the player is within the hit radius
        animator.SetTrigger("clobber");
        if (target.CompareTag("Player") && Vector2.Distance(transform.position, target.position) <= hitRadius)
        {
            Debug.Log("Enemy punches the player!");
            target.GetComponent<PlayerMovement>().damagePlayer(1); // Call the player's damage method
        }

        // Allow time for the punch animation to finish
        yield return new WaitForSeconds(0.5f); // Adjust duration based on animation length
        animator.SetTrigger("endClobber"); // Reset animation state
        isPunching = false; // Reset punching state
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
