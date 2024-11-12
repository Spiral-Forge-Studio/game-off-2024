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

    public RocketProjectileParams(float damage, float speed, float lifetime, float explosionRadius, Vector3 targetPos)
    {
        this.damage = damage;
        this.speed = speed;
        this.lifetime = lifetime;
        this.explosionRadius = explosionRadius;
        this.targetPos = targetPos;
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

    // lifetime logic variables
    private float startTime;
    private bool returningToPool;

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
        }
        else
        {
            Debug.LogError("Must pass a RocketProjectileParams class only as projectileParams");
            Debug.Break();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        returningToPool = true;
        ReturnToPool();

    }
}
