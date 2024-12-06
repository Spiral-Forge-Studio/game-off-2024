using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTriggerScript : MonoBehaviour
{
    private PlatformScript platformScript;

    private void Awake()
    {
        platformScript = GetComponentInParent<PlatformScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            platformScript.StartLiftSequence();
        }
    }
}
