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
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        if (damage < 0)
        {
            Debug.LogError("can't take negative damage, did you want to heal?");
            Debug.Break();
            return;
        }

        if (!shieldBroken)
        {
            currentShield -= damage;

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
                currentHealth = Mathf.Clamp(currentHealth - damage, 0, playerStatus.Health);

                if (shieldRecoverRoutine != null)
                {
                    StopCoroutine(shieldRecoverRoutine);
                    shieldRecoverRoutine = StartCoroutine(ShieldRecovery());
                }
            }

            carryOverDamage = 1f;
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetCurrentShield()
    {
        return currentShield;
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

    public bool IsCriticalHit(EWeaponType weaponType)
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

    public float GetComputedDamage(EWeaponType weaponType, bool isCriticalHit)
    {
        float roll = Random.Range(0, 100f);

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
