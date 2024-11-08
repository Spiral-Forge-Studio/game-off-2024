using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public Transform firePoint;
    public EProjectileType projectileType;
    [HideInInspector] public bool fireProjectile;
    [HideInInspector] public Quaternion fireRotation;

    protected ProjectileShooter(bool fireProjectile = false)
    {
        this.fireProjectile = fireProjectile;
    }

    public void FireProjectile(Quaternion fireRotation)
    {
        fireProjectile = true;
        this.fireRotation = fireRotation;
    }
}
