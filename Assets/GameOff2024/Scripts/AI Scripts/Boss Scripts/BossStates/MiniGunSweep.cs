using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiniGunSweep : IState
{
    private BossController _boss;
    private BossAgentParameters _parameters;
    private NavMeshAgent _agent;

    private bool _isComplete;

    public bool IsComplete => _isComplete;
    public MiniGunSweep(BossController boss, NavMeshAgent agent, BossAgentParameters bossparam)
    {
        _boss = boss;
        _parameters = bossparam;
        _agent = agent;
    }
    public void Tick() { }
    public void OnEnter()
    {
        Debug.Log("Entered MiniSweep");
        _isComplete = false;
        _boss._isLocked = true;
        _agent.speed = _parameters._WhilePattern;

    }

    public void OnExit() { _boss._isLocked = false; _agent.speed = _parameters._Recenter; _boss.ResetAttackFlags(); }


}
