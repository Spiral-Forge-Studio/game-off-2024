using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EStatTypeMultiplier
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
public enum EStatTypeFlatBonus
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

[CreateAssetMenu(fileName = "PlayerStatusSO", menuName = "Scriptable Objects/PlayerStatus Scriptable Object")]
public class PlayerStatusSO : ScriptableObject
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
    public Dictionary<EStatTypeMultiplier, float> multipliers = new Dictionary<EStatTypeMultiplier, float>
    {
        { EStatTypeMultiplier.HealthMultiplier, 1f },
        { EStatTypeMultiplier.ShieldMultiplier, 1f },
        { EStatTypeMultiplier.ShieldRegenAmountMultiplier, 1f },
        { EStatTypeMultiplier.ShieldBreakRecoveryDelayMultiplier, 1f },
        { EStatTypeMultiplier.ShieldRegenTickIntervalMultipier, 1f },
        { EStatTypeMultiplier.DamageReductionMultiplier, 1f },
        { EStatTypeMultiplier.MoveSpeedMultiplier, 1f },
        { EStatTypeMultiplier.DashCooldownMultiplier, 1f },
        { EStatTypeMultiplier.MinigunDamageMultiplier, 1f },
        { EStatTypeMultiplier.MinigunCritRateMultiplier, 1f },
        { EStatTypeMultiplier.MinigunCritDamageMultiplier, 1f },
        { EStatTypeMultiplier.MinigunFireRateMultiplier, 1f },
        { EStatTypeMultiplier.MinigunReloadTimeMultiplier, 1f },
        { EStatTypeMultiplier.MinigunMagazineSizeMultiplier, 1f },
        { EStatTypeMultiplier.MinigunProjectileLifetimeMultiplier, 1f },
        { EStatTypeMultiplier.MinigunProjectileSpeedMultiplier, 1f },
        { EStatTypeMultiplier.MinigunBulletDeviationAngleMultiplier, 1f },
        { EStatTypeMultiplier.RocketDamageMultiplier, 1f },
        { EStatTypeMultiplier.RocketCritRateMultiplier, 1f },
        { EStatTypeMultiplier.RocketCritDamageMultiplier, 1f },
        { EStatTypeMultiplier.RocketFireRateMultiplier, 1f },
        { EStatTypeMultiplier.RocketReloadTimeMultiplier, 1f },
        { EStatTypeMultiplier.RocketMagazineSizeMultiplier, 1f },
        { EStatTypeMultiplier.RocketProjectileLifetimeMultiplier, 1f },
        { EStatTypeMultiplier.RocketProjectileSpeedMultiplier, 1f },
        { EStatTypeMultiplier.RocketExplosionRadiusMultiplier, 1f },
        { EStatTypeMultiplier.RocketHoldDurationMultiplier, 1f },
        { EStatTypeMultiplier.RocketReleaseFireRateMultiplier, 1f }
    };

    public  Dictionary<EStatTypeFlatBonus, float> flatBonuses = new Dictionary<EStatTypeFlatBonus, float>
    {
        { EStatTypeFlatBonus.HealthFlatBonus, 0f },
        { EStatTypeFlatBonus.ShieldFlatBonus, 0f },
        { EStatTypeFlatBonus.ShieldRegenAmountFlatBonus, 0f },
        { EStatTypeFlatBonus.ShieldBreakRecoveryDelayFlatBonus, 0f },
        { EStatTypeFlatBonus.ShieldRegenTickIntervalFlatBonus, 0f },
        { EStatTypeFlatBonus.DamageReductionFlatBonus, 0f },
        { EStatTypeFlatBonus.MoveSpeedFlatBonus, 0f },
        { EStatTypeFlatBonus.DashCooldownFlatBonus, 0f },
        { EStatTypeFlatBonus.MinigunDamageFlatBonus, 0f },
        { EStatTypeFlatBonus.MinigunCritRateFlatBonus, 0f },
        { EStatTypeFlatBonus.MinigunCritDamageFlatBonus, 0f },
        { EStatTypeFlatBonus.MinigunFireRateFlatBonus, 0f },
        { EStatTypeFlatBonus.MinigunReloadTimeFlatBonus, 0f },
        { EStatTypeFlatBonus.MinigunMagazineSizeFlatBonus, 0f },
        { EStatTypeFlatBonus.MinigunProjectileLifetimeFlatBonus, 0f },
        { EStatTypeFlatBonus.MinigunProjectileSpeedFlatBonus, 0f },
        { EStatTypeFlatBonus.MinigunBulletDeviationAngleBonus, 0f },
        { EStatTypeFlatBonus.RocketDamageFlatBonus, 0f },
        { EStatTypeFlatBonus.RocketCritRateFlatBonus, 0f },
        { EStatTypeFlatBonus.RocketCritDamageFlatBonus, 0f },
        { EStatTypeFlatBonus.RocketFireRateFlatBonus, 0f },
        { EStatTypeFlatBonus.RocketReloadTimeFlatBonus, 0f },
        { EStatTypeFlatBonus.RocketMagazineSizeFlatBonus, 0f },
        { EStatTypeFlatBonus.RocketProjectileLifetimeFlatBonus, 0f },
        { EStatTypeFlatBonus.RocketProjectileSpeedFlatBonus, 0f },
        { EStatTypeFlatBonus.RocketExplosionRadiusFlatBonus, 0f },
        { EStatTypeFlatBonus.RocketHoldDurationFlatBonus, 0f },
        { EStatTypeFlatBonus.RocketReleaseFireRateFlatBonus, 0f }
    };

    // General Computed Stats
    public float Health => (baseHealth * multipliers[EStatTypeMultiplier.HealthMultiplier]) + flatBonuses[EStatTypeFlatBonus.HealthFlatBonus];
    public float Shield => (baseShield * multipliers[EStatTypeMultiplier.ShieldMultiplier]) + flatBonuses[EStatTypeFlatBonus.ShieldFlatBonus];
    public float ShieldRegenAmount => (baseShieldRegenAmount * multipliers[EStatTypeMultiplier.ShieldRegenAmountMultiplier]) + flatBonuses[EStatTypeFlatBonus.ShieldRegenAmountFlatBonus];
    public float ShieldBreakRecoveryDelay => (baseShieldBreakRecoveryDelay * multipliers[EStatTypeMultiplier.ShieldBreakRecoveryDelayMultiplier]) + flatBonuses[EStatTypeFlatBonus.ShieldBreakRecoveryDelayFlatBonus];
    public float ShieldRegenTickInterval => (baseShieldRegenTickInterval * multipliers[EStatTypeMultiplier.ShieldRegenTickIntervalMultipier]) + flatBonuses[EStatTypeFlatBonus.ShieldRegenTickIntervalFlatBonus];    
    public float DamageReduction => (baseDamageReduction * multipliers[EStatTypeMultiplier.DamageReductionMultiplier]) + flatBonuses[EStatTypeFlatBonus.DamageReductionFlatBonus];
    public float MoveSpeed => (baseMoveSpeed * multipliers[EStatTypeMultiplier.MoveSpeedMultiplier]) + flatBonuses[EStatTypeFlatBonus.MoveSpeedFlatBonus];
    public float DashCooldown => (baseDashCooldown * multipliers[EStatTypeMultiplier.DashCooldownMultiplier]) + flatBonuses[EStatTypeFlatBonus.DashCooldownFlatBonus];

    // Minigun Computed Stats
    public float MinigunDamage => (b_MinigunDamage * multipliers[EStatTypeMultiplier.MinigunDamageMultiplier]) + flatBonuses[EStatTypeFlatBonus.MinigunDamageFlatBonus];
    public float MinigunCritRate => (b_MinigunCritRate * multipliers[EStatTypeMultiplier.MinigunCritRateMultiplier]) + flatBonuses[EStatTypeFlatBonus.MinigunCritRateFlatBonus];
    public float MinigunCritDamage => (b_MinigunCritDamage * multipliers[EStatTypeMultiplier.MinigunCritDamageMultiplier]) + flatBonuses[EStatTypeFlatBonus.MinigunCritDamageFlatBonus];
    public float MinigunFireRate => (b_MinigunFireRate * multipliers[EStatTypeMultiplier.MinigunFireRateMultiplier]) + flatBonuses[EStatTypeFlatBonus.MinigunFireRateFlatBonus];
    public float MinigunReloadTime => (b_MinigunReloadTime * multipliers[EStatTypeMultiplier.MinigunReloadTimeMultiplier]) + flatBonuses[EStatTypeFlatBonus.MinigunReloadTimeFlatBonus];
    public int MinigunMagazineSize => (int)((b_MinigunMagazineSize * multipliers[EStatTypeMultiplier.MinigunMagazineSizeMultiplier]) + flatBonuses[EStatTypeFlatBonus.MinigunMagazineSizeFlatBonus]);
    public float MinigunProjectileLifetime => (b_MinigunProjectileLifetime * multipliers[EStatTypeMultiplier.MinigunProjectileLifetimeMultiplier]) + flatBonuses[EStatTypeFlatBonus.MinigunProjectileLifetimeFlatBonus];
    public float MinigunProjectileSpeed => (b_MinigunProjectileSpeed * multipliers[EStatTypeMultiplier.MinigunProjectileSpeedMultiplier]) + flatBonuses[EStatTypeFlatBonus.MinigunProjectileSpeedFlatBonus];
    public float MinigunBulletDeviationAngle => (b_minigunDeviationAngle * multipliers[EStatTypeMultiplier.MinigunBulletDeviationAngleMultiplier]) + flatBonuses[EStatTypeFlatBonus.MinigunBulletDeviationAngleBonus];

    // Rocket Computed Stats
    public float RocketDamage => (b_RocketDamage * multipliers[EStatTypeMultiplier.RocketDamageMultiplier]) + flatBonuses[EStatTypeFlatBonus.RocketDamageFlatBonus];
    public float RocketCritRate => (b_RocketCritRate * multipliers[EStatTypeMultiplier.RocketCritRateMultiplier]) + flatBonuses[EStatTypeFlatBonus.RocketCritRateFlatBonus];
    public float RocketCritDamage => (b_RocketCritDamage * multipliers[EStatTypeMultiplier.RocketCritDamageMultiplier]) + flatBonuses[EStatTypeFlatBonus.RocketCritDamageFlatBonus];
    public float RocketFireRate => (b_RocketFireRate * multipliers[EStatTypeMultiplier.RocketFireRateMultiplier]) + flatBonuses[EStatTypeFlatBonus.RocketFireRateFlatBonus];
    public float RocketReloadTime => (b_RocketReloadTime * multipliers[EStatTypeMultiplier.RocketReloadTimeMultiplier]) + flatBonuses[EStatTypeFlatBonus.RocketReloadTimeFlatBonus];
    public int RocketMagazineSize => (int)((b_RocketMagazineSize * multipliers[EStatTypeMultiplier.RocketMagazineSizeMultiplier]) + flatBonuses[EStatTypeFlatBonus.RocketMagazineSizeFlatBonus]);
    public float RocketProjectileLifetime => (b_RocketProjectileLifetime * multipliers[EStatTypeMultiplier.RocketProjectileLifetimeMultiplier]) + flatBonuses[EStatTypeFlatBonus.RocketProjectileLifetimeFlatBonus];
    public float RocketProjectileSpeed => (b_RocketProjectileSpeed * multipliers[EStatTypeMultiplier.RocketProjectileSpeedMultiplier]) + flatBonuses[EStatTypeFlatBonus.RocketProjectileSpeedFlatBonus];
    public float RocketExplosionRadius => (b_RocketExplosionRadius * multipliers[EStatTypeMultiplier.RocketExplosionRadiusMultiplier]) + flatBonuses[EStatTypeFlatBonus.RocketExplosionRadiusFlatBonus];
    public float RocketHoldDuration => (b_RocketHoldDuration * multipliers[EStatTypeMultiplier.RocketHoldDurationMultiplier]) + flatBonuses[EStatTypeFlatBonus.RocketHoldDurationFlatBonus];
    public float RocketReleaseFireRate => (b_RocketReleaseFirerate * multipliers[EStatTypeMultiplier.RocketReleaseFireRateMultiplier]) + flatBonuses[EStatTypeFlatBonus.RocketReleaseFireRateFlatBonus];


    #region --- Initialization Functions ---

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

    #endregion

    #region --- Roguelike functions ---

    /// <summary>
    /// Call to modify stat multiplier
    /// </summary>
    /// <param name="statType">Stat multiplier to be modified</param>
    /// <param name="multiplierValue">Value to modify the stat by</param>
    /// <param name="isPercent">If the value is in percent or not</param>
    public void ModifyMultiplier(EStatTypeMultiplier statType, float multiplierValue, bool isPercent)
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
    public void ModifyFlatBonus(EStatTypeFlatBonus statType, float flatBonusValue)
    {
        flatBonuses[statType] += flatBonusValue;
    }

    #endregion

    public void SpeedDebugger()
    {
        Debug.Log("movespeed base: " +  baseMoveSpeed);
        Debug.Log("movespeed mult: " +  multipliers[EStatTypeMultiplier.MoveSpeedMultiplier]);
        Debug.Log("movespeed flat: " + flatBonuses[EStatTypeFlatBonus.MoveSpeedFlatBonus]);
    }

}
