using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEffects
{
    MinigunHitEffect,
    RocketExplosionEffect
}

public class EffectsPoolManager : MonoBehaviour
{
    public EffectsPoolScript minigunHitEffects;
    public EffectsPoolScript rocketHitEffects;
    public EffectsPoolScript mobExplodeEffects;

    private void Awake()
    {
        if (!minigunHitEffects || !rocketHitEffects || !mobExplodeEffects)
        {
            Debug.LogError("hit effect pool missing");
            Debug.Break();
        }

        minigunHitEffects.Initialize();
        rocketHitEffects.Initialize();
        mobExplodeEffects.Initialize();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SpawnRocketHitEffect(Vector3 spawnPos)
    {
        GameObject rocketHitEffectObject = rocketHitEffects.GetEffectObject();
        rocketHitEffectObject.transform.position = spawnPos;
        rocketHitEffectObject.GetComponent<EffectObjectScript>().PlayEffect();
    }

    public void SpawnMinigunHitEffect(Vector3 spawnPos)
    {
        GameObject minigunHitEffectObject = minigunHitEffects.GetEffectObject();
        minigunHitEffectObject.transform.position = spawnPos;
        minigunHitEffectObject.GetComponent<EffectObjectScript>().PlayEffect();

    }

    public void SpawnMobExplodeEffect(Vector3 spawnPos)
    {
        GameObject mobExplodeEffectObject = mobExplodeEffects.GetEffectObject();
        mobExplodeEffectObject.transform.position = spawnPos;
        mobExplodeEffectObject.GetComponent <EffectObjectScript>().PlayEffect();
    }
}
