using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffMenu", menuName = "Scriptable Objects/BuffMenu")]
public class BuffMenu : ScriptableObject
{
    [System.Serializable]
    public class BuffStatus
    {
        public string BuffName;
        public bool IsDiscovered;
    }

    public bool FirstTimeRun = true;
    public List<BuffStatus> BuffStatuses = new List<BuffStatus>();

    /// <summary>
    /// Initializes the BuffMenu with all buffs in the registry.
    /// </summary>
    public void InitializeMenu()
    {
        BuffStatuses.Clear();
        Debug.Log("Buff Menu Cleared and Reset is this your first time playing?");
        foreach (var entry in BuffRegistry.NameToBuffs.Keys)
        {
            BuffStatuses.Add(new BuffStatus { BuffName = entry, IsDiscovered = false });
        }
    }

    /// <summary>
    /// Mark a buff as discovered.
    /// </summary>
    public void DiscoverBuff(string buffName)
    {
        var buffStatus = BuffStatuses.Find(b => b.BuffName == buffName);
        if (buffStatus != null)
        {
            buffStatus.IsDiscovered = true;
        }
    }

    /// <summary>
    /// Retrieve a list of discovered buffs.
    /// </summary>
    public List<string> GetDiscoveredBuffs()
    {
        return BuffStatuses.FindAll(b => b.IsDiscovered).ConvertAll(b => b.BuffName);
    }

    /// <summary>
    /// Retrieve a list of undiscovered buffs.
    /// </summary>
    public List<string> GetUndiscoveredBuffs()
    {
        return BuffStatuses.FindAll(b => !b.IsDiscovered).ConvertAll(b => b.BuffName);
    }
}
