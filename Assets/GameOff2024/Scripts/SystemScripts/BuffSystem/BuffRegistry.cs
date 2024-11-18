using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class BuffRegistry
{
    private static Dictionary<string, Buff> availableBuffs = new Dictionary<string, Buff>();

    public static Dictionary<string, string> NametoBuffs = new Dictionary<string, string>()
    {
        {"Steel - Plated Armor", "HpBuff"},
        {"Reinforced Shields", "ShieldBuff"},
        {"Nano Repair Kit", "ShieldBreakRecoveryDelay"},
        {"Rapid Shield Regeneration", "ShieldRegenTickInterval"},
        {"Shield Regen Boost", "ShieldRegenAmountBuff"},
        {"Hardened Plating","DamageReductionBuff" },
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


    public static void InitializeBuffs(PlayerStatusSO playerStatus)
    {
        availableBuffs["HpBuff"] = new HpBuff(playerStatus, 0f, HpBuff.BuffType.Flat, HpBuff.Rarity.Common, 0f, 0f, 0f);
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
        availableBuffs["MinigunCritRateBuff"] = new MinigunCritRateBuff(playerStatus, 0f, MinigunCritRateBuff.BuffType.Flat, MinigunCritRateBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["MinigunCritDamageBuff"] = new MinigunCritDamageBuff(playerStatus, 0f, MinigunCritDamageBuff.BuffType.Flat, MinigunCritDamageBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["MinigunMagazineBuff"] = new MinigunMagazineBuff(playerStatus, 0f, MinigunMagazineBuff.BuffType.Flat, MinigunMagazineBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["MinigunProjectileSpeedBuff"] = new MinigunProjectileSpeedBuff(playerStatus, 0f, MinigunProjectileSpeedBuff.BuffType.Flat, MinigunProjectileSpeedBuff.Rarity.Common, 0f, 0f, 0f);

        availableBuffs["RocketDamageBuff"] = new RocketDamageBuff(playerStatus, 0f, RocketDamageBuff.BuffType.Flat, RocketDamageBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["RocketReloadTimeBuff"] = new RocketReloadTimeBuff(playerStatus, 0f, RocketReloadTimeBuff.BuffType.Flat, RocketReloadTimeBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["RocketCritRateBuff"] = new RocketCritRateBuff(playerStatus, 0f, RocketCritRateBuff.BuffType.Flat, RocketCritRateBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["RocketCritDamageBuff"] = new RocketCritDamageBuff(playerStatus, 0f, RocketCritDamageBuff.BuffType.Flat, RocketCritDamageBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["RocketMagazineBuff"] = new RocketMagazineBuff(playerStatus, 0f, RocketMagazineBuff.BuffType.Flat, RocketMagazineBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["RocketExplosionRadiusBuff"] = new RocketExplosionRadiusBuff(playerStatus, 0f, RocketExplosionRadiusBuff.BuffType.Flat, RocketExplosionRadiusBuff.Rarity.Common, 0f, 0f, 0f);
        // Add more buffs as needed
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
