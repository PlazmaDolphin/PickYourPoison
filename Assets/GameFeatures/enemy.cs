using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public float speed = 2f; // Movement speed
    public Transform target; // Player's Transform
    public float stopDistance = 3f; // Distance at which enemies stop
    public float separationRadius = 1f; // Minimum distance between enemies to avoid overlap
    public int enemyHp = 3; // Number of hits required to defeat the enemy
    public event Action OnDeath; // Event triggered when the enemy dies
    public EnemyHeartScript heartDisplay; // Reference to the heart script for enemy health UI

    private static List<Enemy> allEnemies = new List<Enemy>(); // Shared list of all enemies
    private bool isPunching = false; // Prevents multiple punch attempts
    public Animator animator; // Animator for enemy
    private bool isFlipped = false; // Check if the enemy is flipped

    void OnEnable()
    {
        allEnemies.Add(this); // Add enemy to global list when enabled
        if (heartDisplay != null)
        {
            heartDisplay.hp = enemyHp; // Initialize heart display with enemy health
            heartDisplay.updateHeartSprite(); // Update the heart display
        }
    }

    void OnDisable() => allEnemies.Remove(this); // Remove enemy from global list when disabled

    void Update()
    {
        if (target != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, target.position);

            if (distanceToPlayer > stopDistance)
            {
                MoveTowardsPlayerWithSeparation(); // Continue following the player
            }
            else if (!isPunching)
            {
                StartCoroutine(PunchPlayer()); // Punch when close to the player
            }
                    //face towards player
            Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
            if (direction.x < 0 && !isFlipped)
            {
                isFlipped = true;
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x > 0 && isFlipped)
            {
                isFlipped = false;
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }

    }

    private void MoveTowardsPlayerWithSeparation()
    {
        Vector2 position = transform.position;
        Vector2 moveDirection = ((Vector2)target.position - position).normalized;
        Vector2 separationForce = Vector2.zero;

        foreach (Enemy otherEnemy in allEnemies)
        {
            if (otherEnemy != this)
            {
                float distance = Vector2.Distance(position, otherEnemy.transform.position);
                if (distance < separationRadius)
                {
                    separationForce += (position - (Vector2)otherEnemy.transform.position).normalized / distance;
                }
            }
        }

        Vector2 finalMove = (moveDirection + separationForce).normalized;
        transform.position += (Vector3)(finalMove * speed * Time.deltaTime);
    }

    private IEnumerator PunchPlayer()
    {
        isPunching = true;

        animator.SetTrigger("clobber"); // Trigger punch animation
        yield return new WaitForSeconds(0.3f); // Wait before attacking
        animator.SetTrigger("clobber2"); // Trigger second punch animation
        if (target != null && Vector2.Distance(transform.position, target.position) <= stopDistance*1.4f)
        {
            Debug.Log("Enemy punches the player!");
            target.GetComponent<PlayerMovement>()?.damagePlayer(1); // Damage the player
        }

        yield return new WaitForSeconds(0.5f); // Wait for punch animation to finish
        animator.SetTrigger("clobber2");
        isPunching = false; // Reset punching state
    }

    public void TakeDamage(int damage = 1)
    {
        enemyHp -= damage; // Decrease enemy health
        Debug.Log($"Enemy took damage! Remaining HP: {enemyHp}");

        if (heartDisplay != null)
        {
            heartDisplay.hp = enemyHp; // Update heart display with new health
            heartDisplay.updateHeartSprite(); // Update the enemy's heart display
        }

        if (enemyHp <= 0)
        {
            Die(); // Trigger death when health reaches 0
        }
    }

    private void Die()
    {
        Debug.Log("Enemy defeated!");
        OnDeath?.Invoke(); // Notify listeners
        Destroy(gameObject); // Remove enemy from scene
    }
}