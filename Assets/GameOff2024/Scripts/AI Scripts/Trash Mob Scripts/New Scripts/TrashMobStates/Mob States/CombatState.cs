using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatState : TrashMobIState
{
    private readonly TrashMob mob;
    private UnityEngine.AI.NavMeshAgent _agent;

    public CombatState(TrashMob trashMob, UnityEngine.AI.NavMeshAgent agent)
    {
        mob = trashMob;
        _agent = agent;
    }
    public void OnEnter() { _agent.enabled = false; Debug.Log("Entered CombatState"); }
    public void Tick() { }
    public void OnExit() { }
}
