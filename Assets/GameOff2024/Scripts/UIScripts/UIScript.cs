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
    [SerializeField] private Image dashFill;


    [Header("Player Weapon UI")]
    [SerializeField] private TMP_Text minigunCurrentAmmo;
    [SerializeField] private TMP_Text minigunMagazineSize;
    [SerializeField] private TMP_Text rocketCurrentAmmo;
    [SerializeField] private TMP_Text rocketMagazineSize;
    [SerializeField] private TMP_Text rocketAccumulatedShots;

    [SerializeField] private Image rocketIcon;
    [SerializeField] private Image minigunIcon;
    [SerializeField] private Image reloadingMinigunIcon;

    [Header("Data References")]
    public PlayerStatusSO playerStatusSO;

    private WeaponManager weaponManager;
    private PlayerStatusManager playerStatusManager;

    private Color greenHealth;
    private Color orangeHealth;
    private Color redHealth;

    private Color dashChargingColor;
    private Color dashReadyColor;

    private Color reloadingMinigunColor;
    private Color normalMinigunColor;

    private Color bg_reloadingMinigunColor;
    private Color bg_normalMinigunColor;

    private float currentDashTimer;


    // Start is called before the first frame update
    void Start()
    {
        weaponManager = FindObjectOfType<WeaponManager>();
        playerStatusManager = FindObjectOfType<PlayerStatusManager>();

        greenHealth = new Color(93f/255f, 211f / 255f, 102f / 255f);
        orangeHealth = new Color(255f / 255f, 140f / 255f, 48f / 255f);
        redHealth = new Color(255f / 255f, 50f / 255f, 51f / 255f);

        dashChargingColor = new Color(1, 1, 1, 140f / 255f);
        dashReadyColor = new Color(1, 217f / 255, 0, 190f / 255f);

        normalMinigunColor = new Color(1, 1, 1, 150f / 255f);
        reloadingMinigunColor = new Color(1, 123f / 255f, 123f / 255f, 150f / 255f);

        bg_normalMinigunColor = new Color(1, 1, 1, 26f / 255f);
        bg_reloadingMinigunColor = new Color(1, 123f / 255f, 123f / 255f, 26f / 255f);
    }

    // Update is called once per frame
    void Update()
    {
        minigunCurrentAmmo.text = weaponManager.GetMinigunAmmo().ToString();
        minigunMagazineSize.text = playerStatusSO.MinigunMagazineSize.ToString();

        if (weaponManager.isMinigunReloading)
        {
            minigunIcon.color = reloadingMinigunColor;
            reloadingMinigunIcon.color = bg_reloadingMinigunColor;
        }
        else
        {
            minigunIcon.color = normalMinigunColor;
            reloadingMinigunIcon.color = bg_normalMinigunColor;
        }

        minigunIcon.fillAmount = (float)weaponManager.GetMinigunAmmo() / (float)playerStatusSO.MinigunMagazineSize;

        rocketCurrentAmmo.text = weaponManager.GetRocketAmmo().ToString();
        rocketMagazineSize.text = playerStatusSO.RocketMagazineSize.ToString();
        rocketAccumulatedShots.text = weaponManager.GetRocketAccumulatedShots().ToString();

        rocketIcon.fillAmount = weaponManager.rocketRearmFill;

        if (playerStatusManager.playerKCC._canUseMovementAbility)
        {
            dashFill.color = dashReadyColor;
            dashFill.fillAmount = 1f;
        }
        else
        {
            dashFill.color = dashChargingColor;
            dashFill.fillAmount = playerStatusManager.playerKCC.currentDashTime/playerStatusSO.DashCooldown;
        }

        float hpRatio = playerStatusManager.GetCurrentHealth() / playerStatusSO.Health;

        Debug.Log(hpRatio);

        if (hpRatio < 0.25f)
        {
            currentHealth.color = redHealth;
            healthFill.color = redHealth;
        }
        else if (hpRatio < 0.5f)
        {
            currentHealth.color = orangeHealth;
            healthFill.color = orangeHealth;

        }
        else
        {
            currentHealth.color = greenHealth;
            healthFill.color = greenHealth;
        }

        currentHealth.text = Mathf.RoundToInt(playerStatusManager.GetCurrentHealth()).ToString();
        currentShield.text = Mathf.RoundToInt(playerStatusManager.GetCurrentShield()).ToString();

        healthFill.fillAmount = playerStatusManager.GetCurrentHealth()/playerStatusSO.Health;
        shieldFill.fillAmount = playerStatusManager.GetCurrentShield()/playerStatusSO.Shield;
    }
}
