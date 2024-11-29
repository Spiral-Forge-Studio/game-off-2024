using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiniGunPerimeterSpray : IState
{
    private BossController _boss;
    private BossAgentParameters _parameters;
    private NavMeshAgent _agent;
    private Transform BossPlatform;
    private Animator _animator;
    private bool _isComplete;

    public bool IsComplete => _isComplete;
    public MiniGunPerimeterSpray(BossController boss, NavMeshAgent agent, BossAgentParameters bossparam, Animator animator)
    {
        _boss = boss;
        _parameters = bossparam;
        _agent = agent;
        _animator = animator;
    }
    public void Tick() { }
    public void OnEnter()
    {
        Debug.Log("Entered MPeri");
        _animator.CrossFade("Armature|SB_Boss_Lower_Slide", 0.2f);
        _boss._isLocked = true;
        _isComplete = false;
        _agent.speed = _parameters._WhilePattern;
        
        GameObject bossPlatformObject = GameObject.FindWithTag("BossPlatform");
        if (bossPlatformObject != null)
        {
            BossPlatform = bossPlatformObject.transform;
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'BossPlatform' was found!");
        }
        _boss.StartCoroutine(ExecuteMiniPeri());
    }

    public void OnExit() { _agent.speed = _parameters._Recenter; _boss._isLocked = false; _boss.ResetAttackFlags(); }

    private IEnumerator ExecuteMiniPeri()
    {
        int[] waypoints = {2,8,9,5,6,12,11,3};
        foreach (int index in waypoints)
        {
            _agent.SetDestination(_boss._waypoints[index].transform.position);
            Debug.Log($"Going To Waypoint " + index);

            while (_agent.pathPending) { yield return null; }

            while (_agent.remainingDistance >_agent.stoppingDistance)
            {
                _boss.ShootMinigunAt(BossPlatform.position);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f); // Simulate attack delay
        }

        _isComplete = true; // Mark state as complete
    }
}
