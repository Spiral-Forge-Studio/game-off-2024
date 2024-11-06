using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum StatTypeMultiplier
{
    HealthMultiplier,
    ShieldMultiplier,
    ShieldRegenRateMultiplier,
    ShieldRegenDelayMultiplier,
    DamageReductionMultiplier,
    MoveSpeedMultiplier,
    DashCooldownMultiplier,

    MinigunDamageMultiplier,
    MinigunCritRateMultiplier,
    MinigunCritDamageMultiplier,
    MinigunFireRateMultiplier,
    MinigunReloadTimeMultiplier,
    MinigunMagazineSizeMultiplier,
    MinigunProjectileLifetimeMultiplier,
    MinigunProjectileSpeedMultiplier,
    MinigunBulletDeviationAngleMultiplier,

    RocketDamageMultiplier,
    RocketCritRateMultiplier,
    RocketCritDamageMultiplier,
    RocketFireRateMultiplier,
    RocketReloadTimeMultiplier,
    RocketMagazineSizeMultiplier,
    RocketProjectileLifetimeMultiplier,
    RocketProjectileSpeedMultiplier,
    RocketExplosionRadiusMultiplier
}

public enum StatTypeFlatBonus
{
    HealthFlatBonus,
    ShieldFlatBonus,
    ShieldRegenRateFlatBonus,
    ShieldRegenDelayFlatBonus,
    DamageReductionFlatBonus,
    MoveSpeedFlatBonus,
    DashCooldownFlatBonus,

    MinigunDamageFlatBonus,
    MinigunCritRateFlatBonus,
    MinigunCritDamageFlatBonus,
    MinigunFireRateFlatBonus,
    MinigunReloadTimeFlatBonus,
    MinigunMagazineSizeFlatBonus,
    MinigunProjectileLifetimeFlatBonus,
    MinigunProjectileSpeedFlatBonus,
    MinigunBulletDeviationAngleBonus,

    RocketDamageFlatBonus,
    RocketCritRateFlatBonus,
    RocketCritDamageFlatBonus,
    RocketFireRateFlatBonus,
    RocketReloadTimeFlatBonus,
    RocketMagazineSizeFlatBonus,
    RocketProjectileLifetimeFlatBonus,
    RocketProjectileSpeedFlatBonus,
    RocketExplosionRadiusFlatBonus
}



[CreateAssetMenu(fileName = "PlayerStatusSO", menuName = "Scriptable Objects/PlayerStatus Scriptable Object")]
public class PlayerStatusSO : ScriptableObject
{
    [Header("General")]
    [SerializeField] private float baseHealth;
    [SerializeField] private float baseShield;
    [SerializeField] private float baseShieldRegenRate;
    [SerializeField] private float baseShieldRegenDelay;
    [SerializeField] private float baseDamageReduction;
    [SerializeField] private float baseMoveSpeed;
    [SerializeField] private float baseDashCooldown;

    [Header("Minigun Base Stats")]
    [SerializeField] private float baseMinigunDamage;
    [SerializeField] private float baseMinigunCritRate;
    [SerializeField] private float baseMinigunCritDamage;
    [SerializeField] private float baseMinigunFireRate;
    [SerializeField] private float baseMinigunReloadTime;
    [SerializeField] private float baseMinigunMagazineSize;
    [SerializeField] private float baseMinigunProjectileLifetime;
    [SerializeField] private float baseMinigunProjectileSpeed;

    [Header("Minigun Special Base Stats")]
    [SerializeField] private float baseminigunBulletDeviationAngle;

    [Header("Rocket Base Stats")]
    [SerializeField] private float baseRocketDamage;
    [SerializeField] private float baseRocketCritRate;
    [SerializeField] private float baseRocketCritDamage;
    [SerializeField] private float baseRocketFireRate;
    [SerializeField] private float baseRocketReloadTime;
    [SerializeField] private float baseRocketMagazineSize;
    [SerializeField] private float baseRocketProjectileLifetime;
    [SerializeField] private float baseRocketProjectileSpeed;

