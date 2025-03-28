using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public Transform target;
    public float stopDistance = 0.5f; // Distance to maintain from player
    public float punchDelay = 1f; // Delay before punching

    private bool hasReachedPlayer = false;

    private void Update()
    {
        if (target != null && !hasReachedPlayer)
        {
            // Move towards the player but stop at a certain distance
            float distance = Vector2.Distance(transform.position, target.position);
            if (distance > stopDistance)
            {
                Vector2 direction = (target.position - transform.position).normalized;
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
            else
            {
                // Stop movement when within the stopDistance
                hasReachedPlayer = true;
                StartCoroutine(PunchAfterDelay());
            }
        }
    }

    private IEnumerator PunchAfterDelay()
    {
        // Wait for the specified punch delay
        yield return new WaitForSeconds(punchDelay);

        // Perform the punch action (e.g., reduce player's health, play animation, etc.)
        if (target != null)
        {
            Debug.Log("Enemy punches the player!");
            // Add your punch logic here (e.g., reduce player's health)
        }

        // Reset behavior if needed, or make the enemy idle
        hasReachedPlayer = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy collided with player!");
        }
    }
}
