using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public Transform firePoint;
    public EProjectileType projectileType;
    [HideInInspector] public bool fireProjectile;
    [HideInInspector] public List<Quaternion> fireRotations = new List<Quaternion>();
    [HideInInspector] public int projectileAmount;

    protected ProjectileShooter(bool fireProjectile = false)
    {
        this.fireProjectile = fireProjectile;
    }

    public void FireProjectile(List<Quaternion> fireRotations)
    {
        fireProjectile = true;
        this.fireRotations = fireRotations;
        projectileAmount = fireRotations.Count;
    }
}
