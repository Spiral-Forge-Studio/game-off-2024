using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffUIManager : MonoBehaviour
{
    [SerializeField] private GameObject buffUIPanel; // Panel for the buff menu
    [SerializeField] private Transform contentParent; // Parent object for buff entries
    [SerializeField] private GameObject buffEntryPrefab; // Prefab for each buff entry
    [SerializeField] private BuffMenu buffMenu; // Reference to your BuffMenu SO

    private bool isPanelOpen = false;

    private void Start()
    {
        buffUIPanel.SetActive(false);
        PopulateBuffUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleBuffUI();
        }
    }

    private void ToggleBuffUI()
    {
        isPanelOpen = !isPanelOpen;
        buffUIPanel.SetActive(isPanelOpen);

        if (isPanelOpen)
        {
            RefreshBuffUI();
        }
    }

    private void PopulateBuffUI()
    {
        // Clear existing entries
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Add discovered buffs
        List<string> discoveredBuffs = buffMenu.GetDiscoveredBuffs();
        foreach (var buffName in discoveredBuffs)
        {
            CreateBuffEntry(buffName, true);
        }

        // Add undiscovered buffs
        List<string> undiscoveredBuffs = buffMenu.GetUndiscoveredBuffs();
        foreach (var buffName in undiscoveredBuffs)
        {
            CreateBuffEntry(buffName, false);
        }
    }

    private void CreateBuffEntry(string buffName, bool isDiscovered)
    {
        GameObject buffEntry = Instantiate(buffEntryPrefab, contentParent);
        Text nameText = buffEntry.transform.Find("Name").GetComponent<Text>();
        nameText.text = isDiscovered ? buffName : "???";

        Text statusText = buffEntry.transform.Find("Status").GetComponent<Text>();
        statusText.text = isDiscovered ? "Discovered" : "Undiscovered";
    }

    private void RefreshBuffUI()
    {
        PopulateBuffUI();
    }
}
