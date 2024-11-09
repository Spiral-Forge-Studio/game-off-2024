using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class TrashMob : MonoBehaviour
{
    //Import NPC Parameters
    private TrashMobParameters _parameters;
    private NavMeshAgent _agent;

    private StateMachine _statemachine;

    public bool runTriggered = false;
    public float timer;

    private void Awake()
    {
        _parameters = GetComponent<TrashMobParameters>();
        _agent = GetComponent<NavMeshAgent>();

        _statemachine = new StateMachine();

        var idle = new IdleState(this, _agent);
        var run = new RunState(this, _agent);


        At(idle, run, ShouldRun());

        _statemachine.SetState(idle);

        void At(IState from, IState to, Func<bool> condition) => _statemachine.AddTransition(from, to, condition);
        Func<bool> ShouldRun() => () => runTriggered;
    }

    private void Update()
    {

        _statemachine.Tick();
    }


    private void Changebool()
    {
        timer += Time.deltaTime;
        if(timer > 3f)
        {
            runTriggered = true;
        }
    }
}
