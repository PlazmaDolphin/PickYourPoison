using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public Transform target;
    public float stopDistance = 0.5f;
    public float punchDelay = 1f;

    private int health = 5; // Enemy health
    private bool hasReachedPlayer = false;

    void Start()
    {
        // Dynamically assign target (player)
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
            else
            {
                hasReachedPlayer = true;
                StartCoroutine(PunchAfterDelay());
            }
        }
    }

    private IEnumerator PunchAfterDelay()
    {
        yield return new WaitForSeconds(punchDelay);

        if (target != null)
        {
            Debug.Log("Enemy punches the player!");
            // Add logic to reduce player's health if needed
        }

        hasReachedPlayer = false;
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
        Destroy(gameObject); // Remove the enemy from the game
    }
}