using System.Collections;
using System.Threading;
using UnityEngine;

public class NPCProjectileShooter : MonoBehaviour
{
    public NPCPoolingScript poolingScript;
    public WeaponParameters weaponParameters;
    public NPCWeaponType weaponType;
    public AudioSource _soundsource;

    [HideInInspector] public int currentammo;
    [HideInInspector] public bool isreloading = false;
    [HideInInspector] public float reloadtime;
    [HideInInspector] public float lastShootTime;
    [HideInInspector] public float firerate;
    [HideInInspector] public float lifetime;
    [HideInInspector] public float bulletspeed;

    private void Start()
    {
        lastShootTime = Time.time;
    }

    private void Awake()
    {
        weaponParameters = GetComponentInParent<WeaponParameters>();
        _soundsource = GetComponent<AudioSource>();
        
        SetUpWeapon();
    }

    public void TryShoot(Vector3 targetPosition)
    {
        if (isreloading || currentammo <= 0) { return; }

        if(Time.time >= lastShootTime + firerate)
        {
            Shoot(targetPosition);
            lastShootTime = Time.time;
            currentammo--;
            //Debug.Log($"Shooting Projectile # " + currentammo);
            if(currentammo <= 0)
            {
                StartCoroutine(Reload());
            }
        }
    }
    
    public void Shoot(Vector3 targetPosition)
    {
        if (isreloading == false)
        {

            if (weaponType == NPCWeaponType.Shotgun)
            {
                AudioManager.instance.PlaySFX(_soundsource, EGameplaySFX.MobShotgunFire, 0, true);
                // Spread angle for the shotgun (adjust in WeaponParameters for fine-tuning)
                float spreadAngle = weaponParameters.shotgunspreadangle; // e.g., 10 degrees

                // Create 3 projectiles with spread
                for (int i = -1; i <= 1; i++)
                {
                    GameObject projectile = poolingScript.GetProjectile();
                    projectile.transform.position = transform.position;

                    // Calculate the spread rotation
                    Quaternion spreadRotation = Quaternion.Euler(0, i * spreadAngle, 0);
                    Vector3 spreadDirection = spreadRotation * (targetPosition - transform.position).normalized;
                    projectile.transform.rotation = Quaternion.LookRotation(spreadDirection);

                    // Initialize projectile based on weapon type and parameters
                    NPCProjectile projectileScript = projectile.GetComponentInParent<NPCProjectile>();
                    if (projectileScript == null) { Debug.LogError("Projectile not found"); }
                    projectileScript.Initialize(weaponType, weaponParameters, poolingScript);
                }
            }

            else 
            {
                AudioManager.instance.PlaySFX(_soundsource, EGameplaySFX.MobRifleFire, 0, true);
                GameObject projectile = poolingScript.GetProjectile();
                projectile.transform.position = transform.position;
                projectile.transform.LookAt(targetPosition);

                // Initialize projectile based on weapon type and parameters
                NPCProjectile projectileScript = projectile.GetComponentInParent<NPCProjectile>();
                if (projectileScript == null) { Debug.LogError("Projectile not found"); }
                projectileScript.Initialize(weaponType, weaponParameters, poolingScript);
            }
            
            
        }
    }

    private IEnumerator Reload()
    {
        isreloading = true;
        //Debug.Log("Reloading....");
        AudioManager.instance.PlaySFX(_soundsource, EGameplaySFX.MobWindup, 0, true);
        yield return new WaitForSeconds(reloadtime);
        SetUpWeapon();
        isreloading = false;
        //Debug.Log("Reload Complete");
    }

    private void SetUpWeapon()
    {
        switch (weaponType)
        {
            case NPCWeaponType.Rifle:
                currentammo = weaponParameters.riflemagazinesize;
                reloadtime = weaponParameters.riflereload;
                firerate = weaponParameters.riflefirerate;
                lifetime = weaponParameters.riflelifetime;
                bulletspeed = weaponParameters.riflespeed;
                break;
            case NPCWeaponType.Shotgun:
                currentammo = weaponParameters.shotgunmagazinesize;
                reloadtime = weaponParameters.shotgunreload;
                firerate = weaponParameters.shotgunfirerate;
                lifetime = weaponParameters.shotgunlifetime;
                bulletspeed = weaponParameters.shotgunspeed;
                break;
            case NPCWeaponType.Rocket:
                currentammo = weaponParameters.rocketmagazinesize;
                reloadtime = weaponParameters.rocketreload;
                firerate = weaponParameters.rocketfirerate;
                lifetime = weaponParameters.rocketlifetime;
                bulletspeed = weaponParameters.rocketspeed;
                break;

        }
    }
}
