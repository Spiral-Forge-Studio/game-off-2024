using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EStatTypeMultiplierBoss
{
    HealthMultiplier,
    ShieldMultiplier,
    ShieldRegenAmountMultiplier,
    ShieldBreakRecoveryDelayMultiplier,
    ShieldRegenTickIntervalMultipier,
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
    RocketExplosionRadiusMultiplier,
    RocketHoldDurationMultiplier,
    RocketReleaseFireRateMultiplier
}
public enum EStatTypeFlatBonusBoss
{
    HealthFlatBonus,
    ShieldFlatBonus,
    ShieldRegenAmountFlatBonus,
    ShieldBreakRecoveryDelayFlatBonus,
    ShieldRegenTickIntervalFlatBonus,
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
    RocketExplosionRadiusFlatBonus,
    RocketHoldDurationFlatBonus,
    RocketReleaseFireRateFlatBonus
}

[CreateAssetMenu(fileName = "BossStatusSO", menuName = "Scriptable Objects/BossStatus Scriptable Object")]
public class BossStatusSO : ScriptableObject
{
    [Header("General")]
    [SerializeField] private float baseHealth;
    [SerializeField] private float baseShield;
    [SerializeField] private float baseShieldRegenAmount;
    [SerializeField] private float baseShieldBreakRecoveryDelay;
    [SerializeField] private float baseShieldRegenTickInterval;
    [SerializeField] private float baseDamageReduction;
    [SerializeField] private float baseMoveSpeed;
    [SerializeField] private float baseDashCooldown;

    [Header("Minigun Base Stats")]
    [SerializeField] private float b_MinigunDamage;
    [SerializeField] private float b_MinigunCritRate;
    [SerializeField] private float b_MinigunCritDamage;
    [SerializeField] private float b_MinigunFireRate;
    [SerializeField] private float b_MinigunReloadTime;
    [SerializeField] private int b_MinigunMagazineSize;
    [SerializeField] private float b_MinigunProjectileLifetime;
    [SerializeField] private float b_MinigunProjectileSpeed;

    [Header("Minigun Special Base Stats")]
    [SerializeField] private float b_minigunDeviationAngle;

    [Header("Rocket Base Stats")]
    [SerializeField] private float b_RocketDamage;
    [SerializeField] private float b_RocketCritRate;
    [SerializeField] private float b_RocketCritDamage;
    [SerializeField] private float b_RocketFireRate;
    [SerializeField] private float b_RocketReloadTime;
    [SerializeField] private int b_RocketMagazineSize;
    [SerializeField] private float b_RocketProjectileLifetime;
    [SerializeField] private float b_RocketProjectileSpeed;

    [Header("Rocket Special Base Stats")]
    [SerializeField] private float b_RocketExplosionRadius;
    [SerializeField] private float b_RocketHoldDuration;
    [SerializeField] private float b_RocketReleaseFirerate;


    // Modifier Dictionaries
    private Dictionary<EStatTypeMultiplierBoss, float> multipliers = new Dictionary<EStatTypeMultiplierBoss, float>
    {
        { EStatTypeMultiplierBoss.HealthMultiplier, 1f },
        { EStatTypeMultiplierBoss.ShieldMultiplier, 1f },
        { EStatTypeMultiplierBoss.ShieldRegenAmountMultiplier, 1f },
        { EStatTypeMultiplierBoss.ShieldBreakRecoveryDelayMultiplier, 1f },
        { EStatTypeMultiplierBoss.ShieldRegenTickIntervalMultipier, 1f },
        { EStatTypeMultiplierBoss.DamageReductionMultiplier, 1f },
        { EStatTypeMultiplierBoss.MoveSpeedMultiplier, 1f },
        { EStatTypeMultiplierBoss.DashCooldownMultiplier, 1f },
        { EStatTypeMultiplierBoss.MinigunDamageMultiplier, 1f },
        { EStatTypeMultiplierBoss.MinigunCritRateMultiplier, 1f },
        { EStatTypeMultiplierBoss.MinigunCritDamageMultiplier, 1f },
        { EStatTypeMultiplierBoss.MinigunFireRateMultiplier, 1f },
        { EStatTypeMultiplierBoss.MinigunReloadTimeMultiplier, 1f },
        { EStatTypeMultiplierBoss.MinigunMagazineSizeMultiplier, 1f },
        { EStatTypeMultiplierBoss.MinigunProjectileLifetimeMultiplier, 1f },
        { EStatTypeMultiplierBoss.MinigunProjectileSpeedMultiplier, 1f },
        { EStatTypeMultiplierBoss.MinigunBulletDeviationAngleMultiplier, 1f },
        { EStatTypeMultiplierBoss.RocketDamageMultiplier, 1f },
        { EStatTypeMultiplierBoss.RocketCritRateMultiplier, 1f },
        { EStatTypeMultiplierBoss.RocketCritDamageMultiplier, 1f },
        { EStatTypeMultiplierBoss.RocketFireRateMultiplier, 1f },
        { EStatTypeMultiplierBoss.RocketReloadTimeMultiplier, 1f },
        { EStatTypeMultiplierBoss.RocketMagazineSizeMultiplier, 1f },
        { EStatTypeMultiplierBoss.RocketProjectileLifetimeMultiplier, 1f },
        { EStatTypeMultiplierBoss.RocketProjectileSpeedMultiplier, 1f },
        { EStatTypeMultiplierBoss.RocketExplosionRadiusMultiplier, 1f },
        { EStatTypeMultiplierBoss.RocketHoldDurationMultiplier, 1f },
        { EStatTypeMultiplierBoss.RocketReleaseFireRateMultiplier, 1f }
    };

