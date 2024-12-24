using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobPoolManager : MonoBehaviour
{
    public MobPoolScript rocketMobPool;
    public MobPoolScript shotgunMobPool;
    public MobPoolScript rifleMobPool;

    private void Awake()
    {
        if (!rocketMobPool || !shotgunMobPool || !rifleMobPool)
        {
            Debug.LogError("mob pool missing");
            Debug.Break();
        }

        rocketMobPool.Initialize();
        shotgunMobPool.Initialize();
        rifleMobPool.Initialize();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SpawnRocketHitEffect(Vector3 spawnPos)
    {
        GameObject rocketMobObject = rocketMobPool.GetMobObject();
        rocketMobObject.transform.position = spawnPos;
    }

    public void SpawnMinigunHitEffect(Vector3 spawnPos)
    {
        GameObject shotgunMobObject = shotgunMobPool.GetMobObject();
        shotgunMobObject.transform.position = spawnPos;

    }

    public void SpawnMobExplodeEffect(Vector3 spawnPos)
    {
        GameObject rifleMobObject = rifleMobPool.GetMobObject();
        rifleMobObject.transform.position = spawnPos;
    }
}
