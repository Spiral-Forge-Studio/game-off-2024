using System.Collections;
using System.Collections.Generic;
using System.Threading;
using KinematicCharacterController;
using UnityEngine;

public enum BuffScaling
{
    Flat,
    Linear,
    Exponential
}
public enum BuffRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}


public class BuffManager : MonoBehaviour
{
    public PlayerStatusSO playerStats;

    // Buff Rarity Appearance Rate
    public Dictionary<BuffRarity, float> RarityProb = new Dictionary<BuffRarity, float>
    {
        { BuffRarity.Common, 0.45f },
        { BuffRarity.Uncommon, 0.25f },
        { BuffRarity.Rare, 0.15f },
        { BuffRarity.Epic, 0.1f },
        { BuffRarity.Legendary, 0.05f }
    };

    // Initial Multiplier Bonus
    public Dictionary<EStatTypeMultiplier, float> Initmultipliers = new Dictionary<EStatTypeMultiplier, float>
    {
        { EStatTypeMultiplier.HealthMultiplier, 1.1f },
        { EStatTypeMultiplier.ShieldMultiplier, 1.15f },
        { EStatTypeMultiplier.ShieldRegenRateMultiplier, 1.05f },
        { EStatTypeMultiplier.ShieldRegenDelayMultiplier, 0.95f },
        { EStatTypeMultiplier.DamageReductionMultiplier, 1.05f },
        { EStatTypeMultiplier.MoveSpeedMultiplier, 1.01f },
        { EStatTypeMultiplier.DashCooldownMultiplier, 0.95f },
        { EStatTypeMultiplier.MinigunDamageMultiplier, 1.05f },
        { EStatTypeMultiplier.MinigunCritRateMultiplier, 1.02f },
        { EStatTypeMultiplier.MinigunCritDamageMultiplier, 1.1f },
        { EStatTypeMultiplier.MinigunFireRateMultiplier, 1.01f },
        { EStatTypeMultiplier.MinigunReloadTimeMultiplier, 0.95f },
        { EStatTypeMultiplier.MinigunMagazineSizeMultiplier, 1.1f },
        { EStatTypeMultiplier.MinigunProjectileLifetimeMultiplier, 1.05f },
        { EStatTypeMultiplier.MinigunProjectileSpeedMultiplier, 1.1f },
        { EStatTypeMultiplier.MinigunBulletDeviationAngleMultiplier, 0.95f },
        { EStatTypeMultiplier.RocketDamageMultiplier, 1.1f },
        { EStatTypeMultiplier.RocketCritRateMultiplier, 1.01f },
        { EStatTypeMultiplier.RocketCritDamageMultiplier, 1.15f },
        { EStatTypeMultiplier.RocketFireRateMultiplier, 1.01f },
        { EStatTypeMultiplier.RocketReloadTimeMultiplier, 0.95f },
        { EStatTypeMultiplier.RocketMagazineSizeMultiplier, 1.1f },
        { EStatTypeMultiplier.RocketProjectileLifetimeMultiplier, 1.1f },
        { EStatTypeMultiplier.RocketProjectileSpeedMultiplier, 1.1f },
        { EStatTypeMultiplier.RocketExplosionRadiusMultiplier, 1.1f }
    };

