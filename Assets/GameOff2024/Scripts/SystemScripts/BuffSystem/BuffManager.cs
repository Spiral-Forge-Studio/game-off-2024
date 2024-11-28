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
        chosenBuff = GetRandomBuff();
        if (chosenBuff == null) return;

        // Set buff properties
        chosenBuff.UpdateBuffValues(chosenBuff.getRandomType(), chosenBuff.getRandomRarity());
    }
    public Buff chosenBuff;
    private List<Buff> activeBuffs = new List<Buff>();
    public PlayerKCC playerKCC;
    [SerializeField] private PlayerStatusSO playerStats;
    public float sphereCastInterval = 0.1f;

    public GameObject toBeBuffed;
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

    public GameObject floatingTextPrefab; // Assign your FloatingText prefab in the Inspector

    void PerformSphereCast()
    {
        float radius = 0.5f;

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, radius, Vector3.forward, radius);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Player"))
            {
                toBeBuffed = hit.collider.gameObject;
                
                AddBuff(chosenBuff);

                string message = $"You got: {chosenBuff.getBuffName()} " +
                                 $"Rarity: {chosenBuff.getBuffRarity()} " +
                                 $"Amount: {chosenBuff.getBuffBonus()} " +
                                 $"Type: {chosenBuff.getBuffType()}";

                ShowFloatingText(message, toBeBuffed.transform);

                Debug.Log(message);
                if (buffSpawner != null)
                {
                    buffSpawner.DestroyActiveBuff(gameObject); // Notify and destroy this buff
                }
                Destroy(gameObject); // Destroy the buff GameObject
            }
        }
    }


    private void ShowFloatingText(string message, Transform playerTransform)
    {
        if (floatingTextPrefab != null)
        {
            GameObject floatingText = Instantiate(floatingTextPrefab, playerTransform.position + Vector3.up * 2f, Quaternion.identity);
            floatingText.GetComponent<FloatingText>().Initialize(message, playerTransform);
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
