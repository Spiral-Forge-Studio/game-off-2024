using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TrashMobAI;


public class BossStatusManager : MonoBehaviour
{
    [Header("Base Stats Scriptable Object")]
    [SerializeField] private BossStatusSO BossStatus;

    [Header("Boss Status")]
    [SerializeField] private float currentHealth;
    [SerializeField] private float currentShield;

    public PlayerKCC BossKCC;
    public BossWeaponManager weaponManager;
    private UniqueBuffHandler uniqueBuffHandler;

    private MinigunProjectileParams minigunProjectileParams;
    private RocketProjectileParams rocketProjectileParams;


    //[Header("[DEBUG] Local Helper Variabes")]

    [HideInInspector] public bool minigun_landedCriticalHit;
    [HideInInspector] public bool rocket_landedCriticalHit;

    private bool shieldBroken;
    private bool regeneratingShield;
    private float carryOverDamage = 0f;

    private float currentMaxHealth;
    private float currentMaxShield;
    private Coroutine shieldRecoverRoutine;


    private void Awake()
    {
        uniqueBuffHandler = GetComponent<UniqueBuffHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        minigunProjectileParams = new MinigunProjectileParams(
            BossStatus.MinigunProjectileSpeed,
            GetComputedDamage(EWeaponType.Minigun, false),
            BossStatus.MinigunProjectileLifetime,
            uniqueBuffHandler,
            false);

        rocketProjectileParams = new RocketProjectileParams(
            BossStatus.RocketProjectileSpeed,
            GetComputedDamage(EWeaponType.Rocket, false),
            BossStatus.RocketProjectileLifetime,
            BossStatus.RocketExplosionRadius,
            Vector3.zero,
            uniqueBuffHandler,
            false,
            weaponManager.rocketExplosionPrefab);

        currentHealth = BossStatus.Health;
        currentMaxHealth = BossStatus.Health;
        currentShield = BossStatus.Shield;

        shieldRecoverRoutine = null;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBossKCCStats();

        if (!shieldBroken && !regeneratingShield && currentShield != BossStatus.Shield)
        { 
            regeneratingShield = true;
            StartCoroutine(ShieldRegeneration());
        }


        if (currentMaxHealth != BossStatus.Health)
        {
            float currentHealthMultiplier = BossStatus.Health/ currentMaxHealth;
            currentHealth *= currentHealthMultiplier;
            currentMaxHealth = BossStatus.Health;
        }

        if (currentMaxShield != BossStatus.Shield)
        {
            float currentShieldMultiplier = BossStatus.Shield / currentMaxShield;
            currentShield *= currentShieldMultiplier;
            currentMaxShield = BossStatus.Shield;
        }
    }


    #region -- Boss Status Stuff ---

    private void UpdateBossKCCStats()
    {
        BossKCC._walkingSpeed = BossStatus.MoveSpeed;
        BossKCC._dashInternalCooldown = BossStatus.DashCooldown;
    }

    /// <summary>
    /// Called when you want the Boss to take damage
    /// </summary>
    /// <param name="rawDamage"></param>
    public void TakeDamage(float rawDamage)
    {
        if (rawDamage < 0)
        {
            Debug.LogError("can't take negative damage, did you want to heal?");
            Debug.Break();
            return;
        }

        float modifiedDamage = Mathf.Clamp(rawDamage * (1 - BossStatus.DamageReduction), 0, rawDamage*2f);

        if (!shieldBroken)
        {
            currentShield -= modifiedDamage;

            if (currentShield <= 0)
            {
                carryOverDamage = currentShield;

                shieldBroken = true;
                currentShield = 0f;

                shieldRecoverRoutine = StartCoroutine(ShieldRecovery());
            }
        }

        if (shieldBroken)
        {
            if (carryOverDamage <= 0)
            {
                currentHealth = Mathf.Clamp(currentHealth + carryOverDamage, 0, BossStatus.Health);
            }
            else
            {
                currentHealth = Mathf.Clamp(currentHealth - modifiedDamage, 0, BossStatus.Health);

                if (shieldRecoverRoutine != null)
                {
                    StopCoroutine(shieldRecoverRoutine);
                    shieldRecoverRoutine = StartCoroutine(ShieldRecovery());
                }
            }

            carryOverDamage = 1f;
        }
    }

