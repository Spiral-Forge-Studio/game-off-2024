using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealthBar : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    private GameObject _cam;

    private void Awake()
    {
        _cam = GameObject.Find("Main Camera");
        _camera = _cam.GetComponent<Transform>();

        if(_cam == null) { Debug.Log("Camera Not Referenced");  }
    }
    void LateUpdate()
    {
        transform.LookAt(transform.position + _camera.forward);
    }
}
