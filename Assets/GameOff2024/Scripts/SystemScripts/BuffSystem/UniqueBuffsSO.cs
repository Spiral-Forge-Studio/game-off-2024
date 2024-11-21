using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UniqueBuffsSO", menuName = "Buffs/UniqueBuffsSO")]
public class UniqueBuffsSO : ScriptableObject
{
    [Header("Energy Siphon")]
    public float EnergySiphon_ShieldRestoredPercent;

    [Header("Adaptive Plating")]
    public float AdaptivePlating_TimeToTrigger;
    public float AdaptivePlating_DamageReduction;
    public float AdaptivePlating_Duration;

    [Header("Nano Regen Pulse")]
    public float NanoRegenPulse_PercentPerStack;
    public int NanoRegenPulse_maxNumStacks;
    public float NanoRegenPulse_stackDuration;

    [Header("Critical Strike Overload")]
    public float CriticalStrikeOverload_CritDmgPercentPerStack;
    public float CriticalStrikeOverload_maxNumStacks;
    public float CriticalStrikeOverload_stackDuration;

    [Header("Focused Payload")]
    public float FocusedPayload_damageIncreasePercent;
    public float FocusedPayload_AoEDecreasePercent;

    [Header("Overdrive Mode")]
    public float OverdriveMode_minigunFireRateIncreasePercent;
    public float OverdriveMode_damageReductionPercent;

    [Header("Magnetized Payloads")]
    public float MagnetizedPayloads_PullStrength;
    public float MagnetizedPayloads_PullRadius;

    [Header("Shield Overcharge")]
    public float ShieldOvercharge_minigunFireRateIncreasePercent;

}
