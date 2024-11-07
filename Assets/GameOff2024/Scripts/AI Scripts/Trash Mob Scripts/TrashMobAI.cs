using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TrashMobAI : MonoBehaviour
{
    public enum WeaponType { Rifle, Shotgun, RocketLauncher }
    public WeaponType weaponType;

    [Header("AI Settings")]
    public float HP = 100f;
    public float moveSpeed = 3.5f;
    public float dodgeSpeed = 5f;
    public float coverCheckRadius = 5f;

    private Transform player;
    private NavMeshAgent agent;
    private bool isDodging = false;

    [Header("Weapon Settings")]
    public GameObject rifleProjectilePrefab;
    public GameObject shotgunProjectilePrefab;
    public GameObject rocketProjectilePrefab;
    public Transform firePoint;

    private float nextShootTime = 0f;
    private float currentRange;
    private float currentReloadCooldown;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        // Set weapon properties based on the selected weapon type
        SetWeaponProperties();
    }

    private void Update()
    {
        if (HP <= 0) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if player is within weapon range and if cooldown has elapsed
        if (distanceToPlayer <= currentRange && !isDodging && Time.time >= nextShootTime)
        {
            Shoot();
            nextShootTime = Time.time + currentReloadCooldown;
        }
        else
        {
            RandomMovementOrCover();
        }
    }

    private void SetWeaponProperties()
    {
        // Retrieve the reload cooldown and weapon range based on weapon type
        switch (weaponType)
        {
            case WeaponType.Rifle:
                currentReloadCooldown = rifleProjectilePrefab.GetComponent<RifleProjectile>().reloadCooldown;
                currentRange = rifleProjectilePrefab.GetComponent<RifleProjectile>().weaponRange;
                break;

            case WeaponType.Shotgun:
                currentReloadCooldown = shotgunProjectilePrefab.GetComponent<ShotgunProjectile>().reloadCooldown;
                currentRange = shotgunProjectilePrefab.GetComponent<ShotgunProjectile>().weaponRange;
                break;
    
            case WeaponType.RocketLauncher:
                currentReloadCooldown = rocketProjectilePrefab.GetComponent<RocketProjectile>().reloadCooldown;
                currentRange = rocketProjectilePrefab.GetComponent<RocketProjectile>().weaponRange;
                break;
        }
    }

    private void Shoot()
    {
        GameObject projectilePrefab = null;

        // Choose the projectile prefab based on weapon type
        switch (weaponType)
        {
            case WeaponType.Rifle:
                projectilePrefab = rifleProjectilePrefab;
                break;
            case WeaponType.Shotgun:
                projectilePrefab = shotgunProjectilePrefab;
                break;
            case WeaponType.RocketLauncher:
                projectilePrefab = rocketProjectilePrefab;
                break;
        }

        if (projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // For the rocket projectile, aim it toward the player
            if (weaponType == WeaponType.RocketLauncher)
            {
                Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
                projectile.transform.rotation = Quaternion.LookRotation(directionToPlayer);
            }
        }
    }

    private void RandomMovementOrCover()
    {
        if (FindCover(out Vector3 coverPosition))
        {
            agent.SetDestination(coverPosition);
            StartCoroutine(StayInCover());
        }
        else
        {
            agent.SetDestination(RandomNavMeshPoint());
        }
    }

    private bool FindCover(out Vector3 coverPosition)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, coverCheckRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Cover"))
            {
                coverPosition = hitCollider.transform.position;
                return true;
            }
        }
        coverPosition = Vector3.zero;
        return false;
    }

    private IEnumerator StayInCover()
    {
        yield return new WaitForSeconds(2f);
        agent.isStopped = true;
        yield return new WaitForSeconds(2f);
        agent.isStopped = false;
    }

    private Vector3 RandomNavMeshPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 5f;
        randomDirection += transform.position;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 5f, NavMesh.AllAreas);
        return hit.position;
    }

    public void Dodge()
    {
        isDodging = true;
        Vector3 dodgeDirection = (transform.position - player.position).normalized;
        agent.speed = dodgeSpeed;
        agent.SetDestination(transform.position + dodgeDirection * 3f);
        StartCoroutine(ResetDodge());
    }

    private IEnumerator ResetDodge()
    {
        yield return new WaitForSeconds(1f);
        isDodging = false;
        agent.speed = moveSpeed;
    }
}

