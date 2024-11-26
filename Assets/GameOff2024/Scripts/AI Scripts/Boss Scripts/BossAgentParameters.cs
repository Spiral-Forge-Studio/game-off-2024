using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossAgentParameters : MonoBehaviour
{
    [Header("Agent Movement Speed")]

    [SerializeField] public float _Recenter = 5f;
    [SerializeField] public float _WhilePattern = 3f;
    [SerializeField] public float _BackShot = 7f;

    [Header ("Stopping Distance")]

    [SerializeField] public float _BackShot_StoppingDistance = 1.0f;


}
