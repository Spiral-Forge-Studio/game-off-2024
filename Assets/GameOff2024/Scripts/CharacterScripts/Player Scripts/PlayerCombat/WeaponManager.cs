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
    public bool Reload;
}

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Audio Sources")]
    [SerializeField] private AudioSource audioSource_MinigunFireSource1;
    [SerializeField] private AudioSource audioSource_MinigunFireSource2;
    [SerializeField] private AudioSource audioSource_MinigunReload;
    [SerializeField] private AudioSource audioSource_RocketFire;
    [SerializeField] private AudioSource audioSource_RocketRearm;
    [SerializeField] private AudioSource audioSource_RocketAccumulate;

    [Header("Projectile Shooters")]
    [SerializeField] private ProjectileShooter minigunProjectileShooter;
    [SerializeField] private ProjectileShooter rocketProjectileShooter;

    private List<Quaternion> minigunProjectileRotationsList = new List<Quaternion>();
    private List<Quaternion> rocketProjectileRotationsList = new List<Quaternion>();

    [Header("PlayerStatus SO")]
    [SerializeField] private PlayerStatusSO playerStats;

    [Header("Prefab Objects")]
    public GameObject rocketExplosionPrefab;

    [HideInInspector] public Vector3 aimPosition;

    // Firerate Timing Variables
    private float minigun_lastShotTime;
    private float rocket_lastShotTime;

    // Ammo
    private int minigun_currentAmmo;
    private int rocket_currentAmmo;

    private bool isMinigunReloading;
    private bool isRocketRearming;


    // Minigun audio stuff
    private bool useOtherSource;

    [Header("Rocket Rearm Mechanics")]
    [SerializeField] private float rocket_accumulationTimePerRocket;
    [Range(0f, 360f)]
    [SerializeField] private float coneAngle;

    private float rocket_setHoldTime = 0.3f;
    private float rocket_startHoldTime;
    private float rocket_startAccumulateTime;
    private int rocket_accumulatedShots;
    private int rocket_currentAmmoAtTimeOfAccumulation;

    private bool rocket_holdTimerStarted;
    private bool rocket_holdReleased;

    [HideInInspector] public float rocketRearmFill;

    [Header("Animation and Particles")]
    public Animator minigunAnimator;
    public ParticleSystem minigunMuzzleFlash;
    public ParticleSystem minigunMuzzleSmoke;
    public ParticleSystem rocketMuzzleSmoke;

    private bool minigunAnimPlaying;

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
        rocket_accumulatedShots = 0;
        rocket_holdReleased = true;
        isRocketRearming = false;
        useOtherSource = false;
        minigunAnimPlaying = false;

        rocketRearmFill = 1f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetInputs(ref PlayerCombatInputs inputs)
    {
        if (inputs.RightRelease)
        {
            //Debug.Log("release: " + inputs.RightRelease);
        }

        aimPosition = inputs.mousePos;

        if (inputs.Reload && !isMinigunReloading && minigun_currentAmmo < playerStats.MinigunMagazineSize)
        {
            isMinigunReloading = true;
            StartCoroutine(ReloadMinigun(playerStats.MinigunReloadTime));
        }

        // Handle Minigun Shooting and Reloading
        if (inputs.LeftShoot && !isMinigunReloading)
        {
            if (minigun_currentAmmo > 0)
            {
                if (!minigunAnimPlaying)
                {
                    minigunAnimator.Play("Firing");
                    minigunAnimPlaying = true;

                }
                //minigunAnimator.Play("Firing");
                minigunMuzzleFlash.Play();
                minigunMuzzleSmoke.Play();

                if (Time.time - minigun_lastShotTime > 1f / playerStats.MinigunFireRate)
                {

                    FireMinigunProjectile(aimPosition);
                    minigun_currentAmmo--;
                    minigun_lastShotTime = Time.time;

                    if (useOtherSource)
                    {
                        AudioManager.instance.PlaySFX(audioSource_MinigunFireSource1, EGameplaySFX.MinigunFire);
                        useOtherSource = false;

                    }
                    else
                    {
                        AudioManager.instance.PlaySFX(audioSource_MinigunFireSource2, EGameplaySFX.MinigunFire);
                        useOtherSource = true;
                    }
                }
            }
            else
            {
                isMinigunReloading = true;
                StartCoroutine(ReloadMinigun(playerStats.MinigunReloadTime));
            }
        }
        else
        {
            if (minigunAnimPlaying)
            {
                minigunAnimator.Play("Idle");
                minigunAnimPlaying= false;
            }

            minigunMuzzleFlash.Stop();
            minigunMuzzleSmoke.Stop();
        }
        

        // Handle Rocket Shooting and Rearming
        if (inputs.RightShoot && !rocket_holdTimerStarted)
        {
            rocket_holdTimerStarted = true;
            rocket_startHoldTime = Time.time;
        }

        if (rocket_currentAmmo > 0 || rocket_accumulatedShots > 0)
        {
            if (!inputs.RightShoot && rocket_holdTimerStarted)
            {
                if (Time.time - rocket_startHoldTime < rocket_setHoldTime)
                {
                    if (Time.time - rocket_lastShotTime > 1f / playerStats.RocketFireRate && !inputs.RightHold)
                    {
                        rocketMuzzleSmoke.Play();
                        FireRocketProjectiles(aimPosition, 1);
                        AudioManager.instance.PlaySFX(audioSource_RocketFire, EGameplaySFX.RocketFire);
                        rocket_currentAmmo--;
                        rocket_lastShotTime = Time.time;
                    }
                }

                rocket_startAccumulateTime = Time.time;
                rocket_currentAmmoAtTimeOfAccumulation = rocket_currentAmmo;
                rocket_holdTimerStarted = false;
            }

            if (inputs.RightHold)
            {
                rocket_holdReleased = false;

                if (Time.time - rocket_startAccumulateTime > rocket_accumulationTimePerRocket
                    && rocket_accumulatedShots < rocket_currentAmmoAtTimeOfAccumulation
                    && rocket_currentAmmoAtTimeOfAccumulation > 0)
                {
                    rocket_accumulatedShots++;
                    rocket_currentAmmo --;

                    //Debug.Log("Accumulating rockets");

                    if (rocket_accumulatedShots == rocket_currentAmmoAtTimeOfAccumulation)
                    {
                        AudioManager.instance.PlaySFX(audioSource_RocketRearm, EGameplaySFX.MinigunReload, 1);
                    }
                    else
                    {
                        AudioManager.instance.PlaySFX(audioSource_RocketRearm, EGameplaySFX.RocketRearm);
                    }

                    rocket_startAccumulateTime = Time.time;
                }
            }

            if (!inputs.RightHold && !rocket_holdReleased)
            {
                //Debug.Log("Releasing " + rocket_accumulatedShots + " rockets at once");


                FireRocketProjectiles(aimPosition, rocket_accumulatedShots);

                AudioManager.instance.PlaySFX(audioSource_RocketFire, EGameplaySFX.RocketFire);

                rocketMuzzleSmoke.Play();

                rocket_accumulatedShots = 0;

                rocket_holdTimerStarted = false;
                rocket_holdReleased = true;

                rocket_lastShotTime = Time.time;
            }
        }

        if (!isRocketRearming && !inputs.RightHold)
        {
            if (rocket_currentAmmo < playerStats.RocketMagazineSize)
            {
                StartCoroutine(RearmRocket(playerStats.RocketReloadTime));
            }
        }

    }

    private IEnumerator ReloadMinigun(float reloadTime)
    {
        AudioManager.instance.PlaySFX(audioSource_MinigunReload, EGameplaySFX.MinigunReload, 0);

        isMinigunReloading = true;

        //Debug.Log($"Reloading {weaponType}...");
        yield return new WaitForSeconds(reloadTime); // Wait for the reload duration

        AudioManager.instance.PlaySFX(audioSource_MinigunReload, EGameplaySFX.MinigunReload, 1);

        minigun_currentAmmo = playerStats.MinigunMagazineSize; // Refill minigun ammo
        isMinigunReloading = false;
    }

    public IEnumerator RearmRocket(float rearmTime)
    {
        rocketRearmFill = 0f;

        isRocketRearming = true;

        float elapsedTime = 0f;

        while (elapsedTime < rearmTime)
        {
            elapsedTime += Time.deltaTime;

            rocketRearmFill = elapsedTime / rearmTime;

            yield return null; // Wait for the next frame
        }

        rocketRearmFill = elapsedTime / rearmTime;

        if (rocket_holdReleased)
        {
            rocket_currentAmmo++;
        }

        isRocketRearming = false;
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

        // Project the deviated direction onto the horizontal plane defined by the up axis
        Vector3 horizontalDeviatedDirection = Vector3.ProjectOnPlane(deviatedDirection, minigunProjectileShooter.gameObject.transform.up).normalized;

        // Set the final rotation based on the horizontal deviated direction
        Quaternion finalRotation = Quaternion.LookRotation(horizontalDeviatedDirection, minigunProjectileShooter.gameObject.transform.up);

        // Clear previous rotations and add the new one
        minigunProjectileRotationsList.Clear();
        minigunProjectileRotationsList.Add(finalRotation);

        // Draw line to visualize the deviated aim direction
        Vector3 lineEndPoint = minigunProjectileShooter.firePoint.position + horizontalDeviatedDirection * 20f; // 10 units for visualization
        //Debug.DrawLine(minigunProjectileShooter.firePoint.position, lineEndPoint, new Color(1, 0.92f,0, 0.5f), 0.5f);

        // Fire the projectile with the deviated direction
        minigunProjectileShooter.FireProjectile(minigunProjectileRotationsList);
    }


    public int GetMinigunAmmo()
    {
        return minigun_currentAmmo;
    }

    #endregion

    #region -- Rocket Related ---
    private void FireRocketProjectiles(Vector3 aimPosition, int amount)
    {
        rocketProjectileRotationsList.Clear();

        // Calculate the direction to the target
        Vector3 aimDirection;

        if (amount > 1)
        {
            float angleStep = coneAngle / (amount - 1); // Divide spread evenly among rockets

            for (int i = 0; i < amount; i++)
            {
                // Calculate deviation angle: centered at aimPosition, evenly spread across the cone angle
                float deviation = -coneAngle / 2 + angleStep * i;

                // Rotate aimPosition by deviation angle around the upward axis
                Quaternion rotation = Quaternion.AngleAxis(deviation, Vector3.up);
                Vector3 deviatedAimDirection = rotation * (aimPosition - rocketProjectileShooter.firePoint.position);

                // Project the deviated direction onto the plane defined by firePoint.up to keep it horizontal
                Vector3 horizontalDeviatedAimDirection = Vector3.ProjectOnPlane(deviatedAimDirection, rocketProjectileShooter.firePoint.up).normalized;

                // Apply the horizontal deviation to get the final aim rotation
                Quaternion aimRotation = Quaternion.LookRotation(horizontalDeviatedAimDirection, rocketProjectileShooter.firePoint.up);

                rocketProjectileRotationsList.Add(aimRotation);

                // Draw line to visualize the horizontal deviated aim direction
                Vector3 lineEndPoint = rocketProjectileShooter.firePoint.position + horizontalDeviatedAimDirection * 20f; // 10 units for visualization
                Debug.DrawLine(rocketProjectileShooter.firePoint.position, lineEndPoint, new Color(0, 0, 1, 0.5f), 1f);
            }

        }
        else
        {
            aimDirection = aimPosition - rocketProjectileShooter.firePoint.position;
            Quaternion aimRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(aimDirection, rocketProjectileShooter.firePoint.up));
            rocketProjectileRotationsList.Add(aimRotation);
        }

        //Debug.Log("count " + rocketProjectileRotationsList.Count);

        // Fire the projectile with the deviated direction
        rocketProjectileShooter.FireProjectile(rocketProjectileRotationsList);
    }

    public int GetRocketAmmo()
    {
        return rocket_currentAmmo;
    }

    public int GetRocketAccumulatedShots()
    {
        return rocket_accumulatedShots;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(minigunProjectileShooter.firePoint.position, aimPosition);
        Gizmos.DrawLine(rocketProjectileShooter.firePoint.position, aimPosition);
    }
}
