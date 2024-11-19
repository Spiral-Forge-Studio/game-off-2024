using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private PlayerStatusManager playerStatusManager;

    private void Awake()
    {
        playerStatusManager = FindObjectOfType<PlayerStatusManager>();
    }
}
