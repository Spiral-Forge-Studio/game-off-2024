using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BuffRegistry
{
    private static Dictionary<string, Buff> availableBuffs = new Dictionary<string, Buff>();

    public static Dictionary<string, string> NametoBuffs = new Dictionary<string, string>()
    {
        {"Steel-Plated Armor", "HpBuff"},
        {"Reinforced Shields", "ShieldBuff"},
        {"Nano Repair Kit", "ShieldBreakRecoveryDelay"},
        {"Rapid Shield Regeneration", "ShieldRegenTickInterval"},
        {"Shield Regen Boost", "ShieldRegenAmountBuff"},
        {"Extra Magazine Clip", "MinigunMagazineBuff"},
        {"Hardened Plating","DamageReductionBuff" },
        {"Swift Stride", "MoveSpeedBuff"},
        {"Minigun Damage Boost", "MinigunDamageBuff"},
        {"Minigun Bullet Deviation Angle Reduction", "MinigunBulletDeviationAngleBuff"},
        {"Quick Loader", "MinigunReloadTimeBuff"},
        {"Rapid Barrel Upgrade", "MinigunFireRateBuff"}
    };


    public static void InitializeBuffs(PlayerStatusSO playerStatus)
    {
        availableBuffs["HpBuff"] = new HpBuff(playerStatus, 0f, HpBuff.BuffType.Flat, HpBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["MinigunMagazineBuff"] = new MinigunMagazineBuff(playerStatus, 0f, MinigunMagazineBuff.BuffType.Flat, MinigunMagazineBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["ShieldBuff"] = new ShieldBuff(playerStatus, 0f, ShieldBuff.BuffType.Flat, ShieldBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["ShieldRegenAmountBuff"] = new ShieldRegenAmountBuff(playerStatus, 0f, ShieldRegenAmountBuff.BuffType.Flat, ShieldRegenAmountBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["ShieldBreakRecoveryDelay"] = new ShieldBreakRecoveryDelay(playerStatus, 0f, ShieldBreakRecoveryDelay.BuffType.Flat, ShieldBreakRecoveryDelay.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["ShieldRegenTickInterval"] = new ShieldRegenTickIntervalBuff(playerStatus, 0f, ShieldRegenTickIntervalBuff.BuffType.Flat, ShieldRegenTickIntervalBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["DamageReductionBuff"] = new DamageReductionBuff(playerStatus, 0f, DamageReductionBuff.BuffType.Flat, DamageReductionBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["MoveSpeedBuff"] = new MoveSpeedBuff(playerStatus, 0f, MoveSpeedBuff.BuffType.Flat, MoveSpeedBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["MinigunDamageBuff"] = new MinigunDamageBuff(playerStatus, 0f, MinigunDamageBuff.BuffType.Flat, MinigunDamageBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["MinigunBulletDeviationAngleBuff"] = new MinigunBulletDeviationAngleBuff(playerStatus, 0f, MinigunBulletDeviationAngleBuff.BuffType.Flat, MinigunBulletDeviationAngleBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["MinigunFireRateBuff"] = new MinigunFireRateBuff(playerStatus, 0f, MinigunFireRateBuff.BuffType.Flat, MinigunFireRateBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["MinigunReloadTimeBuff"] = new MinigunReloadTimeBuff(playerStatus, 0f, MinigunReloadTimeBuff.BuffType.Flat, MinigunReloadTimeBuff.Rarity.Common, 0f, 0f, 0f);
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
        return new List<string>(NametoBuffs.Keys);
    }

    // Get all available buffs
    public static List<Buff> GetAllBuffs()
    {
        return new List<Buff>(availableBuffs.Values);
    }
}
