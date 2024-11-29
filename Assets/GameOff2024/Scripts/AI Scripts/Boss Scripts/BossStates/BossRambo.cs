using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class BossRambo : IState
{
    private BossController _boss;
    private BossAgentParameters _parameters;
    private NavMeshAgent _agent;
    private Animator _animator;

    private bool _isComplete;

    public bool IsComplete => _isComplete;
    public BossRambo(BossController boss, NavMeshAgent agent, BossAgentParameters bossparam, Animator animator)
    {
        _boss = boss;
        _parameters = bossparam;
        _agent = agent;
        _animator = animator;
    }
    public void Tick() { }
    public void OnEnter()
    {
        Debug.Log("Entered Rambo");
        _boss._isLocked = true;
        _isComplete = false;
        _agent.speed = _parameters._WhilePattern;
        _boss.StartCoroutine(ExecuteRambo());
    }

    public void OnExit() { _boss._isLocked = false; _agent.speed = _parameters._Recenter; _boss.ResetAttackFlags(); }

    private IEnumerator ExecuteRambo()
    {
        float Duration = 10f;//Duration of rambo
        float Start = Time.time;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        while (true)
        {
            if (Time.time - Start > Duration) { break; }
            _boss.ShootMinigunAt(player.transform.position);
            _boss.ShootRocketAt(player.transform.position);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f); // Simulate attack delay
        _isComplete = true;
    }
}
