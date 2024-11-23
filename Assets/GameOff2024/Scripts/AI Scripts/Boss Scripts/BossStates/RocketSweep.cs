using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RocketSweep :IState
{
    private BossController _boss;
    private BossAgentParameters _parameters;
    private NavMeshAgent _agent;

    private bool _isComplete;

    public bool IsComplete => _isComplete;
    public RocketSweep(BossController boss, NavMeshAgent agent, BossAgentParameters bossparam) 
    {
        _boss = boss;
        _parameters = bossparam;
        _agent = agent;
    }
    public void Tick() { }
    public void OnEnter() { }

    public void OnExit() { _boss._isLocked = false; _agent.speed = _parameters._Recenter; _boss.ResetAttackFlags(); }
}
