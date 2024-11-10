using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashMobParameters : MonoBehaviour
{
    #region ---TrashMobCoreParameters---
    [Header ("TrashMob Parameters")]
    public float health = 100f;
    public float roamspeed = 1.5f;
    public float combatspeed = 5f;
    public float dodgeSpeed = 5f;
    public float dodgecooldown = 0.3f;
    public float detectCover = 10f;
    public float dodgeprobability = 0.3f;
    public float shootprobability = 0.7f;
    public float PlayerDetectionRadius = 15f;
    public float PlayerExitCombatRange = 7f;
    #endregion

    #region ---ScalingFactor---
    [Header("TrashMob Parameters Scale Factor")]
    public float healthfactor = 10f;
    public float speedfactor = 0.1f;
    public float dodgespeedfactor = 0.1f;
    #endregion

    //Scaling Function
    public void ScaleParams(int levelscleared)
    {
        health += levelscleared * healthfactor;
        combatspeed += levelscleared * speedfactor;
        dodgeSpeed += levelscleared * dodgespeedfactor;
    }
}

