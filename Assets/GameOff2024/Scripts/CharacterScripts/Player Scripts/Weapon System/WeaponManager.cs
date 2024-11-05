using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public WeaponStatsSO weaponStatsSO;

    private Minigun minigun;
    private Shotgun shotgun;
    private Rocket rocket;
    private Railgun railgun;

    private void Awake()
    {
        minigun = GetComponentInChildren<Minigun>();
        shotgun = GetComponentInChildren<Shotgun>();
        rocket = GetComponentInChildren<Rocket>();
        railgun = GetComponentInChildren<Railgun>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeBaseWeaponStats()
    {

    }
}
