using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossUIScript : MonoBehaviour
{
    [Header("Boss Status")]
    [SerializeField] private TMP_Text currentHealth;
    [SerializeField] private Image healthFill;
    [SerializeField] private TMP_Text currentShield;
    [SerializeField] private Image shieldFill;


    [Header("Boss Weapon UI")]
    [SerializeField] private TMP_Text minigunCurrentAmmo;
    [SerializeField] private TMP_Text minigunMagazineSize;
    [SerializeField] private TMP_Text rocketCurrentAmmo;
    [SerializeField] private TMP_Text rocketMagazineSize;
    [SerializeField] private TMP_Text rocketAccumulatedShots;

    [Header("Data References")]
    public PlayerStatusSO BossStatusSO;

    private WeaponManager weaponManager;
    private BossStatusManager BossStatusManager;


    // Start is called before the first frame update
    void Start()
    {
        weaponManager = FindObjectOfType<WeaponManager>();
        BossStatusManager = FindObjectOfType<BossStatusManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //minigunCurrentAmmo.text = weaponManager.GetMinigunAmmo().ToString();
        //minigunMagazineSize.text = BossStatusSO.MinigunMagazineSize.ToString();

        //rocketCurrentAmmo.text = weaponManager.GetRocketAmmo().ToString();
        //rocketMagazineSize.text = BossStatusSO.RocketMagazineSize.ToString();
        //rocketAccumulatedShots.text = weaponManager.GetRocketAccumulatedShots().ToString();

        currentHealth.text = Mathf.RoundToInt(BossStatusManager.GetCurrentHealth()).ToString();
        //currentShield.text = Mathf.RoundToInt(BossStatusManager.GetCurrentShield()).ToString();

        healthFill.fillAmount = BossStatusManager.GetCurrentHealth()/BossStatusSO.Health;
        shieldFill.fillAmount = BossStatusManager.GetCurrentShield()/BossStatusSO.Shield;
    }
}
