using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiniGunSweep : IState
{
    private BossController _boss;
    private BossAgentParameters _parameters;
    private NavMeshAgent _agent;
    private Animator _animator;
    private GameObject _torso;
    private Quaternion _originalrotation;
    Vector3 tobeshot;
    Vector3 start;
    Vector3 end;
    int startindex;
    int endindex;
    bool shootingaboutxaxis;
    private bool _isComplete;

    public bool IsComplete => _isComplete;
    public MiniGunSweep(BossController boss, NavMeshAgent agent, BossAgentParameters bossparam, Animator animator, GameObject torso)
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
        _animator.CrossFade("Armature|SB_Boss_Lower_Idle", 0.2f);
        Debug.Log("Entered MiniSweep");
        _isComplete = false;
        _boss._isLocked = true;
        _agent.speed = _parameters._WhilePattern;

        _torso.transform.localRotation = _originalrotation;
        _boss.StartCoroutine(ExecuteMiniSweep());
    }

    public void OnExit() { _boss._isLocked = false; _agent.speed = _parameters._Recenter; _boss.ResetAttackFlags(); _torso.transform.localRotation = _originalrotation; _animator.CrossFade("Armature|SB_Boss_Lower_Idle", 0.2f); _boss.MoveToCenter(); }

    private IEnumerator ExecuteMiniSweep()
    {
        // Waypoints represent the corners in order: top-left, top-right, bottom-right, bottom-left, top-left
        int[] waypoints = { 2, 1, 3, 4 , 2};
        for (int i = 0; i < waypoints.Length-1; i++)
        {
            startindex = waypoints[i];
            endindex = waypoints[i+1];
            start = _boss._shootpoints[startindex].transform.position;
            end = _boss._shootpoints[endindex].transform.position;
            
            tobeshot = start;
            // if two points are on the same x axis
            if (start.x == end.x)
            {
                shootingaboutxaxis = false;
            }
            // if two points are on the same z axis
            if (start.z == end.z)
            {
                shootingaboutxaxis = true;
            }

            if (shootingaboutxaxis == true)
            {
                if (start.x > end.x)
                {
                    
                    while (tobeshot.x > end.x)
                    {
                        tobeshot.x--;
                        _boss.ShootMinigunAt(tobeshot);
                        yield return new WaitForSeconds(1f / _boss.BossStatusSO.MinigunFireRate);
                    }
                }
                else
                {
                    while (tobeshot.x < end.x)
                    {
                        tobeshot.x++;
                        _boss.ShootMinigunAt(tobeshot);
                        yield return new WaitForSeconds(1f / _boss.BossStatusSO.MinigunFireRate);
                    }
                }
            }
            else
            {
                if (start.z > end.z)
                {
                    while (tobeshot.z > end.z)
                    {
                        tobeshot.z--;
                        RotateTorsoTowards(tobeshot);
                        _boss.ShootMinigunAt(tobeshot);

                        yield return new WaitForSeconds(1f / _boss.BossStatusSO.MinigunFireRate);
                    }
                }
                else
                {
                    while (tobeshot.z < end.z)
                    {
                        tobeshot.z++;
                        _boss.ShootMinigunAt(tobeshot);
                        yield return new WaitForSeconds(1f / _boss.BossStatusSO.MinigunFireRate);
                    }
                }
            }
            
        }
        yield return new WaitForSeconds(0.5f); // Simulate attack delay
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
