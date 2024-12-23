using UnityEngine;
using TMPro;
using System;

public class BuffMenuUIManager : MonoBehaviour
{
    public BuffMenu buffMenu; // Reference to the BuffMenu ScriptableObject
    public Transform BuffchoiceUI;
    public GameObject buffMenuPanel; // Reference to the UI Panel containing the buff text
    public TextMeshProUGUI discoveredBuffsText; // Reference to the TextMeshPro element for discovered buffs
    public TextMeshProUGUI undiscoveredBuffsText; // Reference to the TextMeshPro element for undiscovered buffs
    public BuffSpawner buffSpawner;
    public GameObject toBeBuffed;

    private bool isMenuVisible = false;

    void Start()
    {
        if (buffMenuPanel != null)
        {
            buffMenuPanel.SetActive(false); // Hide the panel initially
        }
        buffSpawner = FindAnyObjectByType<BuffSpawner>();
        UpdateBuffTexts(); // Initialize text with current buff data
    }

    void Update()
    {
        // Toggle UI visibility with Tab key
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isMenuVisible = !isMenuVisible;
            buffMenuPanel.SetActive(isMenuVisible);

            if (isMenuVisible)
            {
                UpdateBuffTexts();
            }
        }
    }

    /// <summary>
    /// Updates the UI text with discovered and undiscovered buffs.
    /// </summary>
    void UpdateBuffTexts()
    {
        // Get discovered and undiscovered buffs from the BuffMenu
        var discoveredBuffs = buffMenu.GetDiscoveredBuffs();
        var undiscoveredBuffs = buffMenu.GetUndiscoveredBuffs();

        // Format and set the text
        discoveredBuffsText.text = "Discovered Buffs (" + discoveredBuffs.Count + "/" + buffMenu.BuffStatuses.Count + "):\n" + string.Join("\n", discoveredBuffs);
        //undiscoveredBuffsText.text = "Undiscovered Buffs:\n" + string.Join("\n", undiscoveredBuffs);
    }

    public void ToggleBuffchoice()
    {
        Transform Buffchoice =  BuffchoiceUI.GetChild(0);

        if (Buffchoice != null)
        {
            if (Buffchoice.gameObject.activeSelf)
            {
                Buffchoice.gameObject.SetActive(false);
            }
            else
            {
                Buffchoice.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.Log("BuffchoiceUI is null");
        }
    }

    public void Buff1Selected()
    {
        Debug.Log("Buff 1 selected!");
        buffSpawner.Buff1.ApplyBuff(toBeBuffed);
        ToggleBuffchoice();
        // Add your logic here
    }
    public void Buff2Selected()
    {
        buffSpawner.Buff2.ApplyBuff(toBeBuffed);
        Debug.Log("Buff 2 selected!");
        ToggleBuffchoice();

        // Add your logic here
    }
}
