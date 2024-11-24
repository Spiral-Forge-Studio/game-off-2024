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
    private bool _isComplete;

    public bool IsComplete => _isComplete;
    public MiniGunPerimeterSpray(BossController boss, NavMeshAgent agent, BossAgentParameters bossparam)
    {
        _boss = boss;
        _parameters = bossparam;
        _agent = agent;
    }
    public void Tick() { }
    public void OnEnter()
    {
        Debug.Log("Entered MPeri");
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
        int[] waypoints = { 3, 6, 5, 2 };
        foreach (int index in waypoints)
        {
            _agent.SetDestination(_boss._waypoints[index].transform.position);

            while (_agent.remainingDistance > _agent.stoppingDistance)
            {
                _boss.ShootMinigunAt(BossPlatform.position);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f); // Simulate attack delay
        }

        _isComplete = true; // Mark state as complete
    }
}
