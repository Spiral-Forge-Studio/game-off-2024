using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    private List<Buff> activeBuffs = new List<Buff>();
    public PlayerKCC playerKCC;
    public PlayerStatusSO playerStats;
    public float sphereCastInterval = 0.0001f;

    private float sphereCastTimer = 0f;
    private GameObject toBeBuffed;
    private BuffSpawner buffSpawner;
    public PlayerStatusManager playerStatusManager;


    Buff GetRandomBuff()
    {
        List<Buff> allBuffs = BuffRegistry.GetAllBuffs();

        if (allBuffs.Count == 0)
        {
            Debug.LogWarning("No buffs available in the registry.");
            return null;
        }

        int randomIndex = Random.Range(0, allBuffs.Count);
        return allBuffs[randomIndex];
    }


    void PerformSphereCast()
    {
        float radius = 0.5f;

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, radius, Vector3.forward, radius);
        foreach (RaycastHit hit in hits)
        {   
            if (hit.collider.CompareTag("Player"))
            {
                toBeBuffed = hit.collider.gameObject;
                Buff chosenBuff = GetRandomBuff();
                chosenBuff = chosenBuff = BuffRegistry.GetBuff("MinigunReloadTimeBuff");
                //chosenBuff.UpdateBuffValues(chosenBuff.getRandomType(),chosenBuff.getRandomRarity());
                chosenBuff.UpdateBuffValues(chosenBuff.getRandomType(), Buff.Rarity.Legendary);
                //chosenBuff = BuffRegistry.GetBuff("ShieldRegenAmountBuff");
                AddBuff(chosenBuff);
                Debug.Log("You got: " + BuffRegistry.NametoBuffs[chosenBuff.getBuffName()] + " Rarity: " + chosenBuff.getBuffRarity() + " Amount: " + chosenBuff.getBuffBonus() + " Type: " + chosenBuff.getBuffType());
                Destroy(this.gameObject);
            }
        }
    }

    public void AddBuff(Buff newBuff)
    {
        Buff existingBuff = activeBuffs.Find(b => b.GetType() == newBuff.GetType());

        if (existingBuff == null)
        {
            activeBuffs.Add(newBuff);
            newBuff.StartBuff(toBeBuffed);
        }
        else
        {
            // Optionally update the existing buff
        }
    }

    private void Awake()
    {
        BuffRegistry.InitializeBuffs(playerStats);
        playerKCC = FindObjectOfType<PlayerKCC>();
        buffSpawner = FindAnyObjectByType<BuffSpawner>();
        playerStatusManager = FindAnyObjectByType<PlayerStatusManager>();
        sphereCastTimer = sphereCastInterval;
    }
    
    private void Update()
    {
        PerformSphereCast();
    }

    public void RemoveBuff(Buff buff)
    {
        activeBuffs.Remove(buff);
    }

    
}
