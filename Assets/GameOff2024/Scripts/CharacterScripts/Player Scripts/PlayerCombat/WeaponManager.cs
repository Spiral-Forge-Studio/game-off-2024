using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerCombatInputs
{
    public bool LeftShoot;
    public bool RightShoot;
    public Vector3 mousePos;
}

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private ProjectileShooter minigunProjectileShooter;
    [SerializeField] private ProjectileShooter rocketProjectileShooter;

    [SerializeField] private PlayerStatusSO playerStats;

    private Vector3 aimPosition;
    private Quaternion aimRotation;

    // Firerate Timing Variables
    private float minigun_lastShotTime;
    private float rocket_lastShotTime;

    // Ammo
    private int minigun_currentAmmo;
    private int rocket_currentAmmo;

    private bool isMinigunReloading;
    private bool isRocketReloading;


    private void Awake()
    {
        FindAnyObjectByType<ProjectileManager>().AddProjectileShooter(minigunProjectileShooter);
        FindAnyObjectByType<ProjectileManager>().AddProjectileShooter(rocketProjectileShooter);
    }

    // Start is called before the first frame update
    void Start()
    {
        minigun_currentAmmo = playerStats.MinigunMagazineSize;
        rocket_currentAmmo = playerStats.RocketMagazineSize;
    }

    // Update is called once per frame
    void Update()
    {
        // some logic
    }

    public void SetInputs(ref PlayerCombatInputs inputs)
    {
        aimPosition = inputs.mousePos;

        // Handle Minigun Shooting and Reloading
        if (inputs.LeftShoot)
        {
            if (minigun_currentAmmo > 0)
            {
                if (Time.time - minigun_lastShotTime > 1f / playerStats.MinigunFireRate)
                {
                    FireMinigunProjectile(aimPosition);
                    minigun_currentAmmo--;
                    minigun_lastShotTime = Time.time;
                }
            }
            else if (!isMinigunReloading)
            {
                StartCoroutine(reload(playerStats.MinigunReloadTime, EWeaponType.Minigun));
            }
        }

        // Handle Rocket Shooting and Reloading
        if (inputs.RightShoot)
        {
            if (rocket_currentAmmo > 0)
            {
                if (Time.time - rocket_lastShotTime > 1f / playerStats.RocketFireRate)
                {
                    FireRocketProjectile(aimPosition);
                    rocket_currentAmmo--;
                    rocket_lastShotTime = Time.time;
                }
            }
            else if (!isRocketReloading)
            {
                StartCoroutine(reload(playerStats.RocketReloadTime, EWeaponType.Rocket));
            }
        }
    }

    private IEnumerator reload(float reloadTime, EWeaponType weaponType)
    {
        // Set the appropriate reload flag
        if (weaponType == EWeaponType.Minigun) isMinigunReloading = true;
        if (weaponType == EWeaponType.Rocket) isRocketReloading = true;

        Debug.Log($"Reloading {weaponType}...");
        yield return new WaitForSeconds(reloadTime); // Wait for the reload duration

        // Refill ammo and reset reload flag
        if (weaponType == EWeaponType.Minigun)
        {
            minigun_currentAmmo = playerStats.MinigunMagazineSize; // Refill minigun ammo
            isMinigunReloading = false;
            Debug.Log("Minigun reloaded!");
        }

        if (weaponType == EWeaponType.Rocket)
        {
            rocket_currentAmmo = playerStats.RocketMagazineSize; // Refill rocket ammo
            isRocketReloading = false;
            Debug.Log("Rocket launcher reloaded!");
        }
    }

    #region -- Minigun Related ---
    private void FireMinigunProjectile(Vector3 aimPosition)
    {
        float deviationAngle = playerStats.MinigunBulletDeviationAngle; // The max angle to deviate

        // Calculate the direction to the target
        Vector3 aimDirection = aimPosition - minigunProjectileShooter.firePoint.position;

        // Calculate a random deviation within the specified range
        float randomAngle = Random.Range(-deviationAngle, deviationAngle);

        // Create a rotation around the up axis (or another suitable axis based on your setup)
        Quaternion deviationRotation = Quaternion.AngleAxis(randomAngle, minigunProjectileShooter.gameObject.transform.up);

        // Apply the deviation to the aim direction
        Vector3 deviatedDirection = deviationRotation * aimDirection;

        // Fire the projectile with the deviated direction
        minigunProjectileShooter.FireProjectile(Quaternion.LookRotation(Vector3.ProjectOnPlane(deviatedDirection, minigunProjectileShooter.gameObject.transform.up)));
    }

    public int GetMinigunAmmo()
    {
        return minigun_currentAmmo;
    }

    #endregion

    #region -- Rocket Related ---
    private void FireRocketProjectile(Vector3 aimPosition)
    {
        // Calculate the direction to the target
        Vector3 aimDirection = aimPosition - rocketProjectileShooter.firePoint.position;

        // Fire the projectile with the deviated direction
        rocketProjectileShooter.FireProjectile(Quaternion.LookRotation(Vector3.ProjectOnPlane(aimDirection, rocketProjectileShooter.firePoint.up)));
    }

    public int GetRocketAmmo()
    {
        return rocket_currentAmmo;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(minigunProjectileShooter.firePoint.position, aimPosition);
        Gizmos.DrawLine(rocketProjectileShooter.firePoint.position, aimPosition);
    }
}
