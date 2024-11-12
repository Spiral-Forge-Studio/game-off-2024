using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerStatusManager : MonoBehaviour
{
    [Header("Base Stats Scriptable Object")]
    [SerializeField] private PlayerStatusSO playerStatus;

    [Header("Player Status")]
    [SerializeField] private float currentHealth;
    [SerializeField] private float currentShield;

    private PlayerKCC playerKCC;
    private WeaponManager weaponManager;

    private MinigunProjectileParams minigunProjectileParams;
    private RocketProjectileParams rocketProjectileParams;


    //[Header("[DEBUG] Local Helper Variabes")]
    private bool shieldBroken;
    private bool regeneratingShield;
    private float carryOverDamage = 0f;


    // Start is called before the first frame update
    void Start()
    {

        weaponManager = FindObjectOfType<WeaponManager>();
        playerKCC = FindObjectOfType<PlayerKCC>();

        minigunProjectileParams = new MinigunProjectileParams(
            playerStatus.MinigunProjectileSpeed,
            GetComputedDamage(EWeaponType.Minigun),
            playerStatus.MinigunProjectileLifetime);

        rocketProjectileParams = new RocketProjectileParams(
            playerStatus.RocketProjectileSpeed,
            GetComputedDamage(EWeaponType.Rocket),
            playerStatus.RocketProjectileLifetime,
            playerStatus.RocketExplosionRadius,
            Vector3.zero);

        currentHealth = playerStatus.Health;
        currentShield = playerStatus.Shield;
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
    }


    #region -- Player Status Stuff ---

    private void UpdatePlayerKCCStats()
    {
        playerKCC._walkingSpeed = playerStatus.MoveSpeed;
        playerKCC._dashInternalCooldown = playerStatus.DashCooldown;
    }

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

            if (currentShield < 0)
            {
                carryOverDamage = currentShield;
                shieldBroken = true;
                currentShield = 0f;

                StartCoroutine(ShieldRecovery());
            }
        }

        if (shieldBroken)
        {
            if (carryOverDamage != 0)
            {
                currentHealth = Mathf.Clamp(currentHealth - carryOverDamage, 0, playerStatus.Health);
            }
            else
            {
                currentHealth = Mathf.Clamp(currentHealth - damage, 0, playerStatus.Health);
            }

            carryOverDamage = 0f;
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

        shieldBroken = false;
    }

    #endregion

    #region --- Weapon Stuff ---
    public RocketProjectileParams GetRocketProjectileParams()
    {
        rocketProjectileParams.damage = GetComputedDamage(EWeaponType.Rocket);
        rocketProjectileParams.speed = playerStatus.RocketProjectileSpeed;
        rocketProjectileParams.lifetime = playerStatus.RocketProjectileLifetime;
        rocketProjectileParams.explosionRadius = playerStatus.RocketExplosionRadius;
        rocketProjectileParams.targetPos = weaponManager.aimPosition; 

        return rocketProjectileParams;
    }

    public MinigunProjectileParams GetMinigunProjectileParams()
    {
        minigunProjectileParams.damage = GetComputedDamage(EWeaponType.Minigun);
        minigunProjectileParams.speed = playerStatus.MinigunProjectileSpeed;
        minigunProjectileParams.lifetime = playerStatus.MinigunProjectileLifetime;

        return minigunProjectileParams;
    }

    public float GetComputedDamage(EWeaponType weaponType)
    {
        float roll = Random.Range(0, 100f);

        if (weaponType == EWeaponType.Minigun)
        {
            if (roll < playerStatus.MinigunCritRate * 100f)
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
            if (roll < playerStatus.RocketCritRate * 100f)
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
