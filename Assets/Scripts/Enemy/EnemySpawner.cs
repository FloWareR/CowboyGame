using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyLightPrefab;
    [SerializeField] private List<Transform> spawnPointList;
    [SerializeField] private float waveTimeInterval = 5f;  // Time between waves
    [SerializeField] private int initialWaveSize = 10;     // Initial wave size
    [SerializeField] private int maxCorpsesAllowed = 1;    // Max number of corpses allowed at once
    [SerializeField] private List<GameObject> deathEnemies = new List<GameObject>();

    [SerializeField] private int deathCount = 0;

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
            // Wait for wave to finish (all enemies are dead)
            yield return new WaitUntil(() => !waveInProgress);

            // Start new wave
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
        int enemieCoolDown = 0;
        float spawnCoolDown = 0.5f;

        // Spawn enemies across the spawn points
        while (enemiesSpawned < waveSize)
        {
            int randomIndex = Random.Range(0, spawnPointList.Count);
            Instantiate(enemyLightPrefab, spawnPointList[randomIndex].transform.position, Quaternion.identity);
            enemiesSpawned++;
            enemieCoolDown++;

            // Wait 0.5 seconds between spawns
            if (enemieCoolDown == 10)
            {
                enemieCoolDown = 0;
                yield return new WaitForSeconds(spawnCoolDown);
            }
        }

        // Check if all enemies are dead to end the wave
        yield return new WaitUntil(() => deathCount == waveSize);

        // Wait for 5 seconds after each wave
        yield return new WaitForSeconds(waveTimeInterval);

        // Increase wave size by 15% for the next wave
        deathCount = 0;
        currentWaveSize = Mathf.CeilToInt(currentWaveSize * 2);
        waveInProgress = false;
        print(currentWaveSize);
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
            corpse.transform.position = new Vector3(originalPosition.x, Mathf.Lerp(originalPosition.y, originalPosition.y - 1f, elapsedTime / timeToBury), originalPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure it's fully buried
        corpse.transform.position = new Vector3(originalPosition.x, originalPosition.y - 1f, originalPosition.z);

        // Destroy the corpse after burial
        Destroy(corpse);
        deathCount++;
    }

}
