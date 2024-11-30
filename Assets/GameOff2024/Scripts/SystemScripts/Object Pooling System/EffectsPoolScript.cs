using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsPoolScript : MonoBehaviour
{
    public GameObject effectPrefab;
    public int initialSize;
    private Queue<GameObject> poolList = new Queue<GameObject>();

    public void Initialize()
    {
        poolList = new Queue<GameObject>();

        for (int i = 0; i < initialSize; i++)
        {
            GameObject effect = Instantiate(effectPrefab);

            EffectObjectScript effectObjectScript = effect.GetComponent<EffectObjectScript>();
            effectObjectScript.SetEffectPool(this);

            effect.SetActive(false);
            poolList.Enqueue(effect);
        }
    }

    public GameObject GetEffectObject()
    {
        if (poolList.Count > 0)
        {
            GameObject effect = poolList.Dequeue();
            effect.SetActive(true);
            return effect;
        }
        else
        {
            GameObject newEffect = Instantiate(effectPrefab);
            return newEffect;
        }
    }

    public void ReturnEffectObject(GameObject effect)
    {
        effect.SetActive(false);
        poolList.Enqueue(effect);
    }
}
