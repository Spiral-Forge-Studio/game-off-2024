using System.Collections.Generic;
using UnityEngine;

public static class BuffRegistry
{

    public static readonly Dictionary<string, Buff> availableBuffs = new Dictionary<string, Buff>();


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

    public static readonly Dictionary<string, string> NameToComponent = new Dictionary<string, string>()
    {
        {"Steel - Plated Armor", "Body"},
        {"Reinforced Shields", "Body"},
        {"Nano Repair Kit", "Body"},
        {"Rapid Shield Regeneration", "Body"},
        {"Shield Regen Boost", "Body"},
        {"Hardened Plating", "Body"},
        {"Swift Stride", "Body"},

        {"Minigun Damage Boost", "Minigun"},
        {"Minigun Bullet Deviation Angle Reduction", "Minigun"},
        {"Quick Loader", "Minigun"},
        {"Minigun Projectile Speed Buff", "Minigun"},
        {"Minigun Crit Rate Buff", "Minigun"},
        {"Minigun Crit Damage Buff", "Minigun"},
        {"Rapid Barrel Upgrade", "Minigun"},
        {"Extra Magazine Clip", "Minigun"},

        {"Rocket Damage Boost", "Rocket"},
        {"Rocket Explosion Radius Boost", "Rocket"},
        {"Quick Loader Rocket", "Rocket"},
        {"Rocket Crit Rate Buff", "Rocket"},
        {"Rocket Crit Damage Buff", "Rocket"},
        {"Extra Rocket Magazine Clip", "Rocket"}
    };


    public static readonly Dictionary<string, string> GoodBadBuffsForBoss = new Dictionary<string, string>()
    {
        {"Steel - Plated Armor", "Good"},
        {"Reinforced Shields", "Good"},
        {"Nano Repair Kit", "Good"},
        {"Rapid Shield Regeneration", "Good"},
        {"Shield Regen Boost", "Good"},
        {"Hardened Plating", "Good"},
        {"Swift Stride", "Bad"},

        {"Minigun Damage Boost", "Good"},
        {"Minigun Bullet Deviation Angle Reduction", "Good"},
        {"Quick Loader", "Bad"},
        {"Minigun Projectile Speed Buff", "Good"},
        {"Minigun Crit Rate Buff", "Good"},
        {"Minigun Crit Damage Buff", "Good"},
        {"Rapid Barrel Upgrade", "Good"},
        {"Extra Magazine Clip", "Bad"},

        {"Rocket Damage Boost", "Good"},
        {"Rocket Explosion Radius Boost", "Good"},
        {"Quick Loader Rocket", "Bad"},
        {"Rocket Crit Rate Buff", "Good"},
        {"Rocket Crit Damage Buff", "Good"},
        {"Extra Rocket Magazine Clip", "Bad"}
    };

    public static readonly Dictionary<string, string> NametoBuffDescription = new Dictionary<string, string>()
    {
        {"Steel - Plated Armor", "Descriptions"},
        {"Reinforced Shields", "Descriptions"},
        {"Nano Repair Kit", "Descriptions"},
        {"Rapid Shield Regeneration", "Descriptions"},
        {"Shield Regen Boost", "Descriptions"},
        {"Hardened Plating", "Descriptions"},
        {"Swift Stride", "Descriptions"},

        {"Minigun Damage Boost", "Descriptions"},
        {"Minigun Bullet Deviation Angle Reduction", "Descriptions"},
        {"Quick Loader", "Descriptions"},
        {"Minigun Projectile Speed Buff", "Descriptions"},
        {"Minigun Crit Rate Buff", "Descriptions"},
        {"Minigun Crit Damage Buff", "Descriptions"},
        {"Rapid Barrel Upgrade", "Descriptions"},
        {"Extra Magazine Clip", "Descriptions"},

        {"Rocket Damage Boost", "Descriptions"},
        {"Rocket Explosion Radius Boost", "Descriptions"},
        {"Quick Loader Rocket", "Descriptions"},
        {"Rocket Crit Rate Buff", "Descriptions"},
        {"Rocket Crit Damage Buff", "Descriptions"},
        {"Extra Rocket Magazine Clip", "Descriptions"}
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

    public static List<string> RandomBuffComponent(string componentname)
    {
        List<string> buffnameslist = new List<string>();
        foreach(var Name in NameToComponent.Keys)
        {
            if (NameToComponent[Name] == componentname) {
                buffnameslist.Add(Name);
            }

        }
        return buffnameslist;
    }
}
