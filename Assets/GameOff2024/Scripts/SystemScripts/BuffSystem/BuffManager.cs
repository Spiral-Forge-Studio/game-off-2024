using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    private BuffMenu buffMenu;
    private void Awake()
    {
        buffMenu = Resources.Load<BuffMenu>("BuffMenu");
        if (buffMenu == null)
        {
            Debug.LogError("BuffMenu could not be found in Resources!");
        }
        // Initialize other components
        BuffRegistry.InitializeBuffs(playerStats);
        playerKCC = FindObjectOfType<PlayerKCC>();
        buffSpawner = FindAnyObjectByType<BuffSpawner>();
        playerStatusManager = FindAnyObjectByType<PlayerStatusManager>();
        chosenBuff = GetRandomBuff();
        if (chosenBuff == null) return;

        // Set buff properties
        chosenBuff.UpdateBuffValues(chosenBuff.getRandomType(), chosenBuff.getRandomRarity());
        SetBuffComponent(BuffRegistry.NameToComponent[chosenBuff.getBuffName()]);
        SetBuffColor(chosenBuff.getBuffRarity());
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
                
                buffMenu.DiscoverBuff(chosenBuff.getBuffName());
                toBeBuffed = hit.collider.gameObject;
                
                AddBuff(chosenBuff);
                string message = $"You got: {chosenBuff.getBuffName()} ";

                ShowFloatingText(message, toBeBuffed.transform);
                //put teleport function here
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

    private void SetBuffComponent(string component)
    {
        // Find the child with the corresponding name
        Transform child = transform.Find(component);

        // Check if the child exists
        if (child != null)
        {
            // Enable the child
            child.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"Child with name '{component}' not found in the prefab!");
        }


    }

    private void SetBuffColor(Buff.Rarity rarity)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogWarning("No Renderer found on the Buff GameObject!");
            return;
        }

        Color emissionColor;
        switch (rarity)
        {
            case Buff.Rarity.Common:
                emissionColor = new Color(0.71f, 0.71f, 0.71f); // Gray
                break;
            case Buff.Rarity.Uncommon:
                emissionColor = new Color(0.30f, 0.78f, 0.31f); // Green
                break;
            case Buff.Rarity.Rare:
                emissionColor = new Color(0.13f, 0.59f, 0.95f); // Blue
                break;
            case Buff.Rarity.Epic:
                emissionColor = new Color(0.61f, 0.15f, 0.69f); // Purple
                break;
            case Buff.Rarity.Legendary:
                emissionColor = new Color(1.0f, 0.76f, 0.07f); // Gold
                break;
            default:
                emissionColor = Color.white; // Default color
                break;
        }


        // Apply the color to the material
        // Apply the emission color
        Material material = renderer.material;
        material.SetColor("_EmissionColor", emissionColor);

        // Enable emission if it isn't already
        material.EnableKeyword("_EMISSION");
    }
}
