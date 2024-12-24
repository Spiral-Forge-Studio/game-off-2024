using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCWeaponType
{
    Rifle,
    Shotgun,
    Rocket
}

public class WeaponParameters : MonoBehaviour
{
    [Header("Rifle Parameters")]
    public float rifledamage = 5f; // Bullet Damage
    public float riflespeed = 20f; // Bullet Speed
    public float riflelifetime = 3f; // Bullet LifeTime
    public float riflereload = 5f; // Reload Time
    public float riflefirerate = 1f; // Time Between Shots
    public int riflemagazinesize = 10; // Pool Size, goes to reload after reaching 0
    public float riflerange = 10f;

    [Header("Shotgun Parameters")]
    public float shotgundamage = 10f;
    public float shotgunspeed = 15f;
    public float shotgunlifetime = 1f;
    public float shotgunreload = 5f;
    public float shotgunfirerate = 1.5f;
    public int shotgunmagazinesize = 3;
    public float shotgunspreadangle = 2f;
    public float shotgunrange = 5f;

    [Header("Rocket Parameters")]
    public float rocketdamage = 20f;
    public float rocketspeed = 10f;
    public float rocketlifetime = 5f;
    public float rocketreload = 10f;
    public float rocketfirerate = 1f;
    public int rocketmagazinesize = 3;
    public float rocketAOE = 3f;
    public float rocketrange = 15f;

    // Store the original values of weapon parameters
    private float original_rifledamage, original_riflespeed, original_riflelifetime, original_riflereload, original_riflefirerate, original_riflemagazinesize, original_riflerange;
    private float original_shotgundamage, original_shotgunspeed, original_shotgunlifetime, original_shotgunreload, original_shotgunfirerate, original_shotgunmagazinesize, original_shotgunspreadangle, original_shotgunrange;
    private float original_rocketdamage, original_rocketspeed, original_rocketlifetime, original_rocketreload, original_rocketfirerate, original_rocketmagazinesize, original_rocketAOE, original_rocketrange;

    private void Awake()
    {
        StoreOriginalWeaponParams();
    }

    // Store original weapon parameters
    public void StoreOriginalWeaponParams()
    {
        original_rifledamage = rifledamage;
        original_riflespeed = riflespeed;
        original_riflelifetime = riflelifetime;
        original_riflereload = riflereload;
        original_riflefirerate = riflefirerate;
        original_riflemagazinesize = riflemagazinesize;
        original_riflerange = riflerange;

        original_shotgundamage = shotgundamage;
        original_shotgunspeed = shotgunspeed;
        original_shotgunlifetime = shotgunlifetime;
        original_shotgunreload = shotgunreload;
        original_shotgunfirerate = shotgunfirerate;
        original_shotgunmagazinesize = shotgunmagazinesize;
        original_shotgunspreadangle = shotgunspreadangle;
        original_shotgunrange = shotgunrange;

        original_rocketdamage = rocketdamage;
        original_rocketspeed = rocketspeed;
        original_rocketlifetime = rocketlifetime;
        original_rocketreload = rocketreload;
        original_rocketfirerate = rocketfirerate;
        original_rocketmagazinesize = rocketmagazinesize;
        original_rocketAOE = rocketAOE;
        original_rocketrange = rocketrange;
    }

    // Reset weapon parameters to their original values
    public void ResetWeaponParams()
    {
        rifledamage = original_rifledamage;
        riflespeed = original_riflespeed;
        riflelifetime = original_riflelifetime;
        riflereload = original_riflereload;
        riflefirerate = original_riflefirerate;
        riflemagazinesize = (int)original_riflemagazinesize;
        riflerange = original_riflerange;

        shotgundamage = original_shotgundamage;
        shotgunspeed = original_shotgunspeed;
        shotgunlifetime = original_shotgunlifetime;
        shotgunreload = original_shotgunreload;
        shotgunfirerate = original_shotgunfirerate;
        shotgunmagazinesize = (int)original_shotgunmagazinesize;
        shotgunspreadangle = original_shotgunspreadangle;
        shotgunrange = original_shotgunrange;

        rocketdamage = original_rocketdamage;
        rocketspeed = original_rocketspeed;
        rocketlifetime = original_rocketlifetime;
        rocketreload = original_rocketreload;
        rocketfirerate = original_rocketfirerate;
        rocketmagazinesize = (int)original_rocketmagazinesize;
        rocketAOE = original_rocketAOE;
        rocketrange = original_rocketrange;
    }
}
