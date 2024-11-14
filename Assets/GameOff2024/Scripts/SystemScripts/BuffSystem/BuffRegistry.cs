using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BuffRegistry
{
    private static Dictionary<string, Buff> availableBuffs = new Dictionary<string, Buff>();

    // Initialize and register available buffs (e.g., at the start of the game)


    public static Dictionary<string, string> NametoBuffs = new Dictionary<string, string>()
    {
        {"Steel-Plated Armor", "HpBuff"},
        {"Reinforced Shields", "ShieldBuff"},
        {"Nano Repair Kit", "ShieldRegenBuff"},
        {"Reactive Armor", "DamageReductionBuff"},
        {"Boosted Thrusters", "MoveSpeedBuff"},
        {"Overclocked Engines", "DashCooldownBuff"},
        {"Precision Firing", "MinigunDamageBuff"},
        {"Hardened Core", "DamageReductionBuff"},
        {"High-Capacity Battery", "ShieldRegenDelayBuff"},
        {"Experimental Projectiles", "MinigunProjectileSpeedBuff"},
        {"Piercing Rounds", "MinigunCritRateBuff"},
        {"Accelerated Fire Control", "MinigunFireRateBuff"},
        {"Extra Magazine Clip", "MinigunMagazineBuff"},
        {"Rifled Barrel", "MinigunBulletDeviationBuff"},
        {"Thermal Rounds", "RocketDamageBuff"},
        {"Shockwave Rockets", "RocketExplosionRadiusBuff"},
        {"Rapid Detonation", "RocketFireRateBuff"},
        {"Efficient Reload", "RocketReloadTimeBuff"},
        {"Targeted Payloads", "RocketCritRateBuff"},
        {"Tungsten Alloy Rounds", "RocketProjectileSpeedBuff"},
        {"High-Impact Payload", "RocketCritDamageBuff"},
        {"Explosive Ammunition", "MinigunDamageBuff"},
        {"Lightweight Construction", "MoveSpeedBuff"},
        {"Improved Cooling", "ShieldRegenRateBuff"},
        {"Laser Sights", "MinigunCritRateBuff"},
        {"Ion-Plated Armor", "ShieldBuff"},
        {"High-Pressure Rounds", "RocketCritRateBuff"},
        {"Tactical Scanners", "CritRateBuff"},
        {"Quick Reflexes", "DashCooldownBuff"},
        {"Auto-Repair Modules", "HealthRegenBuff"},
        {"Energy Siphon", "MinigunShieldRestoreBuff"},
        {"Adaptive Plating", "DamageReductionBuff"},
        {"Nano-Regen Pulse", "ShieldRegenBuff"},
        {"Critical Strike Overload", "MinigunCritDamageBuff"},
        {"Focused Payload", "RocketDamageBuff"},
        {"Overdrive Mode", "MinigunFireRateBuff"},
        {"Magnetized Payloads", "RocketExplosionAttractionBuff"},
        {"Twin Rockets", "RocketMultiShotBuff"},
        {"Shield Overcharge", "MinigunFireRateBuff"},
        {"Drone Companion", "DroneBuff"},
        {"Swarm Rockets", "RocketMultiShotBuff"},
        {"Hologram Decoy", "HologramBuff"},
        {"Reflective Armor", "ProjectileReflectionBuff"},
        {"Shockwave Stomp", "ShockwaveBuff"},
        {"Dimensional Rift", "RiftBuff"},
        {"Energy Vortex Rockets", "RocketVortexBuff"},
        {"Berserker Mode", "BerserkBuff"},
        {"Ammo Recycler", "MinigunAmmoBuff"},
        {"Phase Shift", "IntangibilityBuff"},
        {"Gigantic Rockets", "RocketSizeBuff"},
        {"Phasewalk Rocket", "IntangibleRocketBuff"},
        {"Reflective Dash", "DashReflectionBuff"}
    };


    public static void InitializeBuffs(PlayerStatusSO playerStatus)
    {
        availableBuffs["HpBuff"] = new HpBuff(playerStatus, 0f, HpBuff.BuffType.Flat, HpBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["MinigunMagazineBuff"] = new MinigunMagazineBuff(playerStatus, 0f, MinigunMagazineBuff.BuffType.Flat, MinigunMagazineBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["ShieldBuff"] = new ShieldBuff(playerStatus, 0f, ShieldBuff.BuffType.Flat, ShieldBuff.Rarity.Common, 0f, 0f, 0f);
        availableBuffs["ShieldRegenAmountBuff"] = new ShieldRegenAmountBuff(playerStatus, 0f, ShieldBuff.BuffType.Flat, ShieldBuff.Rarity.Common, 0f, 0f, 0f);
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
