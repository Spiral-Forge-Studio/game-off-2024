using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatState : IState
{
    private readonly TrashMob mob;
    private UnityEngine.AI.NavMeshAgent _agent;
    //private readonly TrashMobParameters _parameters;

    [HideInInspector] public float _combatspeed;
    public CombatState(TrashMob trashMob, UnityEngine.AI.NavMeshAgent agent, TrashMobParameters parameters)
    {
        mob = trashMob;
        _agent = agent;
        //_parameters = parameters;
        _combatspeed = parameters.combatspeed;

    }
    public void OnEnter() { _agent.enabled = false;  Debug.Log("Entered CombatState"); }
    public void Tick() { }
    public void OnExit() { }
}
