using UnityEngine;


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
    public MobPoolManager mobPoolManager;
    public Wave[] waves;

    private bool tested;
    private void Awake()
    {
        tested = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && !tested)
        {
            tested = true;

            foreach (Wave wave in waves)
            {
                foreach (SpawnGroup spawnGroup in wave.SpawnGroups)
                {
                    foreach (MobGroup mobGroup in spawnGroup.mobGroups)
                    {
                        for (int i = 0; i < mobGroup.amount; i++)
                        {
                            // Get a random point within a sphere (3D random distance)
                            Vector3 randomOffset = Random.insideUnitSphere * spawnGroup.spawnRadius;

                            randomOffset = new Vector3(randomOffset.x, 0, randomOffset.z);

                            // Calculate the final spawn position by adding the random offset to the spawn point
                            Vector3 spawnPosition = spawnGroup.spawnPoint.position + randomOffset;

                            mobPoolManager.SpawnMob(
                                mobGroup.type,
                                mobGroup.grade,
                                mobGroup.amount,
                                spawnPosition);
                        }
                    }
                }
            }
        }
    }
}
