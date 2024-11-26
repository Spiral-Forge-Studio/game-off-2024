using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMinigunProjectileParams : ProjectileParams
{
    public float speed;
    public float damage;
    public float lifetime;
    public UniqueBuffHandler uniqueBuffHandler;
    public bool isCriticalHit;

    public BossMinigunProjectileParams(float speed, float damage, float lifetime, UniqueBuffHandler uniqueBuffHandler, bool isCriticalHit)
    {
        this.speed = speed;
        this.damage = damage;
        this.lifetime = lifetime;
        this.uniqueBuffHandler = uniqueBuffHandler;
        this.isCriticalHit = isCriticalHit;
    }
}

public class BossMinigunProjectileScript : Projectile
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

    public PlayerStatusManager playerStatusManager;
    public BossStatusManager bossStatusManager;

    protected override void OnEnable()
    {
        base.OnEnable();
        playerStatusManager = FindAnyObjectByType<PlayerStatusManager>();
        bossStatusManager = FindAnyObjectByType<BossStatusManager>();
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
        if (bossStatusManager.GetCurrentHealth() == 0)
        {
            returningToPool = true;
            ReturnToPool();
        }
    }

    public override void SetProjectileParams(ProjectileParams projectileParams)
    {
        if (projectileParams is MinigunProjectileParams BossMinigunProjectileParams)
        {
            speed = BossMinigunProjectileParams.speed;
            damage = BossMinigunProjectileParams.damage;
            lifetime = BossMinigunProjectileParams.lifetime;
            uniqueBuffHandler = BossMinigunProjectileParams.uniqueBuffHandler;
            isCritical = BossMinigunProjectileParams.isCriticalHit;
        }
        else
        {
            Debug.Log(projectileParams);
            Debug.LogError("Must pass a BossMinigunProjectileParams class only as projectileParams");
            Debug.Break();
        }
    }

    public float GetDamage()
    {
        return damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        //uniqueBuffHandler.ApplyMinigunOnHitUniqueBuffs(isCritical);
        if (other.gameObject.tag == "Player")
        {
            playerStatusManager.TakeDamage(GetDamage());
        }
        returningToPool = true;
        ReturnToPool();
    }
}
