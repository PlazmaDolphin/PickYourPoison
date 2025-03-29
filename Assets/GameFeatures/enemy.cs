using UnityEngine;
using System;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public float speed = 2f; // Movement speed
    public Transform target; // Player's Transform
    public float stopDistance = 1.5f; // Distance to stop near the player
    public float separationRadius = 1f; // Distance to avoid overlapping with other enemies
    public event Action OnDeath; // Event triggered when enemy dies

    private static List<Enemy> allEnemies = new List<Enemy>(); // Shared list of all enemies
    private bool isPunching = false; // Prevents multiple punch attempts
    private Animator animator; // Animator for enemy

    void OnEnable() => allEnemies.Add(this); // Add to global list on spawn
    void OnDisable() => allEnemies.Remove(this); // Remove from global list on death

    void Update()
    {
        if (target != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, target.position);

            if (distanceToPlayer > stopDistance)
            {
                MoveTowardsPlayerWithSeparation();
            }
            else if (!isPunching)
            {
                StartCoroutine(PunchPlayer());
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
        animator.SetTrigger("clobber");

        yield return new WaitForSeconds(0.3f); // Delay for punching
        if (target != null && Vector2.Distance(transform.position, target.position) <= stopDistance)
        {
            Debug.Log("Enemy punches the player!");
            target.GetComponent<PlayerMovement>()?.damagePlayer(1); // Damage the player
        }

        yield return new WaitForSeconds(0.5f); // Allow punch animation to finish
        isPunching = false;
    }

    public void TakeDamage(int damage = 1)
    {
        // Trigger death if health is <= 0 (add logic for health here)
        Debug.Log("Enemy defeated!");
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
