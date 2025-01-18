using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum EMobType
{
    rocket,
    shotgun,
    rifle
}

[System.Serializable]
public class Wave
{
    public SpawnGroup[] SpawnGroups;

    public int GetNumberOfMobsInWave()
    {
        if (SpawnGroups.Length == 0)
        {
            Debug.LogError("No SpawnGroups in wave, check the array in the inspector");
            Debug.Break();
            return 0;
        }

        int totalMobsInWave = 0;

        foreach (SpawnGroup spawnGroup in SpawnGroups)
        {
            totalMobsInWave += spawnGroup.GetNumberOfMobsInSpawnGroup();
        }

        return totalMobsInWave;
    }
}

[System.Serializable]
public class SpawnGroup
{
    public MobGroup[] mobGroups;
    public float spawnRadius;
    public Transform spawnPoint;

    public int GetNumberOfMobsInSpawnGroup()
    {
        int totalMobsInSpawnGroup = 0;

        if (mobGroups.Length == 0)
        {
            Debug.LogError("No mobGroups in spawnGroup, check the array in the inspector");
            Debug.Break();
            return 0;
        }

        foreach (MobGroup mobGroup in mobGroups)
        {
            totalMobsInSpawnGroup += mobGroup.amount;
        }

        return totalMobsInSpawnGroup;
    }
}

[System.Serializable]
public class MobGroup
{
    public EMobType type;
    public EMobGrade grade;
    public int amount;
}



public class MobSpawnerScript : MonoBehaviour
{
    public bool lastRoom = false;
    public MobPoolManager mobPoolManager;

    // specify waves through custom editor.
    public Wave[] waves;

    private int currentWave;
    public bool AllWavesCompleted;

    private int currentMobCount;

    private BuffSpawner buffSpawner;

    public bool spawnedBuffsThisWave;

    private void Awake()
    {
        buffSpawner = FindObjectOfType<BuffSpawner>();

    }

    private void Start()
    {
        spawnedBuffsThisWave = false;
        AllWavesCompleted = false;
        currentMobCount = waves[0].GetNumberOfMobsInWave();
        currentWave = 0;
    }

    private void Update()
    {
        if (currentMobCount == 0 && !spawnedBuffsThisWave)
        {
            Debug.Log("Spawning Buffs");
            buffSpawner.SpawnBuffs();
            spawnedBuffsThisWave=true;

            currentWave++;

            if (currentWave == waves.Length)
            {
                AllWavesCompleted = true;
                
                if (lastRoom)
                {
                    FindObjectOfType<GameStateManager>().GameComplete();
                }
            }
        }
    }

    public void ReduceMobCount()
    {
        currentMobCount--;
    }

    public void SpawnWave()
    {

        if (!AllWavesCompleted)
        {
            foreach (SpawnGroup spawnGroup in waves[currentWave].SpawnGroups)
            {
                SpawnSpawnGroup(spawnGroup);
            }

            currentMobCount = waves[currentWave].GetNumberOfMobsInWave();

            spawnedBuffsThisWave = false;
        }
    }

    private void SpawnSpawnGroup(SpawnGroup spawnGroup)
    {
        foreach (MobGroup mobGroup in spawnGroup.mobGroups)
        {
            for (int i = 0; i < mobGroup.amount; i++)
            {
                // Get a random point within a sphere (3D random distance)
                Vector2 randomOffset = Random.insideUnitCircle * spawnGroup.spawnRadius;

                Vector3 randomOffsetV3 = new Vector3(randomOffset.x, 0, randomOffset.y);

                // Calculate the final spawn position by adding the random offset to the spawn point
                Vector3 spawnPosition = spawnGroup.spawnPoint.position + randomOffsetV3;

                mobPoolManager.SpawnMob(
                    mobGroup.type,
                    mobGroup.grade,
                    1,
                    spawnPosition);
            }
        }
    }
}