    // Initial Flat Bonus
    public Dictionary<EStatTypeFlatBonus, float> InitflatBonuses = new Dictionary<EStatTypeFlatBonus, float>
    {
        { EStatTypeFlatBonus.HealthFlatBonus, 50f },
        { EStatTypeFlatBonus.ShieldFlatBonus, 75f },
        { EStatTypeFlatBonus.ShieldRegenRateFlatBonus, 0.5f },
        { EStatTypeFlatBonus.ShieldRegenDelayFlatBonus, -0.5f },
        { EStatTypeFlatBonus.DamageReductionFlatBonus, -5f },
        { EStatTypeFlatBonus.MoveSpeedFlatBonus, 0.1f },
        { EStatTypeFlatBonus.DashCooldownFlatBonus, -0.2f },
        { EStatTypeFlatBonus.MinigunDamageFlatBonus, 5f },
        { EStatTypeFlatBonus.MinigunCritRateFlatBonus, 0.01f },
        { EStatTypeFlatBonus.MinigunCritDamageFlatBonus, 10f },
        { EStatTypeFlatBonus.MinigunFireRateFlatBonus, 0.5f },
        { EStatTypeFlatBonus.MinigunReloadTimeFlatBonus, -0.1f },
        { EStatTypeFlatBonus.MinigunMagazineSizeFlatBonus, 10f },
        { EStatTypeFlatBonus.MinigunProjectileLifetimeFlatBonus, 0.5f },
        { EStatTypeFlatBonus.MinigunProjectileSpeedFlatBonus, 10f },
        { EStatTypeFlatBonus.MinigunBulletDeviationAngleBonus, -1f },
        { EStatTypeFlatBonus.RocketDamageFlatBonus, 10f },
        { EStatTypeFlatBonus.RocketCritRateFlatBonus, 0.01f },
        { EStatTypeFlatBonus.RocketCritDamageFlatBonus, 15f },
        { EStatTypeFlatBonus.RocketFireRateFlatBonus, 0.25f },
        { EStatTypeFlatBonus.RocketReloadTimeFlatBonus, -0.15f },
        { EStatTypeFlatBonus.RocketMagazineSizeFlatBonus, 2f },
        { EStatTypeFlatBonus.RocketProjectileLifetimeFlatBonus, 0.75f },
        { EStatTypeFlatBonus.RocketProjectileSpeedFlatBonus, 10f },
        { EStatTypeFlatBonus.RocketExplosionRadiusFlatBonus, 0.5f }
    };

    // Consecutive Multiplier Bonus
    public Dictionary<EStatTypeMultiplier, float> ConsecutiveMultipliers = new Dictionary<EStatTypeMultiplier, float>
    {
        { EStatTypeMultiplier.HealthMultiplier, 1.05f },
        { EStatTypeMultiplier.ShieldMultiplier, 1.10f },
        { EStatTypeMultiplier.ShieldRegenRateMultiplier, 1.025f },
        { EStatTypeMultiplier.ShieldRegenDelayMultiplier, 0.975f },
        { EStatTypeMultiplier.DamageReductionMultiplier, 1.02f },
        { EStatTypeMultiplier.MoveSpeedMultiplier, 1.005f },
        { EStatTypeMultiplier.DashCooldownMultiplier, 0.975f },
        { EStatTypeMultiplier.MinigunDamageMultiplier, 1.02f },
        { EStatTypeMultiplier.MinigunCritRateMultiplier, 1.01f },
        { EStatTypeMultiplier.MinigunCritDamageMultiplier, 1.05f },
        { EStatTypeMultiplier.MinigunFireRateMultiplier, 1.005f },
        { EStatTypeMultiplier.MinigunReloadTimeMultiplier, 0.975f },
        { EStatTypeMultiplier.MinigunMagazineSizeMultiplier, 1.05f },
        { EStatTypeMultiplier.MinigunProjectileLifetimeMultiplier, 1.025f },
        { EStatTypeMultiplier.MinigunProjectileSpeedMultiplier, 1.05f },
        { EStatTypeMultiplier.MinigunBulletDeviationAngleMultiplier, 0.975f },
        { EStatTypeMultiplier.RocketDamageMultiplier, 1.05f },
        { EStatTypeMultiplier.RocketCritRateMultiplier, 1.005f },
        { EStatTypeMultiplier.RocketCritDamageMultiplier, 1.075f },
        { EStatTypeMultiplier.RocketFireRateMultiplier, 1.0025f },
        { EStatTypeMultiplier.RocketReloadTimeMultiplier, 0.975f },
        { EStatTypeMultiplier.RocketMagazineSizeMultiplier, 1.05f },
        { EStatTypeMultiplier.RocketProjectileLifetimeMultiplier, 1.05f },
        { EStatTypeMultiplier.RocketProjectileSpeedMultiplier, 1.05f },
        { EStatTypeMultiplier.RocketExplosionRadiusMultiplier, 1.05f }
    };

