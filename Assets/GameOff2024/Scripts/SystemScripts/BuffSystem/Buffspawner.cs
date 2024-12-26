using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuffSpawner : MonoBehaviour
{
    public GameObject buffPrefab;
    public Transform spawnPoint;  // This can serve as the center of the spawn area
    public LayerMask TerrainLayers;
    //public float spawnRadius = 5f; // Define the radius for random spawning
    public float spacing = 2f; // Adjust this value for the distance between buffs
    public int buffCount = 2;      // Number of buffs to spawn
    public PlayerStatusSO playerStats;
    public PlayerStatusSO BossStatusSO;
    private List<GameObject> activeBuffs = new List<GameObject>();
    private bool buffpickedup = false;
    public Buff Buff1;
    public Buff Buff2;

    public void DestroyActiveBuff(GameObject buff)
    {
        if (buff == null)
        {
            return;
        }
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
        buffpickedup = false;
        playerStats.ResetMultipliersAndFlatBonuses();//MOVE THIS TO RESET ON START OF RUNN
    }

    private void Update()
    {
        //Debug.Log("There are " + activeBuffs.Count + " active buffs");
        if (buffpickedup)
        {
            for (int i = 0; i < buffCount-1; i++)
            {
                GameObject buff = GameObject.FindWithTag("Buff");
                Destroy(buff);
            }
            buffpickedup = false;
        }
    }

    public void SpawnBuffs()
    {
        
        float startX = spawnPoint.position.x - (buffCount - 1) * spacing / 2; // Center the line of buffs

        for (int i = 0; i < buffCount; i++)
        {
            // Calculate the spawn position for each buff
            Vector3 spawnPosition = new Vector3(
                startX + i * spacing,
                spawnPoint.position.y,
                spawnPoint.position.z
            );

            // Ensure no overlapping with other objects
            Collider[] colliders = Physics.OverlapSphere(spawnPosition, 1f, TerrainLayers);
            if (colliders.Length == 0)
            {
                // Instantiate the buff and add it to the active list
                GameObject newBuff = Instantiate(buffPrefab, spawnPosition, Quaternion.identity);
                activeBuffs.Add(newBuff);
            }
            else
            {
                Debug.Log(colliders[0].name);
                Debug.LogWarning($"Failed to spawn buff at {spawnPosition}, position already occupied.");
                Debug.Break();
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
            //Debug.Log("Boss not found");
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