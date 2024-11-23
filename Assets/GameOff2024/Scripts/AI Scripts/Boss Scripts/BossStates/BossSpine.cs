using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossSpine : IState
{
    private BossController _boss;
    private BossAgentParameters _parameters;
    private NavMeshAgent _agent;

    private bool _isComplete;

    public bool IsComplete => _isComplete;
    public BossSpine(BossController boss, NavMeshAgent agent, BossAgentParameters bossparam)
    {
        _boss = boss;
        _parameters = bossparam;
        _agent = agent;
    }
    public void Tick() { }
    public void OnEnter() 
    {
        Debug.Log("Entered Spine Pattern");
        _boss._isLocked = true;
        _isComplete = false;
        _agent.speed = _parameters._WhilePattern;
        _boss.StartCoroutine(ExecuteSpine());
    }

    public void OnExit() { _boss._isLocked = false; _agent.speed = _parameters._Recenter; _boss.ResetAttackFlags(); }

    private IEnumerator ExecuteSpine()
    {
        int[] waypoints = { 1,4,0,7,8 };
        foreach (int index in waypoints)
        {
            _agent.SetDestination(_boss._waypoints[index].transform.position);

            while (_agent.remainingDistance > _agent.stoppingDistance)
            {
                if (index == 1 || index == 4)
                {
                    _boss.ShootMinigunAt(new Vector3(1000, _boss.transform.position.y, _boss.transform.position.z));
                    _boss.ShootRocketAt(new Vector3(-1000, _boss.transform.position.y, _boss.transform.position.z));
                }
                if (index == 7 || index == 8)
                {
                    _boss.ShootMinigunAt(new Vector3(_boss.transform.position.x, _boss.transform.position.y, 1000));
                    _boss.ShootRocketAt(new Vector3(_boss.transform.position.x, _boss.transform.position.y, -1000));
                }
                yield return null;
            }

            yield return new WaitForSeconds(0.5f); // Simulate attack delay
        }

        _isComplete = true; // Mark state as complete
    }
}