    public  Dictionary<EStatTypeFlatBonusBoss, float> flatBonuses = new Dictionary<EStatTypeFlatBonusBoss, float>
    {
        { EStatTypeFlatBonusBoss.HealthFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.ShieldFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.ShieldRegenAmountFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.ShieldBreakRecoveryDelayFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.ShieldRegenTickIntervalFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.DamageReductionFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.MoveSpeedFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.DashCooldownFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.MinigunDamageFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.MinigunCritRateFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.MinigunCritDamageFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.MinigunFireRateFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.MinigunReloadTimeFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.MinigunMagazineSizeFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.MinigunProjectileLifetimeFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.MinigunProjectileSpeedFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.MinigunBulletDeviationAngleBonus, 0f },
        { EStatTypeFlatBonusBoss.RocketDamageFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.RocketCritRateFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.RocketCritDamageFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.RocketFireRateFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.RocketReloadTimeFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.RocketMagazineSizeFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.RocketProjectileLifetimeFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.RocketProjectileSpeedFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.RocketExplosionRadiusFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.RocketHoldDurationFlatBonus, 0f },
        { EStatTypeFlatBonusBoss.RocketReleaseFireRateFlatBonus, 0f }
    };

    // General Computed Stats
    public float Health => (baseHealth * multipliers[EStatTypeMultiplierBoss.HealthMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.HealthFlatBonus];
    public float Shield => (baseShield * multipliers[EStatTypeMultiplierBoss.ShieldMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.ShieldFlatBonus];
    public float ShieldRegenAmount => (baseShieldRegenAmount * multipliers[EStatTypeMultiplierBoss.ShieldRegenAmountMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.ShieldRegenAmountFlatBonus];
    public float ShieldBreakRecoveryDelay => (baseShieldBreakRecoveryDelay * multipliers[EStatTypeMultiplierBoss.ShieldBreakRecoveryDelayMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.ShieldBreakRecoveryDelayFlatBonus];
    public float ShieldRegenTickInterval => (baseShieldRegenTickInterval * multipliers[EStatTypeMultiplierBoss.ShieldRegenTickIntervalMultipier]) + flatBonuses[EStatTypeFlatBonusBoss.ShieldRegenTickIntervalFlatBonus];    
    public float DamageReduction => (baseDamageReduction * multipliers[EStatTypeMultiplierBoss.DamageReductionMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.DamageReductionFlatBonus];
    public float MoveSpeed => (baseMoveSpeed * multipliers[EStatTypeMultiplierBoss.MoveSpeedMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.MoveSpeedFlatBonus];
    public float DashCooldown => (baseDashCooldown * multipliers[EStatTypeMultiplierBoss.DashCooldownMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.DashCooldownFlatBonus];

    // Minigun Computed Stats
    public float MinigunDamage => (b_MinigunDamage * multipliers[EStatTypeMultiplierBoss.MinigunDamageMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.MinigunDamageFlatBonus];
    public float MinigunCritRate => (b_MinigunCritRate * multipliers[EStatTypeMultiplierBoss.MinigunCritRateMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.MinigunCritRateFlatBonus];
    public float MinigunCritDamage => (b_MinigunCritDamage * multipliers[EStatTypeMultiplierBoss.MinigunCritDamageMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.MinigunCritDamageFlatBonus];
    public float MinigunFireRate => (b_MinigunFireRate * multipliers[EStatTypeMultiplierBoss.MinigunFireRateMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.MinigunFireRateFlatBonus];
    public float MinigunReloadTime => (b_MinigunReloadTime * multipliers[EStatTypeMultiplierBoss.MinigunReloadTimeMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.MinigunReloadTimeFlatBonus];
    public int MinigunMagazineSize => (int)((b_MinigunMagazineSize * multipliers[EStatTypeMultiplierBoss.MinigunMagazineSizeMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.MinigunMagazineSizeFlatBonus]);
    public float MinigunProjectileLifetime => (b_MinigunProjectileLifetime * multipliers[EStatTypeMultiplierBoss.MinigunProjectileLifetimeMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.MinigunProjectileLifetimeFlatBonus];
    public float MinigunProjectileSpeed => (b_MinigunProjectileSpeed * multipliers[EStatTypeMultiplierBoss.MinigunProjectileSpeedMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.MinigunProjectileSpeedFlatBonus];
    public float MinigunBulletDeviationAngle => (b_minigunDeviationAngle * multipliers[EStatTypeMultiplierBoss.MinigunBulletDeviationAngleMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.MinigunBulletDeviationAngleBonus];

    // Rocket Computed Stats
    public float RocketDamage => (b_RocketDamage * multipliers[EStatTypeMultiplierBoss.RocketDamageMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.RocketDamageFlatBonus];
    public float RocketCritRate => (b_RocketCritRate * multipliers[EStatTypeMultiplierBoss.RocketCritRateMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.RocketCritRateFlatBonus];
    public float RocketCritDamage => (b_RocketCritDamage * multipliers[EStatTypeMultiplierBoss.RocketCritDamageMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.RocketCritDamageFlatBonus];
    public float RocketFireRate => (b_RocketFireRate * multipliers[EStatTypeMultiplierBoss.RocketFireRateMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.RocketFireRateFlatBonus];
    public float RocketReloadTime => (b_RocketReloadTime * multipliers[EStatTypeMultiplierBoss.RocketReloadTimeMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.RocketReloadTimeFlatBonus];
    public int RocketMagazineSize => (int)((b_RocketMagazineSize * multipliers[EStatTypeMultiplierBoss.RocketMagazineSizeMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.RocketMagazineSizeFlatBonus]);
    public float RocketProjectileLifetime => (b_RocketProjectileLifetime * multipliers[EStatTypeMultiplierBoss.RocketProjectileLifetimeMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.RocketProjectileLifetimeFlatBonus];
    public float RocketProjectileSpeed => (b_RocketProjectileSpeed * multipliers[EStatTypeMultiplierBoss.RocketProjectileSpeedMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.RocketProjectileSpeedFlatBonus];
    public float RocketExplosionRadius => (b_RocketExplosionRadius * multipliers[EStatTypeMultiplierBoss.RocketExplosionRadiusMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.RocketExplosionRadiusFlatBonus];
    public float RocketHoldDuration => (b_RocketHoldDuration * multipliers[EStatTypeMultiplierBoss.RocketHoldDurationMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.RocketHoldDurationFlatBonus];
    public float RocketReleaseFireRate => (b_RocketReleaseFirerate * multipliers[EStatTypeMultiplierBoss.RocketReleaseFireRateMultiplier]) + flatBonuses[EStatTypeFlatBonusBoss.RocketReleaseFireRateFlatBonus];


    #region --- Initialization Functions ---

    /// <summary>
    /// Call to reset the Boss's status. Essentially removes all modifiers to the base stats.
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

    #endregion

    #region --- Roguelike functions ---

    /// <summary>
    /// Call to modify stat multiplier
    /// </summary>
    /// <param name="statType">Stat multiplier to be modified</param>
    /// <param name="multiplierValue">Value to modify the stat by</param>
    /// <param name="isPercent">If the value is in percent or not</param>
    public void ModifyMultiplier(EStatTypeMultiplierBoss statType, float multiplierValue, bool isPercent)
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
    public void ModifyFlatBonus(EStatTypeFlatBonusBoss statType, float flatBonusValue)
    {
        flatBonuses[statType] += flatBonusValue;
    }

    #endregion

}
