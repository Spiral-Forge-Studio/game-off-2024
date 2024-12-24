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

    public void SpawnMob(EMobType mobType, EMobGrade mobGrade, int amount, Vector3 spawnPos)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject mobObject = null;

            switch (mobType)
            {
                case EMobType.rifle:
                    mobObject = rifleMobPool.GetMobObject();
                    break;

                case EMobType.shotgun:
                    mobObject = shotgunMobPool.GetMobObject();
                    break;

                case EMobType.rocket:
                    mobObject = rocketMobPool.GetMobObject();
                    break;
            }

            TrashMobParameters mobParams = mobObject.GetComponent<TrashMobParameters>();
            TrashMob mobScript = mobObject.GetComponent<TrashMob>();

            if (mobParams != null)
            {
                mobParams.ScaleMobParams(mobGrade);
                mobScript.InitializeMob();
            }
            else
            {
                Debug.LogError("No TrashMobParameters script attached");
                Debug.Break();
            }

            mobObject.transform.position = spawnPos;
            mobObject.SetActive(true);
        }
    }
}
