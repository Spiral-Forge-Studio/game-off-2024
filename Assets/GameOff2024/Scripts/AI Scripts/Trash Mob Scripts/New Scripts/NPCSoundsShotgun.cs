using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSoundsShotgun : MonoBehaviour
{
    public NPCWeaponType weaponType;
    public AudioSource _weaponsource, _footsource;

    private void Awake()
    {
        _weaponsource = GetComponents<AudioSource>()[0];
        _footsource = GetComponents<AudioSource>()[1];
    }
    private void Shoot()
    {
        AudioManager.instance.PlaySFX(_weaponsource, EGameplaySFX.MobShotgunFire, 0, true);
    }

    private void Step()
    {
        AudioManager.instance.PlaySFX(_footsource, EGameplaySFX.MobWalk, 0, true);
    }
}
