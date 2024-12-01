using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperBodyPositionScript : MonoBehaviour
{
    public Transform followTransform;
    [SerializeField] private float yOffset;
    [SerializeField] private float smoothing;


    void Update()
    {
        transform.position = followTransform.position + Vector3.up * yOffset;

        //gameObject.transform.SetParent(followTransform, false);
        //transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * smoothing);
    }
}