    /// <summary>
    /// Call to add to the current health (heals, lifesteal, etc.)
    /// </summary>
    /// <param name="health"></param>
    public void GainHealth(float health)
    {
        currentHealth = Mathf.Clamp(currentHealth + health, 0, currentMaxHealth);
    }

    /// <summary>
    /// Call to add to the current shield (shield buffs, energy siphon ability, etc.)
    /// </summary>
    /// <param name="shield"></param>
    public void GainShield(float shield)
    {
        currentShield = Mathf.Clamp(currentShield + shield, 0, currentMaxShield);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetCurrentShield()
    {
        return currentShield;
    }

    public float GetCurrentMaxHealth()
    {
        return currentMaxHealth;
    }
    public float GetCurrentMaxShield()
    {
        return currentMaxShield;
    }


    private IEnumerator ShieldRegeneration()
    {
        yield return new WaitForSeconds(BossStatus.ShieldRegenTickInterval);

        if (!shieldBroken)
        {
            currentShield = Mathf.Clamp(currentShield + BossStatus.ShieldRegenAmount, 0, BossStatus.Shield);
        }

        regeneratingShield = false;
    }

    private IEnumerator ShieldRecovery()
    {
        yield return new WaitForSeconds(BossStatus.ShieldBreakRecoveryDelay);
        currentShield += BossStatus.ShieldRegenAmount;
        shieldBroken = false;

    }

    #endregion

    #region --- Weapon Stuff ---
    public RocketProjectileParams GetRocketProjectileParams()
    {
        bool isCriticalHit = IsCriticalHit(EWeaponType.Rocket);

        rocketProjectileParams.damage = GetComputedDamage(EWeaponType.Rocket, isCriticalHit);
        rocketProjectileParams.speed = BossStatus.RocketProjectileSpeed;
        rocketProjectileParams.lifetime = BossStatus.RocketProjectileLifetime;
        rocketProjectileParams.explosionRadius = BossStatus.RocketExplosionRadius;
        rocketProjectileParams.targetPos = weaponManager.aimPosition; 
        rocketProjectileParams.isCriticalHit = isCriticalHit;

        return rocketProjectileParams;
    }

    public MinigunProjectileParams GetMinigunProjectileParams()
    {
        bool isCriticalHit = IsCriticalHit(EWeaponType.Minigun);

        minigunProjectileParams.damage = GetComputedDamage(EWeaponType.Minigun, isCriticalHit);
        minigunProjectileParams.speed = BossStatus.MinigunProjectileSpeed;
        minigunProjectileParams.lifetime = BossStatus.MinigunProjectileLifetime;
        minigunProjectileParams.isCriticalHit = isCriticalHit;

        return minigunProjectileParams;
    }

    private bool IsCriticalHit(EWeaponType weaponType)
    {
        float roll = Random.Range(0, 100f);

        if (weaponType == EWeaponType.Minigun)
        {
            if (roll < BossStatus.MinigunCritRate * 100f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (weaponType == EWeaponType.Rocket)
        {
            if (roll < BossStatus.RocketCritRate * 100f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private float GetComputedDamage(EWeaponType weaponType, bool isCriticalHit)
    {
        if (weaponType == EWeaponType.Minigun)
        {
            if (isCriticalHit)
            {
                return BossStatus.MinigunDamage * BossStatus.MinigunCritDamage;
            }
            else
            {
                return BossStatus.MinigunDamage;
            }
        }
        else if (weaponType == EWeaponType.Rocket)
        {
            if (isCriticalHit)
            {
                return BossStatus.RocketDamage * BossStatus.RocketCritDamage;
            }
            else
            {
                return BossStatus.RocketDamage;
            }
        }
        else
        {
            Debug.LogError("No weapon of that type exists");
            Debug.Break();
            return 0;
        }

    }

    #endregion
}
