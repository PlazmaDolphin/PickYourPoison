using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public event Action OnDeath; // Event to notify the spawner when the enemy dies

    public int health = 5; // Enemy health
    // Other variables and logic...

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

        // Notify listeners (e.g., the spawner)
        if (OnDeath != null)
        {
            OnDeath.Invoke();
        }

        // Destroy the enemy object
        Destroy(gameObject);
    }
}
