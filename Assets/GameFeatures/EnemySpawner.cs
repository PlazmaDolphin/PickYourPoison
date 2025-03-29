using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Enemy prefab
    public Transform playerTarget; // Player's Transform
    public float spawnY = 8f; // Off-screen spawn Y position
    public float minX = -8f, maxX = 8f; // X boundaries for spawning enemies
    public float spawnDelay = 0.5f; // Delay between spawning each enemy

    private List<GameObject> activeEnemies = new List<GameObject>(); // Tracks active enemies
    private int currentWave = 0; // Tracks the current wave
    private bool isSpawningWave = false; // Prevents multiple simultaneous wave spawns

    void Update()
    {
        // Check if all enemies are defeated
        if (activeEnemies.Count == 0 && !isSpawningWave)
        {
            StartCoroutine(SpawnWaveWithDelay()); // Trigger the next wave
        }
    }

    private IEnumerator SpawnWaveWithDelay()
    {
        isSpawningWave = true; // Prevent additional wave spawns
        currentWave++; // Increment the wave number
        int enemyCount = currentWave <= 3 ? currentWave + 1 : 1; // Number of enemies per wave

        // Spawn enemies with a delay between each
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnDelay); // Wait between spawning each enemy
        }

        isSpawningWave = false; // Reset spawning control
    }

    private void SpawnEnemy()
    {
        float spawnX = Random.Range(minX, maxX); // Random X position off-screen
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(spawnX, spawnY, 0), Quaternion.identity);

        if (enemy == null)
        {
            Debug.LogError("Failed to instantiate enemyPrefab!");
            return;
        }

        // Assign the player's Transform as the enemy's target
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.target = playerTarget; // Set the target for the enemy
            enemyScript.OnDeath += () => activeEnemies.Remove(enemy); // Remove from active enemies on death
            activeEnemies.Add(enemy); // Track this enemy
        }
        else
        {
            Debug.LogError("Enemy prefab is missing the Enemy script!");
        }
    }

    private IEnumerator MoveEnemyOntoScreen(GameObject enemy)
{
    Vector3 startPosition = enemy.transform.position;
    Vector3 targetPosition = new Vector3(startPosition.x, spawnY - 6f, startPosition.z); // Move down to the visible area

    float elapsedTime = 0f;
    float duration = 1f; // Time to move onto the screen

    while (elapsedTime < duration)
    {
        enemy.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
        elapsedTime += Time.deltaTime;
        yield return null;
    }

    // Ensure final position is accurate
    enemy.transform.position = targetPosition;
}

}
