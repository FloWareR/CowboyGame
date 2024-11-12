using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyLightPrefab;
    [SerializeField] private List<Transform> spawnPointList;
    [SerializeField] private float waveTimeInterval = 3.5f;   // Time between waves
    [SerializeField] private int initialWaveSize = 10;      // Initial wave size
    [SerializeField] private int maxCorpsesAllowed = 1;     // Max number of corpses allowed at once
    [SerializeField] private float spawnedPercentage = 20f; // Percentage of the wave to spawn at a time
    [SerializeField] private float spawnCooldown = 6.5f;    // Cooldown between enemy spawns
    [SerializeField] private List<GameObject> deathEnemies = new List<GameObject>();

    [SerializeField] public int deathCount = 0;

    private int waveCount = 0;
    private int currentWaveSize;
    private bool waveInProgress = false;

    void Start()
    {
        currentWaveSize = initialWaveSize;
        StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        while (true)
        {
            // Wait for the current wave to finish (all enemies are dead)
            yield return new WaitUntil(() => !waveInProgress);

            // Start a new wave
            waveInProgress = true;
            SpawnWave(currentWaveSize);
        }
    }

    private void SpawnWave(int waveSize)
    {
        StartCoroutine(SpawnEnemies(waveSize));
    }

    private IEnumerator SpawnEnemies(int waveSize)
    {
        int enemiesSpawned = 0;
        int enemiesToSpawnAtOnce = Mathf.CeilToInt(waveSize * (spawnedPercentage / 100f));
        
        while (enemiesSpawned < waveSize)
        {
            for (int i = 0; i < enemiesToSpawnAtOnce && enemiesSpawned < waveSize; i++)
            {
                // Spawn enemy at a random spawn point
                int randomIndex = Random.Range(0, spawnPointList.Count);
                Instantiate(enemyLightPrefab, spawnPointList[randomIndex].position, Quaternion.identity);
                enemiesSpawned++;

                // Add cooldown between each enemy spawn
                yield return new WaitForSeconds(spawnCooldown);
            }

            // Wait before spawning the next batch
            yield return new WaitForSeconds(spawnCooldown);
        }

        // Check if all enemies are dead to end the wave
        yield return new WaitUntil(() => waveCount == waveSize);

        // Wait for some time after each wave before starting the next
        yield return new WaitForSeconds(waveTimeInterval);

        // Increase wave size by 15% for the next wave
        waveCount = 0;
        currentWaveSize = Mathf.CeilToInt(currentWaveSize * 1.15f); // Increase wave size by 15%
        waveInProgress = false;
        print($"Next Wave Size: {currentWaveSize}");
    }

    public void RegisterDeath(GameObject deadEnemy)
    {
        deathEnemies.Add(deadEnemy);

        // If corpse limit is exceeded, bury the oldest corpse
        if (deathEnemies.Count > maxCorpsesAllowed)
        {
            GameObject corpse = deathEnemies[0];
            deathEnemies.RemoveAt(0);
            StartCoroutine(BuryCorpse(corpse));
        }
    }

    private IEnumerator BuryCorpse(GameObject corpse)
    {
        // Get all child objects of the corpse
        Collider[] colliders = corpse.GetComponentsInChildren<Collider>();
        Rigidbody[] rigidbodies = corpse.GetComponentsInChildren<Rigidbody>();

        // Disable colliders on all children
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        // Deactivate gravity on all rigidbodies
        foreach (var rb in rigidbodies)
        {
            rb.useGravity = false;
        }

        // Save the original position of the corpse
        Vector3 originalPosition = corpse.transform.position;

        // Lerp the Y position down to simulate burial
        float timeToBury = 3f; // Time for burial effect
        float elapsedTime = 0f;
        while (elapsedTime < timeToBury)
        {
            if (corpse == null) break;
            corpse.transform.position = new Vector3(
                originalPosition.x, 
                Mathf.Lerp(originalPosition.y, originalPosition.y - 1f, elapsedTime / timeToBury), 
                originalPosition.z
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (corpse == null) yield break;
        corpse.transform.position = new Vector3(originalPosition.x, originalPosition.y - 1f, originalPosition.z);

        // Destroy the corpse after burial
        Destroy(corpse);
        waveCount++;
        deathCount++;
    }
}
