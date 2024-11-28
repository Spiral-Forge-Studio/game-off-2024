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
    private bool _isComplete;

    public bool IsComplete => _isComplete;
    public RocketPerimeterSpray(BossController boss, NavMeshAgent agent, BossAgentParameters bossparam) 
    {
        _boss = boss;
        _parameters = bossparam;
        _agent = agent;
    }
    public void Tick() { }
    public void OnEnter() 
    {
        Debug.Log("Entered RPeri");
        _isComplete = false;
        _boss._isLocked = true;
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
        _boss.StartCoroutine(ExecuteRocketSweep());
    }

    public void OnExit() { _agent.speed = _parameters._Recenter; _boss._isLocked = false; _boss.ResetAttackFlags(); }

    private IEnumerator ExecuteRocketSweep()
    {
        int[] waypoints = {3,11,12,6,5,9,8,2}; 
        foreach (int index in waypoints)
        {
            _agent.SetDestination(_boss._waypoints[index].transform.position);

            while (_agent.pathPending) { yield return null; }
            while (!_agent.pathPending &&  _agent.remainingDistance > _agent.stoppingDistance)
            {
                _boss.ShootRocketAt(BossPlatform.position);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f); // Simulate attack delay
        }

        _isComplete = true; // Mark state as complete
    }
}
