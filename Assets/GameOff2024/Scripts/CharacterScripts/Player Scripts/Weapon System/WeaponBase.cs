using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWeapon
{
    Minigun,
    Shotgun,
    Railgun,
    Rocket
}

public class WeaponBase : MonoBehaviour
{
    [Header("Stats")]
    public float baseDamage;
    public float baseDamagePercent;
    public float baseCritRate;
    public float baseCritDamage;
    public float baseFireRate;
    public float baseReloadTime;
    public float baseMagazineSize;
    public float baseProjectileLifetime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
