using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSoundsRifle : MonoBehaviour
{
    public NPCWeaponType weaponType;
    public AudioSource _weaponsource;

    private void Awake()
    {
        _weaponsource = GetComponents<AudioSource>()[0];
    }
    private void Shoot()
    {
        AudioManager.instance.PlaySFX(_weaponsource, EGameplaySFX.MobRifleFire, 0, true);
    }
}
