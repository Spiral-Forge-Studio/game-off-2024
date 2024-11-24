using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiniGunSweep : IState
{
    private BossController _boss;
    private BossAgentParameters _parameters;
    private NavMeshAgent _agent;

    private bool _isComplete;

    public bool IsComplete => _isComplete;
    public MiniGunSweep(BossController boss, NavMeshAgent agent, BossAgentParameters bossparam)
    {
        _boss = boss;
        _parameters = bossparam;
        _agent = agent;
    }
    public void Tick() { }
    public void OnEnter()
    {
        Debug.Log("Entered MiniSweep");
        _isComplete = false;
        _boss._isLocked = true;
        _agent.speed = _parameters._WhilePattern;
        _boss.StartCoroutine(ExecuteMiniSweep());
    }

    public void OnExit() { _boss._isLocked = false; _agent.speed = _parameters._Recenter; _boss.ResetAttackFlags(); }

    private IEnumerator ExecuteMiniSweep()
    {
        // Waypoints represent the corners in order: top-left, top-right, bottom-right, bottom-left
        int[] waypoints = { 2, 3, 6, 5 };
        List<Vector3> shootingPoints = new List<Vector3>();

        // Number of divisions for each edge
        //int pointsPerEdge = Mathf.RoundToInt(_boss.BossStatusSO.MinigunFireRate); // Adjust for more or fewer points along the edges
        int pointsPerEdge = Mathf.RoundToInt(10 * _boss.BossStatusSO.MinigunFireRate);

        // Loop through each edge and calculate intermediate points
        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 start = _boss._waypoints[waypoints[i]].transform.position;
            Vector3 end = _boss._waypoints[waypoints[(i + 1) % waypoints.Length]].transform.position; // Wrap around to first waypoint

            for (int j = 0; j <= pointsPerEdge; j++) // Include both start and end points
            {
                float t = (float)j / pointsPerEdge; // Interpolation factor
                Vector3 point = Vector3.Lerp(start, end, t);
                shootingPoints.Add(point);

            }
            
        }
        
        // Shoot at all calculated points
        foreach (Vector3 point in shootingPoints)
        {
            _boss.ShootMinigunAt(point);
            yield return null;

        }
        //shootingPoints.Clear();
        yield return new WaitForSeconds(0.5f); // Simulate attack delay

        _isComplete = true; // Mark state as complete
    }


}
