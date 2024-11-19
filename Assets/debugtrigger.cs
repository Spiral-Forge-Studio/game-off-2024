using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugtrigger : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
