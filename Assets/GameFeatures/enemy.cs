using UnityEngine;
using System;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public Transform target;
    public heartScript theHearts;
    public Animator animator; // Animator for enemy
    private float stopDistance = 1f, hitRadius = 1.5f; 
    private float punchDelay = 0.3f;
    public event Action OnDeath; // When enemy dies?

    private bool hasReachedPlayer = false; 
    private bool isPunching = false; 
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
        animator.SetTrigger("clobber");
        if (target.CompareTag("Player") && Vector2.Distance(transform.position, target.position) <= hitRadius)
        {
            // Trigger the punch animation
            Debug.Log("Enemy punches the player!");
            target.GetComponent<PlayerMovement>().damagePlayer(1);
        }
        // Wait for the punch animation to finish (assuming it's 0.5 seconds long)
        yield return new WaitForSeconds(0.5f); // Adjust duration based on animation length
        animator.SetTrigger("endClobber");
        isPunching = false;
        hasReachedPlayer = false; 
    }

    public void TakeDamage()
    {
        theHearts.hp--;
        theHearts.updateHeartSprite();
        if (theHearts.hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
