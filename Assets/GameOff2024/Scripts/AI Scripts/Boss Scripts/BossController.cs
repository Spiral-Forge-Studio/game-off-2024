using KinematicCharacterController;
using System;
using System.Collections;
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

    [SerializeField] private PlayerKCC _player;

    [Header ("Boss Agent Movement Parameters")]
    [SerializeField] private BossAgentParameters _bossparam;

    [Header("Attack Pattern Flags")]
    [SerializeField] private bool _doMiniperi;
    [SerializeField] private bool _doRocketperi;

    [SerializeField] public List<GameObject> _waypoints = new List<GameObject>();
    private void Awake()
    {
        _player = GameObject.Find("Player Controller").GetComponent<PlayerKCC>();
        _agent = GetComponent<NavMeshAgent>();
        _playerDetector = GetComponent<PlayerDetector>();
        _bossparam = GetComponent<BossAgentParameters>();

        _statemachine = new StateMachine();

        //Agent Movement
        _agent.enabled = false;
        _agent.stoppingDistance = 0f;

        //Boss Phase
        var idle = new BossIdle(this, _agent, _bossparam);
        var rambo = new BossRambo();
        var spine = new BossSpine();
        var minisweep = new MiniGunSweep();
        var rocketsweep = new RocketSweep();
        var backshot = new RocketBackShot();
        var miniperi = new MiniGunPerimeterSpray(this, _agent, _bossparam);
        var rocketperi = new RocketPerimeterSpray(this, _agent, _bossparam);

        At(idle, miniperi, () => _doMiniperi && idle.IsIdle ); //Third Entry Condition to go to next state. If beginning, from idle go to minisweep as first attack move
        At(idle, rocketperi, () => _doRocketperi && idle.IsIdle); //Only go to this transition if miniperi has concluded before
        At(miniperi, idle, () => miniperi.IsComplete);  //Third Entry Condition to go to return to idle. After an attack pattern, go to idle
        At(rocketperi, idle, () => rocketperi.IsComplete);//Third Entry Condition to go to return to idle. After an attack pattern, go to idle
        _statemachine.SetState(idle);

        void At(IState from, IState to, Func<bool> condition) => _statemachine.AddTransition(from, to, condition);



    }

    private void Update()
    {
        DoMiniSweep();
        _statemachine.Tick();
    }

    public void MoveToCenter()
    {
        _agent.SetDestination(_waypoints[0].transform.position);
    }


    public void DoMiniSweep()
    {
        _doMiniperi = false;
        _doRocketperi = true;
    }
    /*
    public void Attack_MiniPerimeter()
    {
        StartCoroutine(MiniGunPerimeterSpray());
    }

    public void Attack_RocketPerimeter()
    {
        StartCoroutine(RocketPerimeterSpray());
    }
    private IEnumerator MiniGunPerimeterSpray()
    {
        int[] waypoint = {3, 6, 5, 2 };

        foreach (int index in waypoint)
        {
            _agent.SetDestination(_waypoints[index].transform.position);

            while( _agent.remainingDistance > _agent.stoppingDistance)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator RocketPerimeterSpray()
    {
        int[] waypoint = {2,5,6,3 };

        foreach (int index in waypoint)
        {
            _agent.SetDestination(_waypoints[index].transform.position);

            while (_agent.remainingDistance > _agent.stoppingDistance)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
    */

}
