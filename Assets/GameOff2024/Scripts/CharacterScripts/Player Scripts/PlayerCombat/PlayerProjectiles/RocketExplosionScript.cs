using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketExplosionScript : MonoBehaviour
{
    [HideInInspector] public float damage;
    [HideInInspector] public float radius;
    [HideInInspector] public UniqueBuffHandler uniqueBuffHandler;

    private EffectsPoolManager effectPoolManager;

    private void Awake()
    {
        effectPoolManager = FindObjectOfType<EffectsPoolManager>();
    }

    public float GetDamage()
    {
        return damage;
    }

    public void Explode()
    {
        gameObject.transform.localScale = Vector3.one*radius;
        GetComponent<SphereCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        effectPoolManager.SpawnRocketHitEffect(transform.position);
        StartCoroutine(DestroyAfterDelay(0.02f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