    [Header("Rocket Special Base Stats")]
    [SerializeField] private float baseRocketExplosionRadius;


    // Modifier Dictionaries
    public Dictionary<StatTypeMultiplier, float> multipliers = new Dictionary<StatTypeMultiplier, float>
    {
        { StatTypeMultiplier.HealthMultiplier, 1f },
        { StatTypeMultiplier.ShieldMultiplier, 1f },
        { StatTypeMultiplier.ShieldRegenRateMultiplier, 1f },
        { StatTypeMultiplier.ShieldRegenDelayMultiplier, 1f },
        { StatTypeMultiplier.DamageReductionMultiplier, 1f },
        { StatTypeMultiplier.MoveSpeedMultiplier, 1f },
        { StatTypeMultiplier.DashCooldownMultiplier, 1f },
        { StatTypeMultiplier.MinigunDamageMultiplier, 1f },
        { StatTypeMultiplier.MinigunCritRateMultiplier, 1f },
        { StatTypeMultiplier.MinigunCritDamageMultiplier, 1f },
        { StatTypeMultiplier.MinigunFireRateMultiplier, 1f },
        { StatTypeMultiplier.MinigunReloadTimeMultiplier, 1f },
        { StatTypeMultiplier.MinigunMagazineSizeMultiplier, 1f },
        { StatTypeMultiplier.MinigunProjectileLifetimeMultiplier, 1f },
        { StatTypeMultiplier.MinigunProjectileSpeedMultiplier, 1f },
        { StatTypeMultiplier.MinigunBulletDeviationAngleMultiplier, 1f },
        { StatTypeMultiplier.RocketDamageMultiplier, 1f },
        { StatTypeMultiplier.RocketCritRateMultiplier, 1f },
        { StatTypeMultiplier.RocketCritDamageMultiplier, 1f },
        { StatTypeMultiplier.RocketFireRateMultiplier, 1f },
        { StatTypeMultiplier.RocketReloadTimeMultiplier, 1f },
        { StatTypeMultiplier.RocketMagazineSizeMultiplier, 1f },
        { StatTypeMultiplier.RocketProjectileLifetimeMultiplier, 1f },
        { StatTypeMultiplier.RocketProjectileSpeedMultiplier, 1f },
        { StatTypeMultiplier.RocketExplosionRadiusMultiplier, 1f }
    };

    public Dictionary<StatTypeFlatBonus, float> flatBonuses = new Dictionary<StatTypeFlatBonus, float>
    {
        { StatTypeFlatBonus.HealthFlatBonus, 0f },
        { StatTypeFlatBonus.ShieldFlatBonus, 0f },
        { StatTypeFlatBonus.ShieldRegenRateFlatBonus, 0f },
        { StatTypeFlatBonus.ShieldRegenDelayFlatBonus, 0f },
        { StatTypeFlatBonus.DamageReductionFlatBonus, 0f },
        { StatTypeFlatBonus.MoveSpeedFlatBonus, 0f },
        { StatTypeFlatBonus.DashCooldownFlatBonus, 0f },
        { StatTypeFlatBonus.MinigunDamageFlatBonus, 0f },
        { StatTypeFlatBonus.MinigunCritRateFlatBonus, 0f },
        { StatTypeFlatBonus.MinigunCritDamageFlatBonus, 0f },
        { StatTypeFlatBonus.MinigunFireRateFlatBonus, 0f },
        { StatTypeFlatBonus.MinigunReloadTimeFlatBonus, 0f },
        { StatTypeFlatBonus.MinigunMagazineSizeFlatBonus, 0f },
        { StatTypeFlatBonus.MinigunProjectileLifetimeFlatBonus, 0f },
        { StatTypeFlatBonus.MinigunProjectileSpeedFlatBonus, 0f },
        { StatTypeFlatBonus.MinigunBulletDeviationAngleBonus, 0f },
        { StatTypeFlatBonus.RocketDamageFlatBonus, 0f },
        { StatTypeFlatBonus.RocketCritRateFlatBonus, 0f },
        { StatTypeFlatBonus.RocketCritDamageFlatBonus, 0f },
        { StatTypeFlatBonus.RocketFireRateFlatBonus, 0f },
        { StatTypeFlatBonus.RocketReloadTimeFlatBonus, 0f },
        { StatTypeFlatBonus.RocketMagazineSizeFlatBonus, 0f },
        { StatTypeFlatBonus.RocketProjectileLifetimeFlatBonus, 0f },
        { StatTypeFlatBonus.RocketProjectileSpeedFlatBonus, 0f },
        { StatTypeFlatBonus.RocketExplosionRadiusFlatBonus, 0f }
    };

