using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobPoolScript : MonoBehaviour
{
    public GameObject mobPrefab;
    public int initialSize;
    private Queue<GameObject> poolList = new Queue<GameObject>();

    public void Initialize()
    {
        poolList = new Queue<GameObject>();

        for (int i = 0; i < initialSize; i++)
        {
            GameObject mob = Instantiate(mobPrefab);

            MobPoolableScript mobPoolScriptObject = mob.GetComponent<MobPoolableScript>();
            mobPoolScriptObject.SetMobPool(this);

            mob.SetActive(false);
            poolList.Enqueue(mob);
        }
    }

    public GameObject GetMobObject()
    {
        if (poolList.Count > 0)
        {
            GameObject mob = poolList.Dequeue();
            //mob.SetActive(true);
            return mob;
        }
        else
        {
            GameObject newMob = Instantiate(mobPrefab);
            newMob.SetActive(false);
            return newMob;
        }
    }

    public void ReturnMobObject(GameObject mob)
    {
        mob.SetActive(false);
        poolList.Enqueue(mob);
    }
}
