using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuffSpawner : MonoBehaviour
{
    public GameObject buffPrefab;
    public Transform spawnPoint;  // This can serve as the center of the spawn area
    public float spawnInterval = 5f;
    public float spawnRadius = 0.5f; // Define the radius for random spawning
    public int buffCount = 3;      // Number of buffs to spawn
    public PlayerStatusSO playerStats;
    public PlayerStatusSO BossStatusSO;
    private List<GameObject> activeBuffs = new List<GameObject>();

    public void DestroyActiveBuff(GameObject buff)
    {
        //Debug.Log(buff.name);
        if (activeBuffs.Contains(buff))
        {
            Debug.Log("Removed " + buff.name);
            activeBuffs.Remove(buff);
            Destroy(buff);
            ApplyRandomBufftoBoss();
        }
        
    }

    void Start()
    {
        //StartCoroutine(SpawnBuffAtIntervals());
        playerStats.ResetMultipliersAndFlatBonuses();//MOVE THIS TO SOMEWHERE 
        SpawnBuffs();
    }

    private void Update()
    {
        //Debug.Log("There are " + activeBuffs.Count + " active buffs");
    }

    private void SpawnBuffs()
    {
        float timestart = Time.time;
        while (activeBuffs.Count < buffCount && buffPrefab != null)
        {
            Vector3 spawnPosition = spawnPoint.position + new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                1f,
                Random.Range(-spawnRadius, spawnRadius)
            );

            Collider[] colliders = Physics.OverlapSphere(spawnPosition, 5);
            bool validPos = true;

            if (colliders.Length != 0)
            {
                foreach (Collider collided in colliders)
                {
                    if (collided.gameObject.CompareTag("Player") || collided.gameObject.CompareTag("Buff"))
                    {
                        validPos = false;
                        break;
                    }
                }
            }

            if (validPos)
            {
                GameObject newBuff = Instantiate(buffPrefab, spawnPosition, Quaternion.identity);
                activeBuffs.Add(newBuff);

               
            }

            if (Time.time - timestart > 3)
            {
                break;
            }
        }

        
    }

    private void ApplyRandomBufftoBoss()
    {
        if (activeBuffs.Count < 1) {  return; }
        int randomIndex = Random.Range(0, activeBuffs.Count);
        BuffManager buffManager = activeBuffs[randomIndex].GetComponent<BuffManager>();
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        if (boss == null)
        {
            Debug.Log("Boss not found");
        }
        else
        {
            Debug.Log("Boss found = " + boss.name);
            buffManager.toBeBuffed = boss;
            buffManager.chosenBuff.Initialize(BossStatusSO, buffManager.chosenBuff.getBuffBonus(), buffManager.chosenBuff.getBuffType(), buffManager.chosenBuff.getBuffRarity(), 0f, 0f, 0f);
            buffManager.AddBuff(buffManager.chosenBuff);
            Debug.Log("Boss buffed with" + buffManager.chosenBuff.getBuffName());
        }
        
        
    }



}