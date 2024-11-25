using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperBodyPositionScript : MonoBehaviour
{
    public Transform followTransform;
    [SerializeField] private float yOffset;

    void Update()
    {
        transform.position = followTransform.position + Vector3.up*yOffset;
    }
}
