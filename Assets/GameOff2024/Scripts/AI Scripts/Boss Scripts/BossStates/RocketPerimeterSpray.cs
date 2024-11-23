using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class RocketPerimeterSpray : IState
{

    private BossController _boss;
    private BossAgentParameters _parameters;
    private NavMeshAgent _agent;

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
        _boss.StartCoroutine(ExecuteRocketSweep());
    }

    public void OnExit() { _agent.speed = _parameters._Recenter; _boss._isLocked = false; _boss.ResetAttackFlags(); }

    private IEnumerator ExecuteRocketSweep()
    {
        int[] waypoints = { 2,5,6,3 };
        foreach (int index in waypoints)
        {
            _agent.SetDestination(_boss._waypoints[index].transform.position);

            while (_agent.remainingDistance > _agent.stoppingDistance)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.5f); // Simulate attack delay
        }

        _isComplete = true; // Mark state as complete
    }
}
