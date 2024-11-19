using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketExplosionScript : MonoBehaviour
{
    [HideInInspector] public float damage;
    [HideInInspector] public float radius;
    [HideInInspector] public UniqueBuffHandler uniqueBuffHandler;

    public float GetDamage()
    {
        return damage;
    }

    public void Explode()
    {
        gameObject.transform.localScale = Vector3.one*radius;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;
    }
}
