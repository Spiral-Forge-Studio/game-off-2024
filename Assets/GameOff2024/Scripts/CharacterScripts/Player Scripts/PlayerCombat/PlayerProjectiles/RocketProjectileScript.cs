using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketProjectileParams : ProjectileParams
{
    public float damage;
    public float speed;
    public float lifetime;
    public float explosionRadius;

    public RocketProjectileParams(float damage, float speed, float lifetime, float explosionRadius)
    {
        this.damage = damage;
        this.speed = speed;
        this.lifetime = lifetime;
        this.explosionRadius = explosionRadius;
    }
}

public class RocketProjectileScript : Projectile
{
    // Minigun Params
    private float speed;
    private float damage;
    private float lifetime;
    private float explosionRadius;

    // lifetime logic variables
    private float startTime;
    private bool returningToPool;

    protected override void OnEnable()
    {
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
