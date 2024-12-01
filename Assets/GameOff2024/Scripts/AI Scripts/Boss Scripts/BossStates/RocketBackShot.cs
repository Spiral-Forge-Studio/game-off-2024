using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RocketBackShot :IState
{
    private BossController _boss;
    private BossAgentParameters _parameters;
    private NavMeshAgent _agent;
    private Animator _animator;
    private GameObject _torso;

    private bool _isComplete;

    public bool IsComplete => _isComplete;
    public RocketBackShot(BossController boss, NavMeshAgent agent, BossAgentParameters bossparam, Animator animator, GameObject torso) 
    {
        _boss = boss;
        _parameters = bossparam;
        _agent = agent;
        _animator = animator;
        _torso = torso;
    }
    public void Tick() { }
    public void OnEnter() 
    {
        _animator.CrossFade("Armature|SB_Boss_Lower_Slide", 0.2f);
        Debug.Log("Entered BackShot Pattern");
        _isComplete = false;
        _boss._isLocked = true;
        _agent.stoppingDistance = _parameters._BackShot_StoppingDistance;
    }

    public void OnExit() { _boss._isLocked = false; _agent.speed = _parameters._Recenter; _boss.ResetAttackFlags(); _animator.CrossFade("Armature|SB_Boss_Lower_Walking", 0.2f); }
}
