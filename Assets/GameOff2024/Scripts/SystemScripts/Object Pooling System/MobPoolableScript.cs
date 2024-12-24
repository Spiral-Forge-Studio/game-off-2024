using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobPoolableScript : MonoBehaviour
{
    public MobPoolScript mobPool;

    private void Awake()
    {

    }

    public void SetMobPool(MobPoolScript mobPool)
    {
        this.mobPool = mobPool;
    }

    public void ReturnToPool()
    {
        if (mobPool != null)
        {
            mobPool.ReturnMobObject(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

