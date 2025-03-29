using UnityEngine;
using System;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed = 2f; // Movement speed
    public Transform target; // Target to follow (usually the player)
    public heartScript theHearts;
    public Animator animator; // Animator for the enemy
    private float stopDistance = 1f, hitRadius = 1.5f; // Distance at which the enemy stops moving towards the target
    private float punchDelay = 0.3f; // Delay before punching the player
    public event Action OnDeath; // Event to notify the spawner when the enemy dies

    private bool hasReachedPlayer = false; // Check if the enemy has reached the player
    private bool isPunching = false; // Prevent multiple punch coroutines from running
    private bool flipped = false;

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
            // Flip sprite based on target position
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
        isPunching = true; // Prevent multiple coroutines from starting
        //Do windup here
        yield return new WaitForSeconds(punchDelay);
        //if tag is player and still in range, punch
        animator.SetTrigger("clobber");
        if (target.CompareTag("Player") && Vector2.Distance(transform.position, target.position) <= hitRadius)
        {
            // Trigger the punch animation
            Debug.Log("Enemy punches the player!");
            target.GetComponent<PlayerMovement>().damagePlayer(1); // Call the player's TakeDamage method
        }
        // Wait for the punch animation to finish (assuming it's 0.5 seconds long)
        yield return new WaitForSeconds(0.5f); // Adjust this duration based on your animation length
        animator.SetTrigger("endClobber"); // Return to idle animation
        // Allow the enemy to punch again if needed
        isPunching = false;
        hasReachedPlayer = false; // Reset to allow movement again
    }

    public void TakeDamage(int damageAmount=1)
    {
        theHearts.hp-=damageAmount; // Update heart script
        theHearts.updateHeartSprite(); // Update heart sprite
        if (theHearts.hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy defeated!");

        // Notify listeners (e.g., the spawner)
        OnDeath?.Invoke();

        // Destroy the enemy object
        Destroy(gameObject);
    }
}
