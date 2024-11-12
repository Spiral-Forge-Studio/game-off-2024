using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [Header("Player Status")]
    [SerializeField] private TMP_Text currentHealth;
    [SerializeField] private Image healthFill;
    [SerializeField] private TMP_Text currentShield;
    [SerializeField] private Image shieldFill;


    [Header("Player Weapon UI")]
    [SerializeField] private TMP_Text minigunCurrentAmmo;
    [SerializeField] private TMP_Text minigunMagazineSize;
    [SerializeField] private TMP_Text rocketCurrentAmmo;
    [SerializeField] private TMP_Text rocketMagazineSize;

    [Header("Data References")]
    public PlayerStatusSO playerStatusSO;

    private WeaponManager weaponManager;
    private PlayerStatusManager playerStatusManager;


    // Start is called before the first frame update
    void Start()
    {
        weaponManager = FindObjectOfType<WeaponManager>();
        playerStatusManager = FindObjectOfType<PlayerStatusManager>();
    }

    // Update is called once per frame
    void Update()
    {
        minigunCurrentAmmo.text = weaponManager.GetMinigunAmmo().ToString();
        minigunMagazineSize.text = playerStatusSO.MinigunMagazineSize.ToString();

        rocketCurrentAmmo.text = weaponManager.GetRocketAmmo().ToString();
        rocketMagazineSize.text = playerStatusSO.RocketMagazineSize.ToString();

        currentHealth.text = playerStatusManager.GetCurrentHealth().ToString();
        currentShield.text = playerStatusManager.GetCurrentShield().ToString();

        healthFill.fillAmount = playerStatusManager.GetCurrentHealth()/playerStatusSO.Health;
    }
}
