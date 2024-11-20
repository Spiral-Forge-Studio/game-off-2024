using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UniqueBuff : MonoBehaviour
{
    protected UniqueBuffsSO uniqueBuffsSO;
    protected PlayerStatusSO playerStatusSO;
    protected PlayerStatusManager playerStatusManager;

    protected UniqueBuff(UniqueBuffsSO uniqueBuffsSO, PlayerStatusSO playerStatusSO, PlayerStatusManager playerStatusManager)
    {
        this.uniqueBuffsSO = uniqueBuffsSO;
        this.playerStatusSO = playerStatusSO;
        this.playerStatusManager = playerStatusManager;
    }

    public abstract void InitializeBuff();

    public abstract void UpdateBuff();

    public abstract void RemoveBuff();
    public abstract void ApplyBuffEffect();
}
