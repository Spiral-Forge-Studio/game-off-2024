using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;



public enum NPCWeaponType
{
    Rifle,
    Shotgun,
    Rocket
}



public class WeaponParameters : MonoBehaviour
{
    [Header ("Rifle Parameters")]
    public float rifledamage = 5f; //Bullet Damage
    public float riflespeed = 20f; //Bullet Speed
    public float riflelifetime = 3f; //Bullet LifeTime
    public float riflereload = 5f; // Reload Time
    public float riflefirerate = 1f; //Time Between Shots
    public int riflemagazinesize = 10; // Pool Size, goes to reload after reaching 0

    [Header("Shotgun Parameters")]
    public float shotgundamage = 10f;
    public float shotgunspeed = 15f;
    public float shotgunlifetime = 1f;
    public float shotgunreload = 5f;
    public float shotgunfirerate = 1.5f;
    public int shotgunmagazinesize = 3;
    public float shotgunspreadangle = 2f;

    [Header("Rocket Parameters")]
    public float rocketdamage = 20f;
    public float rocketspeed = 10f;
    public float rocketlifetime = 5f;
    public float rocketreload = 10f;
    public float rocketfirerate = 1f;
    public int rocketmagazinesize = 3;
    public float rocketAOE = 3f;

}

