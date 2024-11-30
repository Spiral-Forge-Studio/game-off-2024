using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class RocketPerimeterSpray : IState
{

    private BossController _boss;
    private BossAgentParameters _parameters;
    private NavMeshAgent _agent;
    private Transform BossPlatform;
    private Animator _animator;
    private GameObject _torso;
    private Quaternion _originalrotation;
    private bool _isComplete;

    public bool IsComplete => _isComplete;
    public RocketPerimeterSpray(BossController boss, NavMeshAgent agent, BossAgentParameters bossparam, Animator animator, GameObject torso) 
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
        Debug.Log("Entered RPeri");
        _isComplete = false;
        _boss._isLocked = true;
        _agent.speed = _parameters._WhilePattern;
        _torso.transform.localRotation = _originalrotation;
        GameObject bossPlatformObject = GameObject.FindWithTag("BossPlatform");
        if (bossPlatformObject != null)
        {
            BossPlatform = bossPlatformObject.transform;
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'BossPlatform' was found!");
        }
        _boss.StartCoroutine(ExecuteRocketSweep());
    }

    public void OnExit()
    {
        _agent.speed = _parameters._Recenter;
        _boss._isLocked = false;
        _boss.ResetAttackFlags();
        _animator.CrossFade("Armature|SB_Boss_Lower_Walking", 0.2f);
        _boss.MoveToCenter();
        _torso.transform.localRotation = _originalrotation;
    }
    private IEnumerator ExecuteRocketSweep()
    {
        int[] waypoints = {3,11,12,6,5,9,8,2}; 
        foreach (int index in waypoints)
        {
            _agent.SetDestination(_boss._waypoints[index].transform.position);
            Debug.Log($"Going To Waypoint" + index);

            while (_agent.pathPending) { yield return null; }
            while (!_agent.pathPending &&  _agent.remainingDistance > _agent.stoppingDistance)
            {
                RotateTorsoTowards(BossPlatform.position);
                _boss.ShootRocketAt(BossPlatform.position);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f); // Simulate attack delay
        }

        _isComplete = true; // Mark state as complete
    }

    private void RotateTorsoTowards(Vector3 targetPosition)
    {
        Vector3 directionToTarget = (targetPosition - _torso.transform.position).normalized;
        directionToTarget.y = 0; // Ignore Y-axis to only rotate in the XZ plane

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        _torso.transform.rotation = Quaternion.Slerp(_torso.transform.rotation, targetRotation, Time.deltaTime * 2f);
    }
}
