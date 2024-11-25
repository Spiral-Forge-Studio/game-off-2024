using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketProjectileParams : ProjectileParams
{
    public float damage;
    public float speed;
    public float lifetime;
    public float explosionRadius;
    public Vector3 targetPos;
    public UniqueBuffHandler uniqueBuffHandler;
    public bool isCriticalHit;
    public GameObject rocketExplosionPrefab;

    public RocketProjectileParams(
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

public class RocketProjectileScript : Projectile
{
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
        if (projectileParams is RocketProjectileParams rocketProjectileParams)
        {
            speed = rocketProjectileParams.speed;
            damage = rocketProjectileParams.damage;
            lifetime = rocketProjectileParams.lifetime;
            explosionRadius = rocketProjectileParams.explosionRadius;
            targetPos = rocketProjectileParams.targetPos;
            uniqueBuffHandler = rocketProjectileParams.uniqueBuffHandler;
            isCriticalHit = rocketProjectileParams.isCriticalHit;
            rocketExplosionPrefab = rocketProjectileParams.rocketExplosionPrefab;

        }
        else
        {
            Debug.LogError("Must pass a RocketProjectileParams class only as projectileParams");
            Debug.Break();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject explosion = Instantiate(rocketExplosionPrefab, transform.position, Quaternion.identity);
        RocketExplosionScript explosionScript = explosion.GetComponent<RocketExplosionScript>();
        explosionScript.damage = damage;
        explosionScript.radius = explosionRadius;
        explosionScript.Explode();

        returningToPool = true;
        ReturnToPool();
    }
}