    // General Computed Stats
    public float Health => (baseHealth * multipliers[StatTypeMultiplier.HealthMultiplier]) + flatBonuses[StatTypeFlatBonus.HealthFlatBonus];
    public float Shield => (baseShield * multipliers[StatTypeMultiplier.ShieldMultiplier]) + flatBonuses[StatTypeFlatBonus.ShieldFlatBonus];
    public float ShieldRegenRate => (baseShieldRegenRate * multipliers[StatTypeMultiplier.ShieldRegenRateMultiplier]) + flatBonuses[StatTypeFlatBonus.ShieldRegenRateFlatBonus];
    public float ShieldRegenDelay => (baseShieldRegenDelay * multipliers[StatTypeMultiplier.ShieldRegenDelayMultiplier]) + flatBonuses[StatTypeFlatBonus.ShieldRegenDelayFlatBonus];
    public float DamageReduction => (baseDamageReduction * multipliers[StatTypeMultiplier.DamageReductionMultiplier]) + flatBonuses[StatTypeFlatBonus.DamageReductionFlatBonus];
    public float MoveSpeed => (baseMoveSpeed * multipliers[StatTypeMultiplier.MoveSpeedMultiplier]) + flatBonuses[StatTypeFlatBonus.MoveSpeedFlatBonus];
    public float DashCooldown => (baseDashCooldown * multipliers[StatTypeMultiplier.DashCooldownMultiplier]) + flatBonuses[StatTypeFlatBonus.DashCooldownFlatBonus];

    // Minigun Computed Stats
    public float MinigunDamage => (baseMinigunDamage * multipliers[StatTypeMultiplier.MinigunDamageMultiplier]) + flatBonuses[StatTypeFlatBonus.MinigunDamageFlatBonus];
    public float MinigunCritRate => (baseMinigunCritRate * multipliers[StatTypeMultiplier.MinigunCritRateMultiplier]) + flatBonuses[StatTypeFlatBonus.MinigunCritRateFlatBonus];
    public float MinigunCritDamage => (baseMinigunCritDamage * multipliers[StatTypeMultiplier.MinigunCritDamageMultiplier]) + flatBonuses[StatTypeFlatBonus.MinigunCritDamageFlatBonus];
    public float MinigunFireRate => (baseMinigunFireRate * multipliers[StatTypeMultiplier.MinigunFireRateMultiplier]) + flatBonuses[StatTypeFlatBonus.MinigunFireRateFlatBonus];
    public float MinigunReloadTime => (baseMinigunReloadTime * multipliers[StatTypeMultiplier.MinigunReloadTimeMultiplier]) + flatBonuses[StatTypeFlatBonus.MinigunReloadTimeFlatBonus];
    public float MinigunMagazineSize => (baseMinigunMagazineSize * multipliers[StatTypeMultiplier.MinigunMagazineSizeMultiplier]) + flatBonuses[StatTypeFlatBonus.MinigunMagazineSizeFlatBonus];
    public float MinigunProjectileLifetime => (baseMinigunProjectileLifetime * multipliers[StatTypeMultiplier.MinigunProjectileLifetimeMultiplier]) + flatBonuses[StatTypeFlatBonus.MinigunProjectileLifetimeFlatBonus];
    public float MinigunProjectileSpeed => (baseMinigunProjectileSpeed * multipliers[StatTypeMultiplier.MinigunProjectileSpeedMultiplier]) + flatBonuses[StatTypeFlatBonus.MinigunProjectileSpeedFlatBonus];
    public float MinigunBulletDeviationAngle => (baseminigunBulletDeviationAngle * multipliers[StatTypeMultiplier.MinigunBulletDeviationAngleMultiplier]) + flatBonuses[StatTypeFlatBonus.MinigunBulletDeviationAngleBonus];