    // Consecutive Flat Bonus
    public Dictionary<EStatTypeFlatBonus, float> ConsecutiveFlatBonuses = new Dictionary<EStatTypeFlatBonus, float>
    {
        { EStatTypeFlatBonus.HealthFlatBonus, 25f },
        { EStatTypeFlatBonus.ShieldFlatBonus, 50f },
        { EStatTypeFlatBonus.ShieldRegenRateFlatBonus, 0.25f },
        { EStatTypeFlatBonus.ShieldRegenDelayFlatBonus, -0.25f },
        { EStatTypeFlatBonus.DamageReductionFlatBonus, -2f },
        { EStatTypeFlatBonus.MoveSpeedFlatBonus, 0.05f },
        { EStatTypeFlatBonus.DashCooldownFlatBonus, -0.1f },
        { EStatTypeFlatBonus.MinigunDamageFlatBonus, 2f },
        { EStatTypeFlatBonus.MinigunCritRateFlatBonus, 0.5f },
        { EStatTypeFlatBonus.MinigunCritDamageFlatBonus, 5f },
        { EStatTypeFlatBonus.MinigunFireRateFlatBonus, 0.25f },
        { EStatTypeFlatBonus.MinigunReloadTimeFlatBonus, -0.05f },
        { EStatTypeFlatBonus.MinigunMagazineSizeFlatBonus, 5f },
        { EStatTypeFlatBonus.MinigunProjectileLifetimeFlatBonus, 0.25f },
        { EStatTypeFlatBonus.MinigunProjectileSpeedFlatBonus, 5f },
        { EStatTypeFlatBonus.MinigunBulletDeviationAngleBonus, -0.5f },
        { EStatTypeFlatBonus.RocketDamageFlatBonus, 5f },
        { EStatTypeFlatBonus.RocketCritRateFlatBonus, 0.5f },
        { EStatTypeFlatBonus.RocketCritDamageFlatBonus, 7.5f },
        { EStatTypeFlatBonus.RocketFireRateFlatBonus, 0.1f },
        { EStatTypeFlatBonus.RocketReloadTimeFlatBonus, -0.1f },
        { EStatTypeFlatBonus.RocketMagazineSizeFlatBonus, 1f },
        { EStatTypeFlatBonus.RocketProjectileLifetimeFlatBonus, 0.5f },
        { EStatTypeFlatBonus.RocketProjectileSpeedFlatBonus, 5f },
        { EStatTypeFlatBonus.RocketExplosionRadiusFlatBonus, 0.25f }
    };

    // Flat Bonus Buff Scaling
    public Dictionary<EStatTypeFlatBonus, BuffScaling> FlatBonusScaling = new Dictionary<EStatTypeFlatBonus, BuffScaling>
    {
        { EStatTypeFlatBonus.HealthFlatBonus, BuffScaling.Linear },
        { EStatTypeFlatBonus.ShieldFlatBonus, BuffScaling.Linear },
        { EStatTypeFlatBonus.ShieldRegenRateFlatBonus, BuffScaling.Linear },
        { EStatTypeFlatBonus.ShieldRegenDelayFlatBonus, BuffScaling.Flat },
        { EStatTypeFlatBonus.DamageReductionFlatBonus, BuffScaling.Linear },
        { EStatTypeFlatBonus.MoveSpeedFlatBonus, BuffScaling.Linear },
        { EStatTypeFlatBonus.DashCooldownFlatBonus, BuffScaling.Flat },
        { EStatTypeFlatBonus.MinigunDamageFlatBonus, BuffScaling.Linear },
        { EStatTypeFlatBonus.MinigunCritRateFlatBonus, BuffScaling.Linear },
        { EStatTypeFlatBonus.MinigunCritDamageFlatBonus, BuffScaling.Exponential },
        { EStatTypeFlatBonus.MinigunFireRateFlatBonus, BuffScaling.Linear },
        { EStatTypeFlatBonus.MinigunReloadTimeFlatBonus, BuffScaling.Linear },
        { EStatTypeFlatBonus.MinigunMagazineSizeFlatBonus, BuffScaling.Linear },
        { EStatTypeFlatBonus.MinigunProjectileLifetimeFlatBonus, BuffScaling.Linear },
        { EStatTypeFlatBonus.MinigunProjectileSpeedFlatBonus, BuffScaling.Linear },
        { EStatTypeFlatBonus.MinigunBulletDeviationAngleBonus, BuffScaling.Flat },
        { EStatTypeFlatBonus.RocketDamageFlatBonus, BuffScaling.Linear },
        { EStatTypeFlatBonus.RocketCritRateFlatBonus, BuffScaling.Linear },
        { EStatTypeFlatBonus.RocketCritDamageFlatBonus, BuffScaling.Exponential },
        { EStatTypeFlatBonus.RocketFireRateFlatBonus, BuffScaling.Linear },
        { EStatTypeFlatBonus.RocketReloadTimeFlatBonus, BuffScaling.Linear },
        { EStatTypeFlatBonus.RocketMagazineSizeFlatBonus, BuffScaling.Linear },
        { EStatTypeFlatBonus.RocketProjectileLifetimeFlatBonus, BuffScaling.Linear },
        { EStatTypeFlatBonus.RocketProjectileSpeedFlatBonus, BuffScaling.Linear },
        { EStatTypeFlatBonus.RocketExplosionRadiusFlatBonus, BuffScaling.Linear }
    };

