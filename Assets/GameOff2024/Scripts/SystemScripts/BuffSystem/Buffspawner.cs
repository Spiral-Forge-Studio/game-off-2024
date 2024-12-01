using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuffSpawner : MonoBehaviour
{
    public GameObject buffPrefab;
    public Transform spawnPoint;  // This can serve as the center of the spawn area
    public float spawnInterval = 5f;
    public float spawnRadius = 5f; // Define the radius for random spawning
    public int buffCount = 3;      // Number of buffs to spawn
    public PlayerStatusSO playerStats;
    public PlayerStatusSO BossStatusSO;
    private List<GameObject> activeBuffs = new List<GameObject>();
    private bool buffpickedup = false;

    public void DestroyActiveBuff(GameObject buff)
    {
        if (activeBuffs.Contains(buff))
        {
            activeBuffs.Remove(buff);
            Destroy(buff);
            ApplyRandomBufftoBoss();
        }
        else
        {
            Destroy(buff);
            ApplyRandomBufftoBoss();
            buffpickedup = true;
        }
        
    }

    void Start()
    {
        //StartCoroutine(SpawnBuffAtIntervals());
        //playerStats.ResetMultipliersAndFlatBonuses();//MOVE THIS TO SOMEWHERE 
        buffpickedup = false;
        SpawnBuffs();
    }

    private void Update()
    {
        //Debug.Log("There are " + activeBuffs.Count + " active buffs");
        if (buffpickedup)
        {
            GameObject buff = GameObject.FindWithTag("Buff");
            Destroy(buff);
        }
    }

    private void SpawnBuffs()
    {
        float timestart = Time.time;
        int counter = 0;
        while (activeBuffs.Count < buffCount && buffPrefab != null)
        {
            counter++;
            if (counter > 500)
            {
                break;
            }
            Vector3 spawnPosition = spawnPoint.position + new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                2f,
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

            if (Time.time - timestart > 3f)
            {
                break;
            }
        }

        
    }

    private void ApplyRandomBufftoBoss()
    {
        if (activeBuffs.Count < 1) {  return; }
        buffpickedup = true;
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
            
            if (BuffRegistry.GoodBadBuffsForBoss[buffManager.chosenBuff.getBuffName()] == "Bad")
            {
                string buffcomponent = BuffRegistry.NameToComponent[buffManager.chosenBuff.getBuffName()];
                List<string> otherpossiblebuffs = new List<string>();
                foreach(string possiblebuff in BuffRegistry.GetAllBuffNames())
                {
                    if (BuffRegistry.GoodBadBuffsForBoss[possiblebuff] == "Good" && BuffRegistry.NameToComponent[possiblebuff] == BuffRegistry.NameToComponent[buffManager.chosenBuff.getBuffName()])
                    {
                        otherpossiblebuffs.Add(possiblebuff);
                    }
                }
                randomIndex = Random.Range(0, otherpossiblebuffs.Count);
                buffManager.chosenBuff = BuffRegistry.availableBuffs[BuffRegistry.NameToBuffs[otherpossiblebuffs[randomIndex]]];
            }
            buffManager.toBeBuffed = boss;
            buffManager.chosenBuff.Initialize(BossStatusSO, buffManager.chosenBuff.getBuffBonus(), buffManager.chosenBuff.getBuffType(), buffManager.chosenBuff.getBuffRarity(), 0f, 0f, 0f);
            buffManager.AddBuff(buffManager.chosenBuff);
            Debug.Log("Boss buffed with" + buffManager.chosenBuff.getBuffName());
        }
        
        
    }



}