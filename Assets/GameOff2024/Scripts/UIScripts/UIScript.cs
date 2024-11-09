using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [SerializeField] private TMP_Text minigunCurrentAmmo;
    [SerializeField] private TMP_Text minigunMagazineSize;
    [SerializeField] private TMP_Text rocketCurrentAmmo;
    [SerializeField] private TMP_Text rocketMagazineSize;

    public PlayerStatusSO playerStatusSO;

    private WeaponManager weaponManager;


    // Start is called before the first frame update
    void Start()
    {
        weaponManager = FindObjectOfType<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        minigunCurrentAmmo.text = weaponManager.GetMinigunAmmo().ToString();
        minigunMagazineSize.text = playerStatusSO.MinigunMagazineSize.ToString();

        rocketCurrentAmmo.text = weaponManager.GetRocketAmmo().ToString();
        rocketMagazineSize.text = playerStatusSO.RocketMagazineSize.ToString();
    }
}
