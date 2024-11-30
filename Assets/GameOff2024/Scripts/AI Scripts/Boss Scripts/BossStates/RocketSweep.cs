using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RocketSweep :IState
{
    private BossController _boss;
    private BossAgentParameters _parameters;
    private NavMeshAgent _agent;

    private Animator _animator;
    private GameObject _torso;
    Vector3 tobeshot;
    Vector3 start;
    Vector3 end;
    int startindex;
    int endindex;
    bool shootingaboutxaxis;
    private bool _isComplete;

    public bool IsComplete => _isComplete;

    public RocketSweep(BossController boss, NavMeshAgent agent, BossAgentParameters bossparam, Animator animator, GameObject torso) 
    {
        _boss = boss;
        _parameters = bossparam;
        _agent = agent;
        _animator = animator;
        _torso = torso;
    }
    public void Tick() { }
    public void OnEnter() {

        Debug.Log("Entered RocketSweep");
        _isComplete = false;
        _boss._isLocked = true;
        _agent.speed = _parameters._WhilePattern;
        _boss.StartCoroutine(ExecuteRocketSweep());
    }

    public void OnExit() { _boss._isLocked = false; _agent.speed = _parameters._Recenter; _boss.ResetAttackFlags(); }

    private IEnumerator ExecuteRocketSweep()
    {
        int skipcounter = 5;
        // Waypoints represent the corners in order: top-left, top-right, bottom-right, bottom-left, top-left
        int[] waypoints = { 1, 2, 4, 3, 1 };
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            startindex = waypoints[i];
            endindex = waypoints[i + 1];
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
                        tobeshot.x-=skipcounter;
                        _boss.ShootRocketAt(tobeshot);
                        yield return new WaitForSeconds(1f / _boss.BossStatusSO.RocketFireRate);
                    }
                }
                else
                {
                    while (tobeshot.x < end.x)
                    {
                        tobeshot.x+=skipcounter;
                        _boss.ShootRocketAt(tobeshot);
                        yield return new WaitForSeconds(1f / _boss.BossStatusSO.RocketFireRate);
                    }
                }
            }
            else
            {
                if (start.z > end.z)
                {
                    while (tobeshot.z > end.z)
                    {
                        tobeshot.z-=skipcounter;
                        _boss.ShootRocketAt(tobeshot);

                        yield return new WaitForSeconds(1f / _boss.BossStatusSO.RocketFireRate);
                    }
                }
                else
                {
                    while (tobeshot.z < end.z)
                    {
                        tobeshot.z+=skipcounter;
                        _boss.ShootRocketAt(tobeshot);
                        yield return new WaitForSeconds(1f / _boss.BossStatusSO.RocketFireRate);
                    }
                }
            }

        }
        yield return new WaitForSeconds(0.5f); // Simulate attack delay
        _isComplete = true; // Mark state as complete
    }

}