    // Multiplicative Bonus Buff Scaling
    public Dictionary<EStatTypeMultiplier, BuffScaling> MultiplierScaling = new Dictionary<EStatTypeMultiplier, BuffScaling>
    {
        { EStatTypeMultiplier.HealthMultiplier, BuffScaling.Linear },
        { EStatTypeMultiplier.ShieldMultiplier, BuffScaling.Exponential },
        { EStatTypeMultiplier.ShieldRegenRateMultiplier, BuffScaling.Linear },
        { EStatTypeMultiplier.ShieldRegenDelayMultiplier, BuffScaling.Linear },
        { EStatTypeMultiplier.DamageReductionMultiplier, BuffScaling.Linear },
        { EStatTypeMultiplier.MoveSpeedMultiplier, BuffScaling.Exponential },
        { EStatTypeMultiplier.DashCooldownMultiplier, BuffScaling.Linear },
        { EStatTypeMultiplier.MinigunDamageMultiplier, BuffScaling.Linear },
        { EStatTypeMultiplier.MinigunCritRateMultiplier, BuffScaling.Linear },
        { EStatTypeMultiplier.MinigunCritDamageMultiplier, BuffScaling.Exponential },
        { EStatTypeMultiplier.MinigunFireRateMultiplier, BuffScaling.Linear },
        { EStatTypeMultiplier.MinigunReloadTimeMultiplier, BuffScaling.Linear },
        { EStatTypeMultiplier.MinigunMagazineSizeMultiplier, BuffScaling.Exponential },
        { EStatTypeMultiplier.MinigunProjectileLifetimeMultiplier, BuffScaling.Linear },
        { EStatTypeMultiplier.MinigunProjectileSpeedMultiplier, BuffScaling.Linear },
        { EStatTypeMultiplier.MinigunBulletDeviationAngleMultiplier, BuffScaling.Linear },
        { EStatTypeMultiplier.RocketDamageMultiplier, BuffScaling.Linear },
        { EStatTypeMultiplier.RocketCritRateMultiplier, BuffScaling.Linear },
        { EStatTypeMultiplier.RocketCritDamageMultiplier, BuffScaling.Exponential },
        { EStatTypeMultiplier.RocketFireRateMultiplier, BuffScaling.Linear },
        { EStatTypeMultiplier.RocketReloadTimeMultiplier, BuffScaling.Linear },
        { EStatTypeMultiplier.RocketMagazineSizeMultiplier, BuffScaling.Exponential },
        { EStatTypeMultiplier.RocketProjectileLifetimeMultiplier, BuffScaling.Linear },
        { EStatTypeMultiplier.RocketProjectileSpeedMultiplier, BuffScaling.Linear },
        { EStatTypeMultiplier.RocketExplosionRadiusMultiplier, BuffScaling.Exponential }
    };

