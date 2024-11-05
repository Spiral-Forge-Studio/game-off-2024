using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSatsSO", menuName = "PlayerStatsSO")]
public class WeaponStatsSO : ScriptableObject
{
    [Header("Minigun Stats")]
    public float minigunDamage;
    public float minigunDamagePercent;
    public float minigunCritRate;
    public float minigunCritDamage;
    public float minigunFireRate;
    public float minigunReloadTime;
    public float minigunMagazineSize;
    public float minigunProjectileLifetime;

    [Header("Minigun Special Stats")]
    public float minigunBulletDeviation;

    [Header("Shotgun Stats")]
    public float shotgunDamage;
    public float shotgunDamagePercent;
    public float shotgunCritRate;
    public float shotgunCritDamage;
    public float shotgunFireRate;
    public float shotgunReloadTime;
    public float shotgunMagazineSize;
    public float shotgunProjectileLifetime;

    [Header("Shotgun Special Stats")]
    public float shotgunSpreadAngle;
    public float shotgunShotAmount;

    [Header("Rocket Stats")]
    public float rocketDamage;
    public float rocketDamagePercent;
    public float rocketCritRate;
    public float rocketCritDamage;
    public float rocketFireRate;
    public float rocketReloadTime;
    public float rocketMagazineSize;
    public float rocketProjectileLifetime;

    [Header("Rocket Special Stats")]
    public float rocketExplosionRadius;

    [Header("Railgun Stats")]
    public float railgunDamage;
    public float railgunDamagePercent;
    public float railgunCritRate;
    public float railgunCritDamage;
    public float railgunFireRate;
    public float railgunReloadTime;
    public float railgunMagazineSize;
    public float railgunProjectileLifetime;

    [Header("Railgun Special Stats")]
    public float railgunRange;
    public float railgunBeamWidth;
}
