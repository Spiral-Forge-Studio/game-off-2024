using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRocketProjectileParams : ProjectileParams
{
    public float damage;
    public float speed;
    public float lifetime;
    public float explosionRadius;
    public Vector3 targetPos;
    public UniqueBuffHandler uniqueBuffHandler;
    public bool isCriticalHit;
    public GameObject rocketExplosionPrefab;

    public BossRocketProjectileParams(
        float damage, 
        float speed, 
        float lifetime, 
        float explosionRadius, 
        Vector3 targetPos , 
        UniqueBuffHandler uniqueBuffHandler, 
        bool isCriticalHit, 
        GameObject rocketExplosionPrefab)
    {
        this.damage = damage;
        this.speed = speed;
        this.lifetime = lifetime;
        this.explosionRadius = explosionRadius;
        this.targetPos = targetPos;
        this.uniqueBuffHandler = uniqueBuffHandler;
        this.isCriticalHit = isCriticalHit;
        this.rocketExplosionPrefab = rocketExplosionPrefab;
    }
}

public class BossRocketProjectileScript : Projectile
{
    public PlayerStatusManager playerStatusManager;
    // Minigun Params
    private float speed;
    private float damage;
    private float lifetime;
    private float explosionRadius;
    private Vector3 targetPos;
    private bool isCriticalHit;

    // lifetime logic variables
    private float startTime;
    private bool returningToPool;

    private UniqueBuffHandler uniqueBuffHandler;
    private GameObject rocketExplosionPrefab;

    [SerializeField] private AudioSource audioSource;

    protected override void OnEnable()
    {
        AudioManager.instance.PlaySFX(audioSource, EGameplaySFX.RocketFire, 1);
        base.OnEnable();
        playerStatusManager = FindAnyObjectByType<PlayerStatusManager>();
        returningToPool = false;
        startTime = Time.time;
    }

    public override void ProjectileMovementUpdate()
    {
        transform.position = transform.position + transform.forward * speed * Time.deltaTime;

        if (Time.time - startTime > lifetime && returningToPool == false)
        {
            returningToPool = true;
            ReturnToPool();
        }
    }

    public override void SetProjectileParams(ProjectileParams projectileParams)
    {
        if (projectileParams is RocketProjectileParams BossRocketProjectileParams)
        {
            speed = BossRocketProjectileParams.speed;
            damage = BossRocketProjectileParams.damage;
            lifetime = BossRocketProjectileParams.lifetime;
            explosionRadius = BossRocketProjectileParams.explosionRadius;
            targetPos = BossRocketProjectileParams.targetPos;
            uniqueBuffHandler = BossRocketProjectileParams.uniqueBuffHandler;
            isCriticalHit = BossRocketProjectileParams.isCriticalHit;
            rocketExplosionPrefab = BossRocketProjectileParams.rocketExplosionPrefab;

        }
        else
        {
            Debug.LogError("Must pass a BossRocketProjectileParams class only as projectileParams");
            Debug.Break();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            playerStatusManager.TakeDamage(this.damage);
            GameObject explosion = Instantiate(rocketExplosionPrefab, transform.position, Quaternion.identity);
            RocketExplosionScript explosionScript = explosion.GetComponent<RocketExplosionScript>();
            explosionScript.damage = damage;
            explosionScript.radius = explosionRadius;
            explosionScript.Explode();
            returningToPool = true;
            ReturnToPool();
        }

        
    }
}
