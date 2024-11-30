using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossIdle : IState
{
    private BossController _bossfunc;
    private BossAgentParameters _parameters;
    private NavMeshAgent _agent;
    private Animator _animator;
    private GameObject _torso;

    private bool _isComplete;
    public bool IsIdle => _isComplete;


    private float time;

    public BossIdle(BossController boss, NavMeshAgent agent, BossAgentParameters bossparam, Animator animator, GameObject torso) 
    { 
        _bossfunc = boss; 
        _parameters = bossparam;
        _agent = agent;
        _animator = animator;
        _torso = torso;
    }
    public void Tick() 
    {
        // Continuously check if the agent has reached the destination
        if (!_isComplete && !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _isComplete = true; // Set IsIdle to true when destination is reached
            _animator.CrossFade("Armature|SB_Boss_Lower_Idle", 0.2f);
            Debug.Log("Boss has finished moving to the center.");
        }

        if(!_isComplete && _agent.pathPending && _agent.remainingDistance > _agent.stoppingDistance)
        {
            _animator.CrossFade("Armature|SB_Boss_Lower_Walking", 0.2f);
        }
    }
    public void OnEnter() 
    {
        _animator.CrossFade("Armature|SB_Boss_Lower_Walking", 0.2f);
        _isComplete = false;
        _agent.enabled = true;
        _agent.speed = _parameters._Recenter;
        Debug.Log("Entered Idle");
        
        _bossfunc.MoveToCenter();
        
        //_bossfunc.Attack_RocketPerimeter();
    }

    public void OnExit() { }


}



