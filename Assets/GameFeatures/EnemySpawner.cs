using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Enemy prefab
    public Transform playerTarget; // Player's Transform
    public float spawnY = 8f; // Off-screen spawn Y position
    public float minX = -8f, maxX = 8f; // X boundaries for spawning enemies
    public float moveInDelay = 1f; // Delay before enemies move onto the screen
    public float respawnDelay = 2f; // Delay before respawning the next wave

    private List<GameObject> activeEnemies = new List<GameObject>(); // Tracks active enemies
    private int currentWave = 0; // Tracks the current wave
    private bool isRespawning = false; // Ensures only one wave is spawned at a time

    void Start()
    {
        // Start with no enemies on screen
        Invoke(nameof(SpawnWave), respawnDelay);
    }

    private void Update()
    {
        // Check for null enemies and remove them
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] == null)
            {
                activeEnemies.RemoveAt(i);
            }
        }

        // Check if all enemies are defeated
        if (activeEnemies.Count <= 0 && !isRespawning)
        {
            isRespawning = true; // Prevent additional wave spawns during respawn
            Invoke(nameof(SpawnWave), respawnDelay);
        }
    }

private void SpawnWave()
{
    currentWave++; // Increment the wave number

    int enemyCount = 0;

    // Define the number of enemies per wave
    switch (currentWave)
    {
        case 1:
            enemyCount = 2; // Wave 1: Spawn 2 enemies
            break;
        case 2:
            enemyCount = 3; // Wave 2: Spawn 3 enemies
            break;
        case 3:
            enemyCount = 1; // Wave 3: Spawn 1 enemy
            break;
        default:
            enemyCount = 1; // Default additional waves spawn 1 enemy
            break;
    }

    // Start a coroutine to spawn enemies with a delay
    StartCoroutine(SpawnEnemiesWithDelay(enemyCount));
}

private IEnumerator SpawnEnemiesWithDelay(int enemyCount)
{
    for (int i = 0; i < enemyCount; i++)
    {
        SpawnEnemy(); // Spawn a single enemy
        yield return new WaitForSeconds(0.5f); // Wait 0.5 seconds before spawning the next enemy
    }
}

    private void SpawnEnemy()
{
    float spawnX = Random.Range(minX, maxX); // Random X position off-screen

    // Instantiate the enemy off-screen
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
        enemyScript.target = playerTarget; // Set the target for the enemy immediately
        enemyScript.OnDeath += HandleEnemyDeath; // Attach callback for enemy death
        StartCoroutine(MoveEnemyOntoScreen(enemy)); // Move the enemy onto the screen
    }
    else
    {
        Debug.LogError("Enemy prefab is missing the Enemy script!");
    }

    activeEnemies.Add(enemy); // Add to active enemies list
}


    private IEnumerator MoveEnemyOntoScreen(GameObject enemy)
    {
        yield return new WaitForSeconds(moveInDelay); // Delay before moving onto the screen

        // Gradually move enemy onto the screen
        Vector3 startPosition = enemy.transform.position;
        Vector3 targetPosition = new Vector3(startPosition.x, spawnY - 6f, startPosition.z); // Move down to visible area

        float elapsedTime = 0f;
        float duration = 1f; // Time to move onto the screen
        while (elapsedTime < duration)
        {
            enemy.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enemy.transform.position = targetPosition; // Ensure final position is accurate
    }

    private void HandleEnemyDeath()
    {
        // No need to manually decrement count; Update() removes null objects
    }
}
