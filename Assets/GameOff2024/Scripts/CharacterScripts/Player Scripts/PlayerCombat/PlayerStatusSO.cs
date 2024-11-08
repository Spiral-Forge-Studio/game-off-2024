using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EStatTypeMultiplier
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
public enum EStatTypeFlatBonus
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
    public Dictionary<EStatTypeMultiplier, float> multipliers = new Dictionary<EStatTypeMultiplier, float>
    {
        { EStatTypeMultiplier.HealthMultiplier, 1f },
        { EStatTypeMultiplier.ShieldMultiplier, 1f },
        { EStatTypeMultiplier.ShieldRegenRateMultiplier, 1f },
        { EStatTypeMultiplier.ShieldRegenDelayMultiplier, 1f },
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
        { EStatTypeMultiplier.RocketExplosionRadiusMultiplier, 1f }
    };

    public Dictionary<EStatTypeFlatBonus, float> flatBonuses = new Dictionary<EStatTypeFlatBonus, float>
    {
        { EStatTypeFlatBonus.HealthFlatBonus, 0f },
        { EStatTypeFlatBonus.ShieldFlatBonus, 0f },
        { EStatTypeFlatBonus.ShieldRegenRateFlatBonus, 0f },
        { EStatTypeFlatBonus.ShieldRegenDelayFlatBonus, 0f },
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
        { EStatTypeFlatBonus.RocketExplosionRadiusFlatBonus, 0f }
    };

    // General Computed Stats
    public float Health => (baseHealth * multipliers[EStatTypeMultiplier.HealthMultiplier]) + flatBonuses[EStatTypeFlatBonus.HealthFlatBonus];
    public float Shield => (baseShield * multipliers[EStatTypeMultiplier.ShieldMultiplier]) + flatBonuses[EStatTypeFlatBonus.ShieldFlatBonus];
    public float ShieldRegenRate => (baseShieldRegenRate * multipliers[EStatTypeMultiplier.ShieldRegenRateMultiplier]) + flatBonuses[EStatTypeFlatBonus.ShieldRegenRateFlatBonus];
    public float ShieldRegenDelay => (baseShieldRegenDelay * multipliers[EStatTypeMultiplier.ShieldRegenDelayMultiplier]) + flatBonuses[EStatTypeFlatBonus.ShieldRegenDelayFlatBonus];
    public float DamageReduction => (baseDamageReduction * multipliers[EStatTypeMultiplier.DamageReductionMultiplier]) + flatBonuses[EStatTypeFlatBonus.DamageReductionFlatBonus];
    public float MoveSpeed => (baseMoveSpeed * multipliers[EStatTypeMultiplier.MoveSpeedMultiplier]) + flatBonuses[EStatTypeFlatBonus.MoveSpeedFlatBonus];
    public float DashCooldown => (baseDashCooldown * multipliers[EStatTypeMultiplier.DashCooldownMultiplier]) + flatBonuses[EStatTypeFlatBonus.DashCooldownFlatBonus];

    // Minigun Computed Stats
    public float MinigunDamage => (baseMinigunDamage * multipliers[EStatTypeMultiplier.MinigunDamageMultiplier]) + flatBonuses[EStatTypeFlatBonus.MinigunDamageFlatBonus];
    public float MinigunCritRate => (baseMinigunCritRate * multipliers[EStatTypeMultiplier.MinigunCritRateMultiplier]) + flatBonuses[EStatTypeFlatBonus.MinigunCritRateFlatBonus];
    public float MinigunCritDamage => (baseMinigunCritDamage * multipliers[EStatTypeMultiplier.MinigunCritDamageMultiplier]) + flatBonuses[EStatTypeFlatBonus.MinigunCritDamageFlatBonus];
    public float MinigunFireRate => (baseMinigunFireRate * multipliers[EStatTypeMultiplier.MinigunFireRateMultiplier]) + flatBonuses[EStatTypeFlatBonus.MinigunFireRateFlatBonus];
    public float MinigunReloadTime => (baseMinigunReloadTime * multipliers[EStatTypeMultiplier.MinigunReloadTimeMultiplier]) + flatBonuses[EStatTypeFlatBonus.MinigunReloadTimeFlatBonus];
    public float MinigunMagazineSize => (baseMinigunMagazineSize * multipliers[EStatTypeMultiplier.MinigunMagazineSizeMultiplier]) + flatBonuses[EStatTypeFlatBonus.MinigunMagazineSizeFlatBonus];
    public float MinigunProjectileLifetime => (baseMinigunProjectileLifetime * multipliers[EStatTypeMultiplier.MinigunProjectileLifetimeMultiplier]) + flatBonuses[EStatTypeFlatBonus.MinigunProjectileLifetimeFlatBonus];
    public float MinigunProjectileSpeed => (baseMinigunProjectileSpeed * multipliers[EStatTypeMultiplier.MinigunProjectileSpeedMultiplier]) + flatBonuses[EStatTypeFlatBonus.MinigunProjectileSpeedFlatBonus];
    public float MinigunBulletDeviationAngle => (baseminigunBulletDeviationAngle * multipliers[EStatTypeMultiplier.MinigunBulletDeviationAngleMultiplier]) + flatBonuses[EStatTypeFlatBonus.MinigunBulletDeviationAngleBonus];

    // Rocket Computed Stats
    public float RocketDamage => (baseRocketDamage * multipliers[EStatTypeMultiplier.RocketDamageMultiplier]) + flatBonuses[EStatTypeFlatBonus.RocketDamageFlatBonus];
    public float RocketCritRate => (baseRocketCritRate * multipliers[EStatTypeMultiplier.RocketCritRateMultiplier]) + flatBonuses[EStatTypeFlatBonus.RocketCritRateFlatBonus];
    public float RocketCritDamage => (baseRocketCritDamage * multipliers[EStatTypeMultiplier.RocketCritDamageMultiplier]) + flatBonuses[EStatTypeFlatBonus.RocketCritDamageFlatBonus];
    public float RocketFireRate => (baseRocketFireRate * multipliers[EStatTypeMultiplier.RocketFireRateMultiplier]) + flatBonuses[EStatTypeFlatBonus.RocketFireRateFlatBonus];
    public float RocketReloadTime => (baseRocketReloadTime * multipliers[EStatTypeMultiplier.RocketReloadTimeMultiplier]) + flatBonuses[EStatTypeFlatBonus.RocketReloadTimeFlatBonus];
    public float RocketMagazineSize => (baseRocketMagazineSize * multipliers[EStatTypeMultiplier.RocketMagazineSizeMultiplier]) + flatBonuses[EStatTypeFlatBonus.RocketMagazineSizeFlatBonus];
    public float RocketProjectileLifetime => (baseRocketProjectileLifetime * multipliers[EStatTypeMultiplier.RocketProjectileLifetimeMultiplier]) + flatBonuses[EStatTypeFlatBonus.RocketProjectileLifetimeFlatBonus];
    public float RocketProjectileSpeed => (baseRocketProjectileSpeed * multipliers[EStatTypeMultiplier.RocketProjectileSpeedMultiplier]) + flatBonuses[EStatTypeFlatBonus.RocketProjectileSpeedFlatBonus];
    public float RocketExplosionRadius => (baseRocketExplosionRadius * multipliers[EStatTypeMultiplier.RocketExplosionRadiusMultiplier]) + flatBonuses[EStatTypeFlatBonus.RocketExplosionRadiusFlatBonus];


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

}
