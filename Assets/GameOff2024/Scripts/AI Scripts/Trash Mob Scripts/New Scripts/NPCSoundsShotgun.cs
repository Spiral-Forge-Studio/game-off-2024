using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSoundsShotgun : MonoBehaviour
{
    public NPCWeaponType weaponType;
    public AudioSource _weaponsource;

    private void Awake()
    {
        _weaponsource = GetComponents<AudioSource>()[0];
    }
    private void Shoot()
    {
        AudioManager.instance.PlaySFX(_weaponsource, EGameplaySFX.MobShotgunFire, 0, true);
    }
}
