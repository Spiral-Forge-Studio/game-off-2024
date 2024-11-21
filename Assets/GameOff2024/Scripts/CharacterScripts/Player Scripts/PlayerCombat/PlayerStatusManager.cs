using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TrashMobAI;


public class PlayerStatusManager : MonoBehaviour
{
    [Header("Base Stats Scriptable Object")]
    [SerializeField] private PlayerStatusSO playerStatus;

    [Header("Player Status")]
    [SerializeField] private float currentHealth;
    [SerializeField] private float currentShield;

    private PlayerKCC playerKCC;
    private WeaponManager weaponManager;
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

        weaponManager = FindObjectOfType<WeaponManager>();
        playerKCC = FindObjectOfType<PlayerKCC>();

        minigunProjectileParams = new MinigunProjectileParams(
            playerStatus.MinigunProjectileSpeed,
            GetComputedDamage(EWeaponType.Minigun, false),
            playerStatus.MinigunProjectileLifetime,
            uniqueBuffHandler,
            false);

        rocketProjectileParams = new RocketProjectileParams(
            playerStatus.RocketProjectileSpeed,
            GetComputedDamage(EWeaponType.Rocket, false),
            playerStatus.RocketProjectileLifetime,
            playerStatus.RocketExplosionRadius,
            Vector3.zero,
            uniqueBuffHandler,
            false,
            weaponManager.rocketExplosionPrefab);

        currentHealth = playerStatus.Health;
        currentMaxHealth = playerStatus.Health;
        currentShield = playerStatus.Shield;

        shieldRecoverRoutine = null;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerKCCStats();

        if (!shieldBroken && !regeneratingShield && currentShield != playerStatus.Shield)
        { 
            regeneratingShield = true;
            StartCoroutine(ShieldRegeneration());
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            TakeDamage(5f);
        }

        if (currentMaxHealth != playerStatus.Health)
        {
            float currentHealthMultiplier = playerStatus.Health/ currentMaxHealth;
            currentHealth *= currentHealthMultiplier;
            currentMaxHealth = playerStatus.Health;
        }

        if (currentMaxShield != playerStatus.Shield)
        {
            float currentShieldMultiplier = playerStatus.Shield / currentMaxShield;
            currentShield *= currentShieldMultiplier;
            currentMaxShield = playerStatus.Shield;
        }
    }


    #region -- Player Status Stuff ---

    private void UpdatePlayerKCCStats()
    {
        playerKCC._walkingSpeed = playerStatus.MoveSpeed;
        playerKCC._dashInternalCooldown = playerStatus.DashCooldown;
    }

    /// <summary>
    /// Called when you want the player to take damage
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

        float modifiedDamage = Mathf.Clamp(rawDamage * (1 - playerStatus.DamageReduction), 0, rawDamage*2f);

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
                currentHealth = Mathf.Clamp(currentHealth + carryOverDamage, 0, playerStatus.Health);
            }
            else
            {
                currentHealth = Mathf.Clamp(currentHealth - modifiedDamage, 0, playerStatus.Health);

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
        yield return new WaitForSeconds(playerStatus.ShieldRegenTickInterval);

        if (!shieldBroken)
        {
            currentShield = Mathf.Clamp(currentShield + playerStatus.ShieldRegenAmount, 0, playerStatus.Shield);
        }

        regeneratingShield = false;
    }

    private IEnumerator ShieldRecovery()
    {
        yield return new WaitForSeconds(playerStatus.ShieldBreakRecoveryDelay);
        currentShield += playerStatus.ShieldRegenAmount;
        shieldBroken = false;

    }

    #endregion

    #region --- Weapon Stuff ---
    public RocketProjectileParams GetRocketProjectileParams()
    {
        bool isCriticalHit = IsCriticalHit(EWeaponType.Rocket);

        rocketProjectileParams.damage = GetComputedDamage(EWeaponType.Rocket, isCriticalHit);
        rocketProjectileParams.speed = playerStatus.RocketProjectileSpeed;
        rocketProjectileParams.lifetime = playerStatus.RocketProjectileLifetime;
        rocketProjectileParams.explosionRadius = playerStatus.RocketExplosionRadius;
        rocketProjectileParams.targetPos = weaponManager.aimPosition; 
        rocketProjectileParams.isCriticalHit = isCriticalHit;

        return rocketProjectileParams;
    }

    public MinigunProjectileParams GetMinigunProjectileParams()
    {
        bool isCriticalHit = IsCriticalHit(EWeaponType.Minigun);

        minigunProjectileParams.damage = GetComputedDamage(EWeaponType.Minigun, isCriticalHit);
        minigunProjectileParams.speed = playerStatus.MinigunProjectileSpeed;
        minigunProjectileParams.lifetime = playerStatus.MinigunProjectileLifetime;
        minigunProjectileParams.isCriticalHit = isCriticalHit;

        return minigunProjectileParams;
    }

    private bool IsCriticalHit(EWeaponType weaponType)
    {
        float roll = Random.Range(0, 100f);

        if (weaponType == EWeaponType.Minigun)
        {
            if (roll < playerStatus.MinigunCritRate * 100f)
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
            if (roll < playerStatus.RocketCritRate * 100f)
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
                return playerStatus.MinigunDamage * playerStatus.MinigunCritDamage;
            }
            else
            {
                return playerStatus.MinigunDamage;
            }
        }
        else if (weaponType == EWeaponType.Rocket)
        {
            if (isCriticalHit)
            {
                return playerStatus.RocketDamage * playerStatus.RocketCritDamage;
            }
            else
            {
                return playerStatus.RocketDamage;
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
