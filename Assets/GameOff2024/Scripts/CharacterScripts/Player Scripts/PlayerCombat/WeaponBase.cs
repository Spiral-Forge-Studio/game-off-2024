using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWeaponType
{
    Minigun,
    Rocket
}

public class WeaponBase : MonoBehaviour
{
    private ProjectileShooter projectileShooter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetProjectileShooter(ProjectileShooter projectileShooter)
    {
        this.projectileShooter = projectileShooter;
    }
}
