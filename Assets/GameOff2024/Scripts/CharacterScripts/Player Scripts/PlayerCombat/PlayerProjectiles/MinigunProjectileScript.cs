using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunProjectileParams : ProjectileParams
{
    public float speed;
    public float damage;
    public float lifetime;

    public MinigunProjectileParams(float speed, float damage, float lifetime)
    {
        this.speed = speed;
        this.damage = damage;
        this.lifetime = lifetime;
    }
}

public class MinigunProjectileScript : Projectile
{
    // Minigun Params
    private float speed;
    private float damage;
    private float lifetime;

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
        if (projectileParams is MinigunProjectileParams minigunProjectileParams)
        {
            speed = minigunProjectileParams.speed;
            damage = minigunProjectileParams.damage;
            lifetime = minigunProjectileParams.lifetime;
        }
        else
        {
            Debug.LogError("Must pass a MinigunProjectileParams class only as projectileParams");
            Debug.Break();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        returningToPool = true;
        ReturnToPool();
    }
}
