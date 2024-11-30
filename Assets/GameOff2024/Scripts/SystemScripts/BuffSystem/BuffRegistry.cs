using System.Collections.Generic;
using UnityEngine;

public static class BuffRegistry
{

    private static readonly Dictionary<string, Buff> availableBuffs = new Dictionary<string, Buff>();


    public static readonly Dictionary<string, string> NameToBuffs = new Dictionary<string, string>()
    {
        {"Steel - Plated Armor", "HpBuff"},
        {"Reinforced Shields", "ShieldBuff"},
        {"Nano Repair Kit", "ShieldBreakRecoveryDelay"},
        {"Rapid Shield Regeneration", "ShieldRegenTickInterval"},
        {"Shield Regen Boost", "ShieldRegenAmountBuff"},
        {"Hardened Plating", "DamageReductionBuff"},
        {"Swift Stride", "MoveSpeedBuff"},

        {"Minigun Damage Boost", "MinigunDamageBuff"},
        {"Minigun Bullet Deviation Angle Reduction", "MinigunBulletDeviationAngleBuff"},
        {"Quick Loader", "MinigunReloadTimeBuff"},
        {"Minigun Projectile Speed Buff", "MinigunProjectileSpeedBuff"},
        {"Minigun Crit Rate Buff", "MinigunCritRateBuff"},
        {"Minigun Crit Damage Buff", "MinigunCritDamageBuff"},
        {"Rapid Barrel Upgrade", "MinigunFireRateBuff"},
        {"Extra Magazine Clip", "MinigunMagazineBuff"},

        {"Rocket Damage Boost", "RocketDamageBuff"},
        {"Rocket Explosion Radius Boost", "RocketExplosionRadiusBuff"},
        {"Quick Loader Rocket", "RocketReloadTimeBuff"},
        {"Rocket Crit Rate Buff", "RocketCritRateBuff"},
        {"Rocket Crit Damage Buff", "RocketCritDamageBuff"},
        {"Extra Rocket Magazine Clip", "RocketMagazineBuff"}
    };



    /// <summary>
    /// Initializes all buffs and stores them in the registry.
    /// </summary>
    /// <param name="playerStatus">Player's status for initializing buffs.</param>
    public static void InitializeBuffs(PlayerStatusSO playerStatus)
    {
        
        
        AddBuff<HpBuff>("HpBuff", playerStatus);
        AddBuff<ShieldBuff>("ShieldBuff", playerStatus);
        AddBuff<ShieldBreakRecoveryDelay>("ShieldBreakRecoveryDelay", playerStatus);
        AddBuff<ShieldRegenTickIntervalBuff>("ShieldRegenTickInterval", playerStatus);
        AddBuff<ShieldRegenAmountBuff>("ShieldRegenAmountBuff", playerStatus);
        AddBuff<DamageReductionBuff>("DamageReductionBuff", playerStatus);
        AddBuff<MoveSpeedBuff>("MoveSpeedBuff", playerStatus);

        AddBuff<MinigunDamageBuff>("MinigunDamageBuff", playerStatus);
        AddBuff<MinigunBulletDeviationAngleBuff>("MinigunBulletDeviationAngleBuff", playerStatus);
        AddBuff<MinigunReloadTimeBuff>("MinigunReloadTimeBuff", playerStatus);
        AddBuff<MinigunProjectileSpeedBuff>("MinigunProjectileSpeedBuff", playerStatus);
        AddBuff<MinigunCritRateBuff>("MinigunCritRateBuff", playerStatus);
        AddBuff<MinigunCritDamageBuff>("MinigunCritDamageBuff", playerStatus);
        AddBuff<MinigunFireRateBuff>("MinigunFireRateBuff", playerStatus);
        AddBuff<MinigunMagazineBuff>("MinigunMagazineBuff", playerStatus);

        AddBuff<RocketDamageBuff>("RocketDamageBuff", playerStatus);
        AddBuff<RocketExplosionRadiusBuff>("RocketExplosionRadiusBuff", playerStatus);
        AddBuff<RocketReloadTimeBuff>("RocketReloadTimeBuff", playerStatus);
        AddBuff<RocketCritRateBuff>("RocketCritRateBuff", playerStatus);
        AddBuff<RocketCritDamageBuff>("RocketCritDamageBuff", playerStatus);
        AddBuff<RocketMagazineBuff>("RocketMagazineBuff", playerStatus);
    }

    /// <summary>
    /// Adds a new buff to the registry using ScriptableObject.
    /// </summary>
    private static void AddBuff<T>(string buffKey, PlayerStatusSO playerStatus) where T : Buff
    {
        T buff = ScriptableObject.CreateInstance<T>();
        buff.Initialize(playerStatus, 0f, Buff.BuffType.Flat, Buff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs[buffKey] = buff;
    }

    /// <summary>
    /// Retrieves a specific buff by its key.
    /// </summary>
    public static Buff GetBuff(string buffKey)
    {
        if (availableBuffs.TryGetValue(buffKey, out Buff buff))
        {
            return buff;
        }

        Debug.LogWarning($"Buff not found: {buffKey}");
        return null;
    }

    /// <summary>
    /// Retrieves all available buff names.
    /// </summary>
    public static List<string> GetAllBuffNames()
    {
        return new List<string>(NameToBuffs.Keys);
    }

    /// <summary>
    /// Retrieves all available buffs.
    /// </summary>
    public static List<Buff> GetAllBuffs()
    {
        return new List<Buff>(availableBuffs.Values);
    }
}