    // Rocket Computed Stats
    public float RocketDamage => (baseRocketDamage * multipliers[StatTypeMultiplier.RocketDamageMultiplier]) + flatBonuses[StatTypeFlatBonus.RocketDamageFlatBonus];
    public float RocketCritRate => (baseRocketCritRate * multipliers[StatTypeMultiplier.RocketCritRateMultiplier]) + flatBonuses[StatTypeFlatBonus.RocketCritRateFlatBonus];
    public float RocketCritDamage => (baseRocketCritDamage * multipliers[StatTypeMultiplier.RocketCritDamageMultiplier]) + flatBonuses[StatTypeFlatBonus.RocketCritDamageFlatBonus];
    public float RocketFireRate => (baseRocketFireRate * multipliers[StatTypeMultiplier.RocketFireRateMultiplier]) + flatBonuses[StatTypeFlatBonus.RocketFireRateFlatBonus];
    public float RocketReloadTime => (baseRocketReloadTime * multipliers[StatTypeMultiplier.RocketReloadTimeMultiplier]) + flatBonuses[StatTypeFlatBonus.RocketReloadTimeFlatBonus];
    public float RocketMagazineSize => (baseRocketMagazineSize * multipliers[StatTypeMultiplier.RocketMagazineSizeMultiplier]) + flatBonuses[StatTypeFlatBonus.RocketMagazineSizeFlatBonus];
    public float RocketProjectileLifetime => (baseRocketProjectileLifetime * multipliers[StatTypeMultiplier.RocketProjectileLifetimeMultiplier]) + flatBonuses[StatTypeFlatBonus.RocketProjectileLifetimeFlatBonus];
    public float RocketProjectileSpeed => (baseRocketProjectileSpeed * multipliers[StatTypeMultiplier.RocketProjectileSpeedMultiplier]) + flatBonuses[StatTypeFlatBonus.RocketProjectileSpeedFlatBonus];
    public float RocketExplosionRadius => (baseRocketExplosionRadius * multipliers[StatTypeMultiplier.RocketExplosionRadiusMultiplier]) + flatBonuses[StatTypeFlatBonus.RocketExplosionRadiusFlatBonus];


    // Helper Functions

    /// <summary>
    /// Call to reset the player's status. Essentially removes all modifiers to the base stats.
    /// </summary>
    public void ResetMultipliersAndFlatBonuses()
    {
        foreach (var key in multipliers.Keys.ToList())
        {
            multipliers[key] = 1f;
        }

        foreach (var key in flatBonuses.Keys.ToList())
        {
            flatBonuses[key] = 0f;
        }
    }

    /// <summary>
    /// Call to modify stat multiplier
    /// </summary>
    /// <param name="statType">Stat multiplier to be modified</param>
    /// <param name="multiplierValue">Value to modify the stat by</param>
    /// <param name="isPercent">If the value is in percent or not</param>
    public void ModifyMultiplier(StatTypeMultiplier statType, float multiplierValue, bool isPercent)
    {
        if (isPercent)
        {
            multipliers[statType] += multiplierValue/100f;
        }
        else
        {
            multipliers[statType] += multiplierValue;
        }
    }

    /// <summary>
    /// Call to modify stat multiplier
    /// </summary>
    /// <param name="statType">Stat flatbonus to be modified</param>
    /// <param name="flatBonusValue">Value to modify the stat by</param>
    public void ModifyFlatBonus(StatTypeFlatBonus statType, float flatBonusValue)
    {
        flatBonuses[statType] += flatBonusValue;
    }


}
