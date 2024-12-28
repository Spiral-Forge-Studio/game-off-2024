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
        {"Steel - Plated Armor", "Increase HP."},
        {"Reinforced Shields", "Increase shield."},
        {"Nano Repair Kit", "Reduce shield break recovery delay."},
        {"Rapid Shield Regeneration", "Increase shield regeneration speed."},
        {"Shield Regen Boost", "Increase shield regeneration amount."},
        {"Hardened Plating", "Increase damage reduction."},
        {"Swift Stride", "Increase movement speed."},

        {"Minigun Damage Boost", "Increase minigun damage per shot."},
        {"Minigun Bullet Deviation Angle Reduction", "Reduce minigun bullet deviation."},
        {"Quick Loader", "Reduce minigun reload time."},
        {"Minigun Projectile Speed Buff", "Increase minigun bullet speed."},
        {"Minigun Crit Rate Buff", "Increase minigun critical rate."},
        {"Minigun Crit Damage Buff", "Increase minigun critical damage."},
        {"Rapid Barrel Upgrade", "Increase minigun firerate."},
        {"Extra Magazine Clip", "Increase minigun maximum ammo capacity"},

        {"Rocket Damage Boost", "Increase rocket damage."},
        {"Rocket Explosion Radius Boost", "Increase rocket explosion radius."},
        {"Quick Loader Rocket", "Increase rocket rearm time."},
        {"Rocket Crit Rate Buff", "Inrease rocket critical rate."},
        {"Rocket Crit Damage Buff", "Increase rocket critical damage."},
        {"Extra Rocket Magazine Clip", "Increase rocket maximum ammo capacity"}
    };

    public static readonly Dictionary<string, string> NametoStattobebuffed = new Dictionary<string, string>()
{
    {"Steel - Plated Armor", "HP"},
    {"Reinforced Shields", "Shield"},
    {"Nano Repair Kit", "Shield Break Recovery Delay"},
    {"Rapid Shield Regeneration", "Shield Regeneration Tick Interval"},
    {"Shield Regen Boost", "Shield Regeneration Amount"},
    {"Hardened Plating", "Damage Reduction"},
    {"Swift Stride", "Movement Speed"},

    {"Minigun Damage Boost", "Minigun Damage"},
    {"Minigun Bullet Deviation Angle Reduction", "Minigun Bullet Deviation"},
    {"Quick Loader", "Minigun Reload Time"},
    {"Minigun Projectile Speed Buff", "Minigun Bullet Speed"},
    {"Minigun Crit Rate Buff", "Minigun Critical Rate"},
    {"Minigun Crit Damage Buff", "Minigun Critical Damage"},
    {"Rapid Barrel Upgrade", "Minigun Fire Rate"},
    {"Extra Magazine Clip", "Minigun Maximum Ammo Capacity"},

    {"Rocket Damage Boost", "Rocket Damage"},
    {"Rocket Explosion Radius Boost", "Rocket Explosion Radius"},
    {"Quick Loader Rocket", "Rocket Rearm Time"},
    {"Rocket Crit Rate Buff", "Rocket Critical Rate"},
    {"Rocket Crit Damage Buff", "Rocket Critical Damage"},
    {"Extra Rocket Magazine Clip", "Rocket Maximum Ammo Capacity"}
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