    // Flat Bonus Buff Rarity
    public Dictionary<EStatTypeFlatBonus, BuffRarity> FlatBonusRarityMapping = new Dictionary<EStatTypeFlatBonus, BuffRarity>
    {
        { EStatTypeFlatBonus.DamageReductionFlatBonus, BuffRarity.Epic },
        { EStatTypeFlatBonus.DashCooldownFlatBonus, BuffRarity.Uncommon },
        { EStatTypeFlatBonus.HealthFlatBonus, BuffRarity.Common },
        { EStatTypeFlatBonus.MinigunCritDamageFlatBonus, BuffRarity.Rare },
        { EStatTypeFlatBonus.MinigunCritRateFlatBonus, BuffRarity.Uncommon },
        { EStatTypeFlatBonus.MinigunDamageFlatBonus, BuffRarity.Common },
        { EStatTypeFlatBonus.MinigunFireRateFlatBonus, BuffRarity.Common },
        { EStatTypeFlatBonus.MinigunMagazineSizeFlatBonus, BuffRarity.Rare },
        { EStatTypeFlatBonus.MinigunProjectileLifetimeFlatBonus, BuffRarity.Epic },
        { EStatTypeFlatBonus.MinigunProjectileSpeedFlatBonus, BuffRarity.Common },
        { EStatTypeFlatBonus.MinigunReloadTimeFlatBonus, BuffRarity.Uncommon },
        { EStatTypeFlatBonus.MoveSpeedFlatBonus, BuffRarity.Common },
        { EStatTypeFlatBonus.RocketCritDamageFlatBonus, BuffRarity.Epic },
        { EStatTypeFlatBonus.RocketCritRateFlatBonus, BuffRarity.Uncommon },
        { EStatTypeFlatBonus.RocketDamageFlatBonus, BuffRarity.Rare },
        { EStatTypeFlatBonus.RocketExplosionRadiusFlatBonus, BuffRarity.Legendary },
        { EStatTypeFlatBonus.RocketFireRateFlatBonus, BuffRarity.Common },
        { EStatTypeFlatBonus.RocketMagazineSizeFlatBonus, BuffRarity.Rare },
        { EStatTypeFlatBonus.RocketProjectileLifetimeFlatBonus, BuffRarity.Epic },
        { EStatTypeFlatBonus.RocketProjectileSpeedFlatBonus, BuffRarity.Common },
        { EStatTypeFlatBonus.RocketReloadTimeFlatBonus, BuffRarity.Uncommon },
        { EStatTypeFlatBonus.ShieldFlatBonus, BuffRarity.Uncommon },
        { EStatTypeFlatBonus.ShieldRegenDelayFlatBonus, BuffRarity.Rare },
        { EStatTypeFlatBonus.ShieldRegenRateFlatBonus, BuffRarity.Rare }
    };

    // Multiplicative Bonus Buff Rarity
    public Dictionary<EStatTypeMultiplier, BuffRarity> MultiplierRarityMapping = new Dictionary<EStatTypeMultiplier, BuffRarity>
    {
        { EStatTypeMultiplier.DamageReductionMultiplier, BuffRarity.Epic },
        { EStatTypeMultiplier.DashCooldownMultiplier, BuffRarity.Uncommon },
        { EStatTypeMultiplier.HealthMultiplier, BuffRarity.Common },
        { EStatTypeMultiplier.MinigunBulletDeviationAngleMultiplier, BuffRarity.Uncommon },
        { EStatTypeMultiplier.MinigunCritDamageMultiplier, BuffRarity.Rare },
        { EStatTypeMultiplier.MinigunCritRateMultiplier, BuffRarity.Uncommon },
        { EStatTypeMultiplier.MinigunDamageMultiplier, BuffRarity.Common },
        { EStatTypeMultiplier.MinigunFireRateMultiplier, BuffRarity.Common },
        { EStatTypeMultiplier.MinigunMagazineSizeMultiplier, BuffRarity.Rare },
        { EStatTypeMultiplier.MinigunProjectileLifetimeMultiplier, BuffRarity.Epic },
        { EStatTypeMultiplier.MinigunProjectileSpeedMultiplier, BuffRarity.Common },
        { EStatTypeMultiplier.MinigunReloadTimeMultiplier, BuffRarity.Uncommon },
        { EStatTypeMultiplier.MoveSpeedMultiplier, BuffRarity.Common },
        { EStatTypeMultiplier.RocketCritDamageMultiplier, BuffRarity.Epic },
        { EStatTypeMultiplier.RocketCritRateMultiplier, BuffRarity.Uncommon },
        { EStatTypeMultiplier.RocketDamageMultiplier, BuffRarity.Rare },
        { EStatTypeMultiplier.RocketExplosionRadiusMultiplier, BuffRarity.Legendary },
        { EStatTypeMultiplier.RocketFireRateMultiplier, BuffRarity.Common },
        { EStatTypeMultiplier.RocketMagazineSizeMultiplier, BuffRarity.Rare },
        { EStatTypeMultiplier.RocketProjectileLifetimeMultiplier, BuffRarity.Epic },
        { EStatTypeMultiplier.RocketProjectileSpeedMultiplier, BuffRarity.Common },
        { EStatTypeMultiplier.RocketReloadTimeMultiplier, BuffRarity.Uncommon },
        { EStatTypeMultiplier.ShieldMultiplier, BuffRarity.Uncommon },
        { EStatTypeMultiplier.ShieldRegenDelayMultiplier, BuffRarity.Rare },
        { EStatTypeMultiplier.ShieldRegenRateMultiplier, BuffRarity.Rare }
    };

