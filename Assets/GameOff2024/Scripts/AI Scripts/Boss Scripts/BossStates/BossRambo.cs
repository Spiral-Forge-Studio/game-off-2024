using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class BossRambo : IState
{
    private BossController _boss;
    private BossAgentParameters _parameters;
    private Quaternion _originalrotation;
    private NavMeshAgent _agent;
    private Animator _animator;
    private GameObject _torso;

    private bool _isComplete;

    public bool IsComplete => _isComplete;
    public BossRambo(BossController boss, NavMeshAgent agent, BossAgentParameters bossparam, Animator animator, GameObject torso)
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
        _animator.CrossFade("Armature|SB_Boss_LowerIdle", 0.2f);
        Debug.Log("Entered Rambo");
        _boss._isLocked = true;
        _isComplete = false;
        _agent.speed = _parameters._WhilePattern;
        _originalrotation = _torso.transform.localRotation;
        _boss.StartCoroutine(ExecuteRambo());
    }

    public void OnExit() { _boss._isLocked = false; _agent.speed = _parameters._Recenter; _boss.ResetAttackFlags(); _torso.transform.localRotation = _originalrotation; }

    private IEnumerator ExecuteRambo()
    {
        float Duration = 10f;//Duration of rambo
        float Start = Time.time;
        GameObject player = GameObject.Find("Player Controller");
        while (true)
        {
            if (Time.time - Start > Duration) { break; }
            RotateTorsoTowards(player.transform.position);
            _boss.ShootMinigunAt(player.transform.position);
            _boss.ShootRocketAt(player.transform.position);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f); // Simulate attack delay
        _isComplete = true;
    }

    private void RotateTorsoTowards(Vector3 targetPosition)
    {
        Vector3 directionToTarget = (targetPosition - _torso.transform.position).normalized;
        directionToTarget.y = 0; // Ignore Y-axis to only rotate in the XZ plane

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        _torso.transform.rotation = Quaternion.Slerp(_torso.transform.rotation, targetRotation, Time.deltaTime * 4f);
    }
}
