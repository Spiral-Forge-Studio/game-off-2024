using System.Collections.Generic;
using UnityEngine;

public static class BuffRegistry
{
    private static Dictionary<string, Buff> availableBuffs = new Dictionary<string, Buff>();

    // Initialize and register available buffs (e.g., at the start of the game)
    public static void InitializeBuffs(PlayerStatusSO playerStatus)
    {
        availableBuffs["HpBuff"] = new HpBuff(playerStatus, 0f, HpBuff.BuffType.Flat, HpBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["MinigunMagazineBuff"] = new MinigunMagazineBuff(playerStatus, 0f, MinigunMagazineBuff.BuffType.Flat, MinigunMagazineBuff.Rarity.Common, 0f, 0f, 0f);
        // Add more buffs as needed
    }

    // Get a specific buff by name
    public static Buff GetBuff(string buffName)
    {
        if (availableBuffs.TryGetValue(buffName, out Buff buff))
        {
            return buff;
        }
        Debug.LogWarning("Buff not found: " + buffName);
        return null;
    }

    // Get all available buff names (for UI or debugging)
    public static List<string> GetAllBuffNames()
    {
        return new List<string>(availableBuffs.Keys);
    }

    // Get all available buffs
    public static List<Buff> GetAllBuffs()
    {
        return new List<Buff>(availableBuffs.Values);
    }
}
