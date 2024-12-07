using System.Collections.Generic;
using KinematicCharacterController;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    private BuffMenu buffMenu;
    public Buff chosenBuff;
    public Buff Buff1;
    public Buff Buff2;
    private bool choosingbuff;
    private List<Buff> activeBuffs = new List<Buff>();
    public PlayerKCC playerKCC;
    [SerializeField] private PlayerStatusSO playerStats;
    public float sphereCastInterval = 0.1f;
    public GameObject BuffChoiceUI;
    public GameObject toBeBuffed;
    private BuffSpawner buffSpawner;
    public PlayerStatusManager playerStatusManager;
    public BuffMenuUIManager BMUIManager;



    private void Awake()
    {
        buffMenu = Resources.Load<BuffMenu>("BuffMenu");
        if (buffMenu == null)
        {
            Debug.LogError("BuffMenu could not be found in Resources!");
        }
        choosingbuff = false;
        // Initialize other components
        BuffRegistry.InitializeBuffs(playerStats);
        playerKCC = FindObjectOfType<PlayerKCC>();
        buffSpawner = FindAnyObjectByType<BuffSpawner>();
        playerStatusManager = FindAnyObjectByType<PlayerStatusManager>();
        chosenBuff = GetRandomBuff();
        BuffChoiceUI = GameObject.FindGameObjectWithTag("BuffChoiceUI");
        BMUIManager = FindAnyObjectByType<BuffMenuUIManager>();
        if (chosenBuff == null) return;

        // Set buff properties
        chosenBuff.UpdateBuffValues(chosenBuff.getRandomType(), chosenBuff.getRandomRarity());
        SetBuffComponent(BuffRegistry.NameToComponent[chosenBuff.getBuffName()]);
        //SetBuffColor(chosenBuff.getBuffRarity());
    }
    

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
                choosingbuff = true;
                
                toBeBuffed = hit.collider.gameObject;
                Buffchoosing();
                /*
                buffMenu.DiscoverBuff(chosenBuff.getBuffName());
                toBeBuffed = hit.collider.gameObject;
                
                AddBuff(chosenBuff);
                string message = $"You got: {chosenBuff.getBuffName()} ";

                ShowFloatingText(message, toBeBuffed.transform);
                //put teleport function here
                Debug.Log("BUFF TYPE: " + chosenBuff.getBuffType());
                if (buffSpawner != null)
                {
                    buffSpawner.DestroyActiveBuff(gameObject); // Notify and destroy this buff
                   
                }
                else
                {
                    Debug.Log("MISSING BUFFSPAWNER");
                }
                Destroy(gameObject); // Destroy the buff GameObject*/
                break;

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
        if (choosingbuff)
        {
            
        }
        else
        {
            PerformSphereCast();
        }
        
    }

    private void Buffchoosing()
    {
        string componenttobebuffed = BuffRegistry.NameToComponent[chosenBuff.getBuffName()];
        List<string> Buffchoices = BuffRegistry.RandomBuffComponent(componenttobebuffed);
        int choice1 = Random.Range(0, Buffchoices.Count);
        int choice2 = Random.Range(0, Buffchoices.Count);
        while (choice1 == choice2)
        {
            if (Buffchoices.Count < 2) { break; }
            choice2 = Random.Range(0, Buffchoices.Count);
        }
        string buffname1 = Buffchoices[choice1];
        string buffname2 = Buffchoices[choice2];
        Buff.BuffType Buff1type = chosenBuff.getRandomType();
        Buff.BuffType Buff2type = chosenBuff.getRandomType();
        Buff.Rarity Buff1rarity = chosenBuff.getRandomRarity();
        Buff.Rarity Buff2rarity = chosenBuff.getRandomRarity();
        BMUIManager.ToggleBuffchoice();
        // Assuming BuffChoiceUI is already referenced in your script
        BuffChoiceUI = GameObject.FindGameObjectWithTag("BuffChoiceUI");
        if (BuffChoiceUI != null)
        {
            
            Transform buffList = BuffChoiceUI.transform.Find("Buff List");
            if (buffList != null)
            {
                // Get the BuffChoicePrefabs (assuming only two are visible at a time)
                Transform choice1Transform = buffList.GetChild(0);
                Transform choice2Transform = buffList.GetChild(1);

                // Update choice 1
                choice1Transform.Find("BuffName").GetComponent<TextMeshProUGUI>().text = buffname1;
                choice1Transform.Find("BuffDescription").GetComponent<TextMeshProUGUI>().text = BuffRegistry.NametoBuffDescription[buffname1];
                choice1Transform.Find("BuffRarity").GetComponent<TextMeshProUGUI>().text = Buff1rarity.ToString();
                choice1Transform.Find("BuffRarity").GetComponent<TextMeshProUGUI>().color = SetBuffColor(Buff1rarity);
                choice1Transform.Find("BuffSelectBorder").GetComponent<Image>().color = SetBuffColor(Buff1rarity);

                // Update choice 2
                choice2Transform.Find("BuffName").GetComponent<TextMeshProUGUI>().text = buffname2;
                choice2Transform.Find("BuffDescription").GetComponent<TextMeshProUGUI>().text = BuffRegistry.NametoBuffDescription[buffname2];
                choice2Transform.Find("BuffRarity").GetComponent<TextMeshProUGUI>().text = Buff2rarity.ToString();
                choice2Transform.Find("BuffRarity").GetComponent<TextMeshProUGUI>().color = SetBuffColor(Buff2rarity);
                choice2Transform.Find("BuffSelectBorder").GetComponent<Image>().color = SetBuffColor(Buff2rarity);

                // Optionally, set other properties like images
                // choice1Transform.Find("BuffIcon").GetComponent<Image>().sprite = BuffRegistry.GetBuffIcon(buffname1);
            }
            else
            {
                Debug.LogError("Buff List not found in BuffChoiceUI!");
            }
        }
        else
        {
            Debug.LogError("BuffChoiceUI is null!");
        }
        Buff1 = BuffRegistry.availableBuffs[BuffRegistry.NameToBuffs[buffname1]];
        Buff2 = BuffRegistry.availableBuffs[BuffRegistry.NameToBuffs[buffname2]];
        Buff1.UpdateBuffValues(chosenBuff.getRandomType(), chosenBuff.getRandomRarity());
        Buff2.UpdateBuffValues(chosenBuff.getRandomType(), chosenBuff.getRandomRarity());

        //AddBuff(chosenBuff);
        //string message = $"You got: {chosenBuff.getBuffName()} ";

        //ShowFloatingText(message, toBeBuffed.transform);
        //put teleport function here
        //Debug.Log("BUFF TYPE: " + chosenBuff.getBuffType());

        //buffMenu.DiscoverBuff(chosenBuff.getBuffName());
        if (buffSpawner != null)
        {
            buffSpawner.DestroyActiveBuff(gameObject); // Notify and destroy this buff
            buffSpawner.Buff1 = Buff1;
            buffSpawner.Buff2 = Buff2;

        }
        else
        {
            Debug.Log("MISSING BUFFSPAWNER");
        }
        Destroy(gameObject); // Destroy the buff GameObject
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

    private Color SetBuffColor(Buff.Rarity rarity)
    {

        switch (rarity)
        {
            case Buff.Rarity.Common:
                return new Color(0.71f, 0.71f, 0.71f); // Gray
            case Buff.Rarity.Uncommon:
                return new Color(0.30f, 0.78f, 0.31f); // Green
            case Buff.Rarity.Rare:
                return new Color(0.13f, 0.59f, 0.95f); // Blue
            case Buff.Rarity.Epic:
                return new Color(0.61f, 0.15f, 0.69f); // Purple
            case Buff.Rarity.Legendary:
                return new Color(1.0f, 0.76f, 0.07f); // Gold
            default:
                return Color.white; // Default color
        }


        // Apply the color to the material
        // Apply the emission color
        //Material material = renderer.material;
        //material.SetColor("_EmissionColor", emissionColor);

        // Enable emission if it isn't already
        //material.EnableKeyword("_EMISSION");
    }
}
