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

    [SerializeField] List<GameObject> _waypoints = new List<GameObject>();
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _playerDetector = GetComponent<PlayerDetector>();

        _statemachine = new StateMachine();

        //Boss Phase
        var idle = new BossIdle(this);
        var rambo = new BossRambo();
        var spine = new BossSpine();
        var minisweep = new MiniGunSweep();
        var rocketsweep = new RocketSweep();
        var backshot = new RocketBackShot();
        var miniperi = new MiniGunPerimeterSpray();
        var rocketperi = new RocketPerimeterSpray();


        _statemachine.SetState(idle);
    }

    private void Update()
    {
        _statemachine.Tick();
    }

    public void MoveToCenter()
    {
        _agent.SetDestination(_waypoints[0].transform.position);
    }

    public void AttackPattern_PerimeterSpray()
    {

    }
}
