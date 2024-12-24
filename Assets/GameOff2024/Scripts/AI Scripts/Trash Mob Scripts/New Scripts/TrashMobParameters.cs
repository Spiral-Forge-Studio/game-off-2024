using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMobGrade
{
    Private,
    Corporal,
    Seargeant,
    Lieutenant,
    Captain,
    Major,
    General
}

public class TrashMobParameters : MonoBehaviour
{
    private WeaponParameters weaponParameters;
    private MaterialFlasher materialFlasher;
    private MobChangeMaterialGrade materialGradeChanger;

    #region ---TrashMobCoreParameters---
    [Header("TrashMob Parameters")]
    public float health = 100f;
    public float roamspeed = 1.5f;
    public float combatspeed = 5f;
    public float dodgeSpeed = 5f;
    public float dodgecooldown = 0.3f;
    public float detectCover = 10f;
    public float dodgeprobability = 0.3f;
    public float shootprobability = 0.7f;
    public float PlayerDetectionRadius = 15f;
    public float PlayerExitCombatRange = 4f;

    private float original_health = 100f;
    private float original_roamspeed = 1.5f;
    private float original_combatspeed = 5f;
    private float original_dodgeSpeed = 5f;
    private float original_dodgecooldown = 0.3f;
    private float original_detectCover = 10f;
    private float original_dodgeprobability = 0.3f;
    private float original_shootprobability = 0.7f;
    private float original_PlayerDetectionRadius = 15f;
    private float original_PlayerExitCombatRange = 4f;
    #endregion

    #region ---ScalingFactor---
    [Header("[Private] Parameters Scale Factor")]
    public float healthfactor_Private = 1.0f;
    public float speedfactor_Private = 1.0f;

    [Header("[Corporal] Parameters Scale Factor")]
    public float healthfactor_Corporal = 1.2f;
    public float speedfactor_Corporal = 1.1f;

    [Header("[Seargeant] Parameters Scale Factor")]
    public float healthfactor_Seargeant = 1.4f;
    public float speedfactor_Seargeant = 1.2f;

    [Header("[Lieutenant] Parameters Scale Factor")]
    public float healthfactor_Lieutenant = 1.6f;
    public float speedfactor_Lieutenant = 1.3f;

    [Header("[Captain] Parameters Scale Factor")]
    public float healthfactor_Captain = 1.8f;
    public float speedfactor_Captain = 1.4f;

    [Header("[Major] Parameters Scale Factor")]
    public float healthfactor_Major = 2.0f;
    public float speedfactor_Major = 1.5f;

    [Header("[General] Parameters Scale Factor")]
    public float healthfactor_General = 2.5f;
    public float speedfactor_General = 1.6f;

    [Header("TrashMob Weapon Params Scale Factor")]
    public float weaponFactor_Private = 1.0f;
    public float weaponFactor_Corporal = 1.5f;
    public float weaponFactor_Seargeant = 2.0f;
    public float weaponFactor_Lieutenant = 3.0f;
    public float weaponFactor_Captain = 4.0f;
    public float weaponFactor_Major = 5.5f;
    public float weaponFactor_General = 8.0f;
    #endregion

    private void Awake()
    {
        weaponParameters = GetComponentInChildren<WeaponParameters>();
        materialFlasher = GetComponent<MaterialFlasher>();
        materialGradeChanger = GetComponent<MobChangeMaterialGrade>();

        StoreOriginalParams();
    }

    private void StoreOriginalParams()
    {
        original_health = health;
        original_roamspeed = roamspeed;
        original_combatspeed = combatspeed;
        original_dodgeSpeed = dodgeSpeed;
        original_dodgecooldown = dodgecooldown;
        original_detectCover = detectCover;
        original_dodgeprobability = dodgeprobability;
        original_shootprobability = shootprobability;
        original_PlayerDetectionRadius = PlayerDetectionRadius;
        original_PlayerExitCombatRange = PlayerExitCombatRange;
    }

