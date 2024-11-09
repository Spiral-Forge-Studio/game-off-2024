using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerCombatInputs
{
    public bool LeftShoot;
    public bool RightShoot;
    public bool LeftSwap;
    public bool RightSwap;
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


    private void Awake()
    {
        FindAnyObjectByType<ProjectileManager>().AddProjectileShooter(minigunProjectileShooter);
        FindAnyObjectByType<ProjectileManager>().AddProjectileShooter(rocketProjectileShooter);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // some logic
    }

    public void InitializeBaseWeaponStats()
    {

        
    }

    public void SetInputs(ref PlayerCombatInputs inputs)
    {
        aimPosition = inputs.mousePos;

        if (inputs.LeftShoot)
        {
            if (Time.time - minigun_lastShotTime > 1f / playerStats.MinigunFireRate)
            {
                FireMinigunProjectile(aimPosition);

                minigun_lastShotTime = Time.time;
            }
        }
        if (inputs.RightShoot)
        {
            if (Time.time - rocket_lastShotTime > 1f / playerStats.RocketFireRate)
            {
                FireRocketProjectile(aimPosition);

                rocket_lastShotTime = Time.time;
            }
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

    #endregion

    #region -- Rocket Related ---
    private void FireRocketProjectile(Vector3 aimPosition)
    {
        // Calculate the direction to the target
        Vector3 aimDirection = aimPosition - rocketProjectileShooter.firePoint.position;

        // Fire the projectile with the deviated direction
        rocketProjectileShooter.FireProjectile(Quaternion.LookRotation(Vector3.ProjectOnPlane(aimDirection, rocketProjectileShooter.firePoint.up)));
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(minigunProjectileShooter.firePoint.position, aimPosition);
        Gizmos.DrawLine(rocketProjectileShooter.firePoint.position, aimPosition);
    }
}
