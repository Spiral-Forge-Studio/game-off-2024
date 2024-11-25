using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public int mobsPerWave = 5;             // Number of mobs to spawn per wave
    public int totalWaves = 3;              // Total number of waves
    public float timeBetweenWaves = 5f;     // Delay between waves in seconds
    public float spawnRadius = 5f;          // Radius around the spawner where mobs will appear
    public Transform[] spawnPoints;         // Optional spawn points; if none provided, spawns randomly within radius

    [Header("Mob Prefabs")]
    public List<MobVariant> rangedVariants; // List of ranged mob variants
    public List<MobVariant> meleeVariants;  // List of melee mob variants

    private int currentWave = 0;            // Tracks the current wave number
    private bool spawning = false;          // Prevents overlapping wave spawns

    [System.Serializable]
    public class MobVariant
    {
        public string name;                 // Name of the variant for clarity
        public GameObject prefab;           // Prefab of the variant
    }

    void Start()
    {
        if ((rangedVariants == null || rangedVariants.Count == 0) &&
            (meleeVariants == null || meleeVariants.Count == 0))
        {
            Debug.LogError("No mob variants assigned! Add ranged or melee variants in the Inspector.");
            enabled = false;
        }
    }

    public void StartSpawning()
    {
        if (!spawning)
        {
            StartCoroutine(SpawnWaves());
        }
    }

    private IEnumerator SpawnWaves()
    {
        spawning = true;

        while (currentWave < totalWaves)
        {
            currentWave++;
            Debug.Log($"Spawning Wave {currentWave}/{totalWaves}");

            for (int i = 0; i < mobsPerWave; i++)
            {
                SpawnMob();
                yield return new WaitForSeconds(0.5f); // Slight delay between individual spawns (optional)
            }

            if (currentWave < totalWaves)
                yield return new WaitForSeconds(timeBetweenWaves);
        }

        spawning = false;
        Debug.Log("All waves completed.");
    }

    private void SpawnMob()
    {
        Vector3 spawnPosition;

        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            // Spawn from one of the predefined points
            spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        }
        else
        {
            // Spawn randomly within a radius
            spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = transform.position.y; // Keep it on the same Y level as the spawner
        }

        GameObject mobToSpawn = ChooseMob();
        if (mobToSpawn != null)
        {
            Instantiate(mobToSpawn, spawnPosition, Quaternion.identity);
        }
    }

    private GameObject ChooseMob()
    {
        // Randomly decide between ranged and melee types
        bool isRanged = Random.value > 0.5f;

        // Choose a random variant from the selected type
        if (isRanged && rangedVariants.Count > 0)
        {
            return rangedVariants[Random.Range(0, rangedVariants.Count)].prefab;
        }
        else if (!isRanged && meleeVariants.Count > 0)
        {
            return meleeVariants[Random.Range(0, meleeVariants.Count)].prefab;
        }

        Debug.LogWarning("No suitable mob variant found!");
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the spawn radius for visual debugging
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
