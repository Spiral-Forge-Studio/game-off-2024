using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySiphon : UniqueBuff
{
    public EnergySiphon(UniqueBuffsSO uniqueBuffsSO, PlayerStatusSO playerStatusSO, PlayerStatusManager playerStatusManager) : 
        base(uniqueBuffsSO, playerStatusSO, playerStatusManager)
    {

    }

    public override void ApplyBuffEffect()
    {
        float siphonedShield = playerStatusManager.GetCurrentMaxShield() * (uniqueBuffsSO.EnergySiphon_ShieldRestoredPercent / 100f);
        playerStatusManager.GainShield(siphonedShield);
    }

    public override void InitializeBuff()
    {

    }

    public override void RemoveBuff()
    {

    }

    public override void UpdateBuff()
    {

    }
}
