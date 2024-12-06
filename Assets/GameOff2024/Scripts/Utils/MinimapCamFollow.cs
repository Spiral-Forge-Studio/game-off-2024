using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamFollow : MonoBehaviour
{

    [SerializeField] private Transform followTransform;
    [SerializeField] private Transform cameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraTransform.position = followTransform.position;
    }
}
