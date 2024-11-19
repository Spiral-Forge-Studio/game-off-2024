using KinematicCharacterController;
using System;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class BossController : MonoBehaviour
{
    private NavMeshAgent _agent;

    private StateMachine _statemachine;

    private PlayerDetector _playerDetector;
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _playerDetector = GetComponent<PlayerDetector>();

        _statemachine = new StateMachine();

        var idle = new BossIdle();

        _statemachine.SetState(idle);
    }

    private void Update()
    {
        _statemachine.Tick();
    }
}
