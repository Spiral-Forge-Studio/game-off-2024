using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerStatusManager : MonoBehaviour
{
    [SerializeField] private PlayerStatusSO playerStatus;
    private PlayerKCC playerKCC;

    private MinigunProjectileParams minigunProjectileParams;

    // Start is called before the first frame update
    void Start()
    {
        playerKCC = FindAnyObjectByType<PlayerKCC>();

        minigunProjectileParams = new MinigunProjectileParams(
            playerStatus.MinigunProjectileSpeed,
            GetComputedDamage(EWeaponType.Minigun),
            playerStatus.MinigunProjectileLifetime);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerKCCStats();
    }

    public MinigunProjectileParams GetMinigunProjectileParams()
    {
        minigunProjectileParams.damage = GetComputedDamage(EWeaponType.Minigun);
        minigunProjectileParams.speed = playerStatus.MinigunProjectileSpeed;
        minigunProjectileParams.lifetime = playerStatus.MinigunProjectileLifetime;


        return minigunProjectileParams;
    }

    private void UpdatePlayerKCCStats()
    {
        playerKCC._walkingSpeed = playerStatus.MoveSpeed;
    }

    public float GetComputedDamage(EWeaponType weaponType)
    {
        float roll = Random.Range(0, 100f);

        if (weaponType == EWeaponType.Minigun)
        {
            if (roll < playerStatus.MinigunCritRate * 100f)
            {
                return playerStatus.MinigunDamage * playerStatus.MinigunCritDamage;
            }
            else
            {
                return playerStatus.MinigunDamage;
            }
        }
        else if (weaponType == EWeaponType.Rocket)
        {
            if (roll < playerStatus.RocketCritRate * 100f)
            {
                return playerStatus.RocketDamage * playerStatus.RocketCritDamage;
            }
            else
            {
                return playerStatus.RocketDamage;
            }
        }
        else
        {
            Debug.LogError("No weapon of that type exists");
            Debug.Break();
            return 0;
        }

    }
}
