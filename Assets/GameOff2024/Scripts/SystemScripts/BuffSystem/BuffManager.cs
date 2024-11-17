using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

public class BuffManager : MonoBehaviour
{

    private void Awake()
    {
        // Initialize other components
        BuffRegistry.InitializeBuffs(playerStats);
        playerKCC = FindObjectOfType<PlayerKCC>();
        buffSpawner = FindAnyObjectByType<BuffSpawner>();
        playerStatusManager = FindAnyObjectByType<PlayerStatusManager>();
    }
    private List<Buff> activeBuffs = new List<Buff>();
    public PlayerKCC playerKCC;
    [SerializeField] private PlayerStatusSO playerStats;
    public float sphereCastInterval = 0.1f;

    private GameObject toBeBuffed;
    private BuffSpawner buffSpawner;
    public PlayerStatusManager playerStatusManager;

    // Get a random buff from the registry
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
                if (chosenBuff == null) return;

                // Set buff properties
                chosenBuff.UpdateBuffValues(chosenBuff.getRandomType(), chosenBuff.getRandomRarity());
                AddBuff(chosenBuff);

                Debug.Log($"You got: {chosenBuff.getBuffName()} " +
                          $"Rarity: {chosenBuff.getBuffRarity()} " +
                          $"Amount: {chosenBuff.getBuffBonus()} " +
                          $"Type: {chosenBuff.getBuffType()}");

                Destroy(gameObject); // Destroy the buff GameObject
            }
        }
    }

    public void AddBuff(Buff newBuff)
    {
        // Check if a similar buff is already active
        Buff existingBuff = activeBuffs.Find(b => b.GetType() == newBuff.GetType());

        if (existingBuff == null)
        {
            // Add and apply the new buff
            activeBuffs.Add(newBuff);
            newBuff.StartBuff(toBeBuffed);
        }
        else
        {
            // Apply stacking or consecutive logic if needed
            existingBuff.ApplyConsecutiveBuff();
        }
    }

    public void RemoveBuff(Buff buff)
    {
        buff.EndBuff(toBeBuffed); // Remove effects
        activeBuffs.Remove(buff); // Remove from list
    }

    private void Update()
    {
        PerformSphereCast();
    }
}
