using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerCombatInputs
{
    public bool LeftShoot;
    public bool RightShoot;
    public bool RightHold;
    public bool RightRelease;
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

    private float rocket_setHoldTime = 0.3f;
    private float rocket_startHoldTime;

    private bool rocket_holdTimerStarted;
    private bool rocket_holdReleased;


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
        if (inputs.RightRelease)
        {
            Debug.Log("release: " + inputs.RightRelease);
        }

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
                StartCoroutine(ReloadMinigun(playerStats.MinigunReloadTime));
            }
        }

        // Handle Rocket Shooting and Reloading
        if (inputs.RightShoot && !rocket_holdTimerStarted)
        {
            rocket_holdTimerStarted = true;
            rocket_startHoldTime = Time.time;
        }

        if (!inputs.RightShoot && rocket_holdTimerStarted)
        {
            if (Time.time - rocket_startHoldTime < rocket_setHoldTime)
            {
                if (rocket_currentAmmo > 0)
                {
                    if (Time.time - rocket_lastShotTime > 1f / playerStats.RocketFireRate && !inputs.RightHold)
                    {
                        FireRocketProjectile(aimPosition);
                        rocket_currentAmmo--;
                        rocket_lastShotTime = Time.time;
                    }
                }
            }

            rocket_holdTimerStarted = false;
        }

    }

    private IEnumerator ReloadMinigun(float reloadTime)
    {
        isMinigunReloading = true;

        //Debug.Log($"Reloading {weaponType}...");
        yield return new WaitForSeconds(reloadTime); // Wait for the reload duration

        minigun_currentAmmo = playerStats.MinigunMagazineSize; // Refill minigun ammo
        isMinigunReloading = false;

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
