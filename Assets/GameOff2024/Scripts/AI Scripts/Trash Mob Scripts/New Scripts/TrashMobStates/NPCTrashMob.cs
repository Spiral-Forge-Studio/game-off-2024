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

    private void Awake()
    {
        _parameters = GetComponent<TrashMobParameters>();
        _agent = GetComponent<NavMeshAgent>();
        var playerdetector = GetComponentInChildren<PlayerDetector>();

        _statemachine = new StateMachine();

        var idle = new IdleState(this, _agent, _parameters);
        var combat = new CombatState(this, _agent, _parameters);

        _statemachine.AddAnyTransition(combat, () => playerdetector.PlayerInRange);
        At(combat, idle, () => playerdetector.PlayerInRange == false);
        

        _statemachine.SetState(idle);

        void At(IState from, IState to, Func<bool> condition) => _statemachine.AddTransition(from, to, condition);
    }

    private void Update() { _statemachine.Tick(); }




}
