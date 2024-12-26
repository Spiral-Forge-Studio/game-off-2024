using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : IState
{
    private readonly TrashMob TrashMob;
    private NavMeshAgent _navmeshagent;
    //private readonly TrashMobParameters _parameters;
    private Animator _animator;

    [HideInInspector] public float roamspeed;
    public float roamradius = 7f;

    private List<Vector3> roamPoints = new List<Vector3>();
    private int currentPointIndex = 0;
    public IdleState(TrashMob trashMob, NavMeshAgent agent, TrashMobParameters parameters, Animator animator)
    {
        TrashMob = trashMob;
        _navmeshagent = agent;
        //_parameters = parameters;
        roamspeed = parameters.roamspeed;
        _animator = animator;
    }

    public void OnEnter() 
    { 
        _navmeshagent.enabled = true; 
        _navmeshagent.speed = roamspeed; 
        _navmeshagent.autoBraking = true;
        _navmeshagent.stoppingDistance = 0f;
        _animator?.Play("Movement");
    }
    public void Tick()
    {
        if (_navmeshagent.remainingDistance < 1 && !_navmeshagent.pathPending)
        {
            if (currentPointIndex >= roamPoints.Count)
            {
                GenerateRoamPoints();
                currentPointIndex = 0;
            }

            
            _navmeshagent.SetDestination(roamPoints[currentPointIndex]);
            currentPointIndex++;
        }
    }

    #region Movement Function
    /// <summary>
    /// Generate 5 around set radius
    /// </summary>
    private void GenerateRoamPoints()
    {
        roamPoints.Clear(); // Clear any previous points
        Vector3 center = TrashMob.transform.position;

        for (int i = 0; i < 5; i++)
        {
            // Calculate angle for evenly spaced points around a circle
            //float angle = i * (2 * Mathf.PI / 5);
            //float xOffset = Mathf.Cos(angle) * roamradius;
            //float zOffset = Mathf.Sin(angle) * roamradius;
            float xOffset = Random.Range(_navmeshagent.transform.position.x, roamradius);
            float zOffset = Random.Range(_navmeshagent.transform.position.z, roamradius);

            Vector3 roamPoint = new Vector3(center.x + xOffset, center.y, center.z + zOffset);

            // Check if the point is on the NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(roamPoint, out hit, roamradius, NavMesh.AllAreas))
            {
                roamPoints.Add(hit.position);
            }
            else
            {
                // If not, add the center position as a fallback (or regenerate)
                roamPoints.Add(center);
            }
        }
    }
    #endregion

    public void OnExit() { }
}
