using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum EUniqueBuffs
{
    EnergySiphon,
    AdaptivePlating,
    NanoRegenPulse,
    CriticalStrikeOverload,
    FocusedPayload,
    OverdriveMode,
    MagnetizedPayload,
    ShieldOvercharge,

    DroneCompanion,
    SwarmRockets,
    ReflectiveArmor,
    DimensionalRift,
    EnergyVortexRockets,
    BerserkerMode,
    AmmoRecycler,
    PhaseShift,
    ReflectiveDash
}

public class UniqueBuffHandler : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private PlayerStatusSO playerStatsSO;
    [SerializeField] private UniqueBuffsSO playerUniqueBuffsSO;
    private PlayerStatusManager playerStatusManager;

    private Dictionary<EUniqueBuffs, UniqueBuff> uniqueBuffsLogicDict;
    private Dictionary<EUniqueBuffs, bool> uniqueBuffsActiveCheckDict;

    private void Awake()
    {
        uniqueBuffsLogicDict = new Dictionary<EUniqueBuffs, UniqueBuff>();
        uniqueBuffsActiveCheckDict = new Dictionary<EUniqueBuffs, bool>();

        playerStatusManager = FindObjectOfType<PlayerStatusManager>();
    }

    private void Start()
    {
        uniqueBuffsLogicDict.Add(EUniqueBuffs.EnergySiphon, new EnergySiphon(playerUniqueBuffsSO, playerStatsSO, playerStatusManager));
        uniqueBuffsActiveCheckDict.Add(EUniqueBuffs.EnergySiphon, true);
    }

    private void Update()
    {
        UpdateAllUniqueBuffsLogic();
    }

    private void UpdateAllUniqueBuffsLogic()
    {
        foreach (var key in uniqueBuffsLogicDict.Keys.ToList())
        {
            if (UniqueBuffIsActive(key))
            {
                uniqueBuffsLogicDict[key].UpdateBuff();
            }
        }
    }
    private bool UniqueBuffIsActive(EUniqueBuffs uniqueBuffEnum)
    {
        return uniqueBuffsActiveCheckDict[uniqueBuffEnum];
    }

    #region --- Public Functions (for outside use) ---

    public void ActivateUniqueBuff(EUniqueBuffs uniqueBuffEnum)
    {
        uniqueBuffsActiveCheckDict[uniqueBuffEnum] = true;
    }

    public void DeactivateUniqueBuff(EUniqueBuffs uniqueBuffEnum)
    {
        uniqueBuffsActiveCheckDict[uniqueBuffEnum] = false;
    }

    #endregion 

    #region --- Minigun Related ---

    public void ApplyMinigunOnHitUniqueBuffs(bool isCriticalHit)
    {
        if (isCriticalHit)
        {
            if (UniqueBuffIsActive(EUniqueBuffs.EnergySiphon))
            {
                uniqueBuffsLogicDict[EUniqueBuffs.EnergySiphon].ApplyBuffEffect();
            }
        }
    }


    #endregion
}
