using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobSpawnerLevels : MonoBehaviour
{
    [Header("Spawner Settings")]
    public int totalWaves = 3;                  // Total number of waves
    public float timeBetweenWaves = 5f;         // Delay between waves (after all enemies are defeated)
    public float timeBetweenSpawns = 0.2f;      // Delay between individual spawns
    public bool enableRandomSpawn = false;      // Toggle for random spawning on the NavMesh

    [Header("Random Spawn Settings")]
    public Vector3 randomSpawnCenter;           // Center point for random spawn area
    public float randomSpawnRadius = 20f;       // Radius of the random spawn area

    [Header("Mob Prefabs")]
    public List<MobVariant> rangedVariants;     // List of ranged mob variants
    public List<MobVariant> meleeVariants;      // List of melee mob variants

    [Header("Wave Settings")]
    public List<WaveConfig> waves;              // Configuration for each wave

    [Header("UI Integration")]
    public BuffSelectionUI buffSelectionUI; // Reference to the buff selection UI

    [Header("Player Reference")]
    public Transform player; // Reference to the player's transform
    public Transform nextRoomPosition; // The position of the next room

    private int currentWave = 0;                // Tracks the current wave number
    private bool spawning = false;              // Prevents overlapping wave spawns
    private List<GameObject> activeEnemies = new List<GameObject>(); // Tracks active enemies

    public GameObject buffSelectionPrefab; // Assign this prefab in the Inspector

    private GameObject buffSelectionUIInstance;

    public DungeonGenerator dungeonGenerator; // Replace RoomGeneration with your actual room generation script's class name



    [System.Serializable]
    public class MobVariant
    {
        public string name;                     // Name of the variant for clarity
        public GameObject prefab;               // Prefab of the variant
    }

    [System.Serializable]
    public class WaveConfig
    {
        public string waveName;                 // Optional name for the wave
        public List<SpawnConfig> spawnConfigs;  // Configuration of spawn points and variants for this wave
    }

    [System.Serializable]
    public class SpawnConfig
    {
        public MobVariant variant;              // The mob variant to spawn
        public Transform spawnPoint;            // The specific spawn point for this variant
        public int mobCount = 1;                // Number of mobs to spawn at this spawn point
    }

    void Start()
    {
        StartSpawning();
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

        while (currentWave < totalWaves && currentWave < waves.Count)
        {
            WaveConfig currentWaveConfig = waves[currentWave];
            currentWave++;

            Debug.Log($"[MobSpawner] Starting Wave {currentWave}/{totalWaves}: {currentWaveConfig.waveName}");

            // Spawn all mobs for this wave
            foreach (SpawnConfig spawnConfig in currentWaveConfig.spawnConfigs)
            {
                for (int i = 0; i < spawnConfig.mobCount; i++)
                {
                    SpawnMob(spawnConfig);
                    yield return new WaitForSeconds(timeBetweenSpawns); // Delay between individual spawns
                }
            }

            // Wait until all enemies are defeated, with a safety timeout
            float elapsedTime = 0f;
            while (activeEnemies.Count > 0 && elapsedTime < 30f) // Check for a maximum of 30 seconds
            {
                Debug.Log($"[MobSpawner] Waiting for all enemies to be defeated. Active enemies: {activeEnemies.Count}");
                yield return new WaitForSeconds(1f);
                elapsedTime += 1f;
            }

            if (activeEnemies.Count > 0)
            {
                Debug.LogWarning("[MobSpawner] Timeout reached, but some enemies are still active. Proceeding to the next wave.");
            }
            else
            {
                Debug.Log($"[MobSpawner] All enemies defeated. Starting next wave in {timeBetweenWaves} seconds.");
            }

            // Wait before starting the next wave
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        spawning = false;
        Debug.Log("[MobSpawner] All waves completed.");
    }

    private IEnumerator WaitForEnemiesToBeDefeated()
    {
        float elapsedTime = 0f;
        while (activeEnemies.Count > 0 && elapsedTime < 30f) // Check for a maximum of 30 seconds
        {
            Debug.Log($"[MobSpawner] Waiting for all enemies to be defeated. Active enemies: {activeEnemies.Count}");
            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
        }

        if (activeEnemies.Count > 0)
        {
            Debug.LogWarning("[MobSpawner] Timeout reached, but some enemies are still active.");
        }
    }

    private void SpawnMob(SpawnConfig spawnConfig)
    {
        Vector3 spawnPosition;

        if (enableRandomSpawn)
        {
            // Generate a random point on the NavMesh
            spawnPosition = GetRandomNavMeshPoint(randomSpawnCenter, randomSpawnRadius);
            if (spawnPosition == Vector3.zero) return; // No valid NavMesh point found
        }
        else
        {
            // Use predefined spawn points
            if (spawnConfig.spawnPoint == null || spawnConfig.variant == null)
            {
                Debug.LogWarning($"[MobSpawner] Missing variant or spawn point for spawn config. Skipping spawn.");
                return;
            }
            spawnPosition = spawnConfig.spawnPoint.position;
        }

        // Spawn the mob and track it in activeEnemies
        GameObject spawnedMob = Instantiate(spawnConfig.variant.prefab, spawnPosition, Quaternion.identity);
        activeEnemies.Add(spawnedMob);

        // Remove from activeEnemies when destroyed
        spawnedMob.GetComponent<EnemyTracker>().onDestroyed += () => activeEnemies.Remove(spawnedMob);

        Debug.Log($"[MobSpawner] Spawned {spawnConfig.variant.name} at {spawnPosition}");
    }

    private Vector3 GetRandomNavMeshPoint(Vector3 center, float radius)
    {
        for (int i = 0; i < 10; i++) // Try up to 10 times to find a valid NavMesh point
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * radius;
            randomPoint.y = center.y; // Keep the same height
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        Debug.LogWarning("[MobSpawner] Could not find a valid random NavMesh point.");
        return Vector3.zero;
    }

    private void ShowBuffSelectionUI()
    {
        if (buffSelectionPrefab != null)
        {
            // Instantiate the UI prefab
            buffSelectionUIInstance = Instantiate(buffSelectionPrefab, transform);

            // Initialize the BuffSelectionUI
            BuffSelectionUI buffUI = buffSelectionUIInstance.GetComponent<BuffSelectionUI>();
            if (buffUI != null)
            {
                buffUI.DisplayBuffOptions(ApplySelectedBuffAndTeleport);
            }
        }
        else
        {
            Debug.LogWarning("[MobSpawner] Buff selection UI prefab is not assigned!");
        }
    }

    private void ApplySelectedBuffAndTeleport(int buffIndex)
    {
        Debug.Log($"Buff {buffIndex} selected and applied!");

        // Destroy the BuffSelection UI
        if (buffSelectionUIInstance != null)
        {
            Destroy(buffSelectionUIInstance);
        }

        // Delegate teleportation logic to the dungeonGenerator
        if (dungeonGenerator != null)
        {
            dungeonGenerator.TeleportPlayerToNextRoom();
        }
        else
        {
            Debug.LogWarning("Dungeon Generator is not assigned in MobSpawnerLevels!");
        }
    }



    private void ApplyBuff(int buffIndex)
    {
        switch (buffIndex)
        {
            case 0:
                Debug.Log("[Buff] Player chose Buff 1: Increased Health!");
                break;

            case 1:
                Debug.Log("[Buff] Player chose Buff 2: Increased Damage!");
                break;

            default:
                Debug.LogWarning("[Buff] Invalid buff index selected.");
                break;
        }
    }

    private void TeleportToNextRoom()
    {
        if (player != null && nextRoomPosition != null)
        {
            player.position = nextRoomPosition.position;
            Debug.Log("[MobSpawner] Teleported player to the next room!");
        }
        else
        {
            Debug.LogWarning("[MobSpawner] Player or next room position is not assigned!");
        }
    }

    private void OnDrawGizmos()
    {
        if (enableRandomSpawn)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(randomSpawnCenter, randomSpawnRadius);
        }

        if (waves == null) return;

        foreach (var wave in waves)
        {
            foreach (var spawnConfig in wave.spawnConfigs)
            {
                if (spawnConfig.spawnPoint != null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(spawnConfig.spawnPoint.position, 0.5f);
                }
            }
        }
    }
}