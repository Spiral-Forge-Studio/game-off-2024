using System.Collections;
using UnityEngine;

public class BuffSpawner : MonoBehaviour
{
    public GameObject buffPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 5f;

    private GameObject activeBuff; 
    public PlayerStatusSO playerStats;
    void Start()
    {
        
        StartCoroutine(SpawnBuffAtIntervals());
    }

    private IEnumerator SpawnBuffAtIntervals()
    {
        while (true)
        {
            if (activeBuff == null && buffPrefab != null)
            {
                activeBuff = Instantiate(buffPrefab, spawnPoint.position, spawnPoint.rotation);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