    public void ResetParams()
    {
        health = original_health;
        roamspeed = original_roamspeed;
        combatspeed = original_combatspeed;
        dodgeSpeed = original_dodgeSpeed;
        dodgecooldown = original_dodgecooldown;
        detectCover = original_detectCover;
        dodgeprobability = original_dodgeprobability;
        shootprobability = original_shootprobability;
        PlayerDetectionRadius = original_PlayerDetectionRadius;
        PlayerExitCombatRange = original_PlayerExitCombatRange;
    }


    // Scaling Function
    public void ScaleMobParams(EMobGrade mobGrade)
    {
        ResetParams();

        // Get the scaling factors for TrashMob core parameters
        float healthFactor = 1f;
        float speedFactor = 1f;

        switch (mobGrade)
        {
            case EMobGrade.Private:
                healthFactor = healthfactor_Private;
                speedFactor = speedfactor_Private;
                break;
            case EMobGrade.Corporal:
                healthFactor = healthfactor_Corporal;
                speedFactor = speedfactor_Corporal;
                break;
            case EMobGrade.Seargeant:
                healthFactor = healthfactor_Seargeant;
                speedFactor = speedfactor_Seargeant;
                break;
            case EMobGrade.Lieutenant:
                healthFactor = healthfactor_Lieutenant;
                speedFactor = speedfactor_Lieutenant;
                break;
            case EMobGrade.Captain:
                healthFactor = healthfactor_Captain;
                speedFactor = speedfactor_Captain;
                break;
            case EMobGrade.Major:
                healthFactor = healthfactor_Major;
                speedFactor = speedfactor_Major;
                break;
            case EMobGrade.General:
                healthFactor = healthfactor_General;
                speedFactor = speedfactor_General;
                break;
        }

        // Apply scaling to TrashMob core parameters
        health *= healthFactor;
        combatspeed *= speedFactor;

        // Scale Weapon Parameters
        if (weaponParameters != null)
        {
            ScaleWeaponParams(weaponParameters, mobGrade);
        }

        // Change Materials
        materialGradeChanger.ChangeMaterials(mobGrade);

        materialFlasher.InitializeMaterials();
    }

    private void ScaleWeaponParams(WeaponParameters weaponParams, EMobGrade mobGrade)
    {
        weaponParams.ResetWeaponParams();

        // Define scaling factors for each weapon based on the mob grade
        float weaponFactor = 1f;

        switch (mobGrade)
        {
            case EMobGrade.Private: weaponFactor = weaponFactor_Private; break;
            case EMobGrade.Corporal: weaponFactor = weaponFactor_Corporal; break;
            case EMobGrade.Seargeant: weaponFactor = weaponFactor_Seargeant; break;
            case EMobGrade.Lieutenant: weaponFactor = weaponFactor_Lieutenant; break;
            case EMobGrade.Captain: weaponFactor = weaponFactor_Captain; break;
            case EMobGrade.Major: weaponFactor = weaponFactor_Major; break;
            case EMobGrade.General: weaponFactor = weaponFactor_General; break;
        }

        // Scale Rifle parameters
        weaponParams.rifledamage *= weaponFactor;
        weaponParams.riflereload /= weaponFactor;
        weaponParams.riflefirerate /= weaponFactor;
        weaponParams.riflemagazinesize = Mathf.CeilToInt(weaponParams.riflemagazinesize * weaponFactor);

        // Scale Shotgun parameters
        weaponParams.shotgundamage *= weaponFactor;
        weaponParams.shotgunreload /= weaponFactor;
        weaponParams.shotgunfirerate /= weaponFactor;
        weaponParams.shotgunmagazinesize = Mathf.CeilToInt(weaponParams.shotgunmagazinesize * weaponFactor);

        // Scale Rocket parameters
        weaponParams.rocketdamage *= weaponFactor;
        weaponParams.rocketfirerate /= weaponFactor;
        weaponParams.rocketmagazinesize = Mathf.CeilToInt(weaponParams.rocketmagazinesize * weaponFactor);
    }
}
