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

    private bool _isComplete;
    public bool IsIdle => _isComplete;


    private float time;
    public BossIdle(BossController boss, NavMeshAgent agent, BossAgentParameters bossparam, Animator animator)
    { 
        _bossfunc = boss; 
        _parameters = bossparam;
        _agent = agent;
        _animator = animator;
    }
    public void Tick() 
    {
        // Continuously check if the agent has reached the destination
        if (!_isComplete && !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {

            //_isComplete = true; // Set IsIdle to true when destination is reached
            //Debug.Log("Boss has finished moving to the center.");
            _animator.CrossFade("Armature|SB_Boss_Lower_Idle", 0.2f);
            
        }

        
        if(Time.time > time + 3f)
        {
            _isComplete = true;
            //Debug.Log("Ready for New State");
        }
    }
    public void OnEnter() 
    {
        _animator.CrossFade("Armature|SB_Boss_Lower_Walking", 0.2f);
        time = Time.time;
        _isComplete = true; // Set IsIdle to true when destination is reached
        Debug.Log("Boss has finished moving to the center.");
        _isComplete = false;
        _agent.enabled = true;
        _agent.speed = _parameters._Recenter;
        Debug.Log("Entered Idle");
        
        _bossfunc.MoveToCenter();
        
        //_bossfunc.Attack_RocketPerimeter();
    }

    public void OnExit() { }


}



