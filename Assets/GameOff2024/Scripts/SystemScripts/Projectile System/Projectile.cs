using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EProjectileType
{
    Minigun,
    Rocket,
    Enemy

    // add your projectile types
}

public abstract class ProjectileParams {}

public abstract class Projectile : MonoBehaviour
{
    public EProjectileType projectileType;
    private ProjectilePoolScript projectilePool;
    public bool isActive;

    protected virtual void OnEnable()
    {
        isActive = true;
    }

    protected virtual void OnDisable()
    {
        isActive = false;
    }

    public void SetProjectilePool(ProjectilePoolScript projectilePool)
    {
        this.projectilePool = projectilePool;
    }

    protected void ReturnToPool()
    {
        if (projectilePool != null)
        {
            projectilePool.ReturnProjectile(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Must implement for the projectile parameters
    /// </summary>
    /// <param name="projectileParams"></param>
    public abstract void SetProjectileParams(ProjectileParams projectileParams);

    /// <summary>
    /// Must be implemented for uniform projectile movement update in the projectile manager class
    /// </summary>
    public abstract void ProjectileMovementUpdate();
}