    // Buffrarity prob
    public Dictionary<BuffRarity, float> RarityProbabilityMapping = new Dictionary<BuffRarity, float>
    {
        { BuffRarity.Legendary, 0.05f },
        { BuffRarity.Epic, 0.1f },
        { BuffRarity.Rare, 0.15f },
        { BuffRarity.Uncommon, 0.25f },
        { BuffRarity.Common, 0.45f }
    };

    //for debugging rarity only
    public Dictionary<BuffRarity, float> RarityMultiplier = new Dictionary<BuffRarity, float>
    {
        { BuffRarity.Common, 1f },
        { BuffRarity.Uncommon, 1.25f },
        { BuffRarity.Rare, 1.5f },
        { BuffRarity.Epic, 2f },
        { BuffRarity.Legendary, 5f }
    };



    //Debugging
    EStatTypeFlatBonus bufftoapply = EStatTypeFlatBonus.MinigunProjectileSpeedFlatBonus;
    public PlayerKCC playerKCC;
    float bufftimer = 0f;
    public GameObject buffPrefab; 
    public Transform spawnPoint; 
    public float spawnInterval = 5f;
    float buffamount = 0f;
    float randomvalue = 0f;
    float cumulativeProbability = 0f;
    BuffRarity temprarity;
    void ApplyBuff()
    {
        if (InitflatBonuses.ContainsKey(bufftoapply))
        {
            cumulativeProbability = 0f;
            randomvalue = UnityEngine.Random.value;
            foreach (var rarity in RarityProbabilityMapping)
            {
                cumulativeProbability += rarity.Value;
                if (randomvalue <= cumulativeProbability)
                {
                    temprarity = rarity.Key;
                    buffamount = InitflatBonuses[bufftoapply] * RarityMultiplier[rarity.Key];
                    break;
                }
            }

            playerStats.ModifyFlatBonus(bufftoapply, buffamount);
            Debug.Log("Buff applied: " + bufftoapply + "+" + buffamount + " Buff rarity: " + temprarity + randomvalue +  " Current projectile speed: " + playerStats.MinigunProjectileSpeed + " Current fire rate: " + playerStats.MinigunFireRate);
            bufftimer = Time.time; 
            Destroy(gameObject); 
        }
        else
        {
            Debug.LogWarning("Buff type " + bufftoapply + " not found in InitflatBonuses.");
        }
    }

    void PerformSphereCast()
    {
        float radius = 0.5f; // TO DO: get radius from gameobject

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, radius, Vector3.forward, radius);
        foreach (RaycastHit hit in hits)
        {
            //Debug.Log("Buff collided with an object with the tag: " + hit.collider.gameObject.tag);
            if (hit.collider.CompareTag("Player"))
            {
                ApplyBuff();
            }
        }
    }


    private void Awake()
    {
        playerKCC = FindObjectOfType<PlayerKCC>();
        
        bufftimer = Time.time;

    }

    private void Update()
    {
        PerformSphereCast();
    }




}