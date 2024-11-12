using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpawner : MonoBehaviour
{
    public GameObject buffPrefab;
    public Transform spawnPoint;  // This can serve as the center of the spawn area
    public float spawnInterval = 5f;
    public float spawnRadius = 5f; // Define the radius for random spawning
    public int buffCount = 3;      // Number of buffs to spawn
    public PlayerStatusSO playerStats;

    private List<GameObject> activeBuffs = new List<GameObject>();

    public void DestroyActiveBuff(GameObject buff)
    {
        Debug.Log(buff.name);
        if (activeBuffs.Contains(buff))
        {
            Debug.Log(buff.name);
            activeBuffs.Remove(buff);
            Destroy(buff);
        }
    }

    void Start()
    {
        StartCoroutine(SpawnBuffAtIntervals());
        playerStats.ResetMultipliersAndFlatBonuses();//MOVE THIS TO SOMEWHERE 
    }

    private IEnumerator SpawnBuffAtIntervals()
    {
        while (true)
        {
            // Spawn buffs only if the count is less than the defined number
            while (activeBuffs.Count < buffCount && buffPrefab != null)
            {
                Vector3 spawnPosition = spawnPoint.position + new Vector3(
                    Random.Range(-spawnRadius, spawnRadius),
                    0f,
                    Random.Range(-spawnRadius, spawnRadius)
                );

                GameObject newBuff = Instantiate(buffPrefab, spawnPosition, Quaternion.identity);
                activeBuffs.Add(newBuff);
            }

            yield return new WaitForSeconds(spawnInterval); // Wait for the next spawn


        }
    }
}