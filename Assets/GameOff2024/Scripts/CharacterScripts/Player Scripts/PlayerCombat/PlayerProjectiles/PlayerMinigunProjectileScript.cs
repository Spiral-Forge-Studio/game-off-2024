using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class MinigunProjectileParams : ProjectileParams
{
    public float speed;
    public float damage;
    public float lifetime;
    public UniqueBuffHandler uniqueBuffHandler;
    public bool isCriticalHit;

    public MinigunProjectileParams(float speed, float damage, float lifetime, UniqueBuffHandler uniqueBuffHandler, bool isCriticalHit)
    { 
        this.speed = speed;
        this.damage = damage;
        this.lifetime = lifetime;
        this.uniqueBuffHandler = uniqueBuffHandler;
        this.isCriticalHit = isCriticalHit;
    }
}

public class PlayerMinigunProjectileScript : Projectile
{
    // Minigun Params
    private float speed;
    private float damage;
    private float lifetime;
    private bool isCritical;

    // Lifetime logic variables
    private float startTime;
    private bool returningToPool;

    private UniqueBuffHandler uniqueBuffHandler;
    private TrailRenderer trailRenderer;
    private BossStatusManager StatusManager;
    private EffectsPoolManager EffectsPoolManager;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        StatusManager = FindAnyObjectByType<BossStatusManager>();
        EffectsPoolManager = FindObjectOfType<EffectsPoolManager>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        returningToPool = false;
        startTime = Time.time;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        trailRenderer.Clear();
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
            uniqueBuffHandler = minigunProjectileParams.uniqueBuffHandler;
            isCritical = minigunProjectileParams.isCriticalHit;
        }
        else
        {
            Debug.LogError("Must pass a MinigunProjectileParams class only as projectileParams");
            Debug.Break();
        }
    }

    public float GetDamage()
    {
        return damage;
    }

    public bool GetIsCritical()
    {
        return isCritical;
    }

    private void OnTriggerEnter(Collider other)
    {
        uniqueBuffHandler.ApplyMinigunOnHitUniqueBuffs(isCritical);

        if (other.gameObject.tag == "BossHitBox")
        {
            StatusManager.TakeDamage(damage);
        }

        EffectsPoolManager.SpawnMinigunHitEffect(transform.position);

        returningToPool = true;
        ReturnToPool();
    }
}
