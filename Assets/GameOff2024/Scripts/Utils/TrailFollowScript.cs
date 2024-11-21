using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailFollowScript : MonoBehaviour
{
    public Transform followTransform;

    private void Update()
    {
        transform.position = followTransform.position;
    }
}
