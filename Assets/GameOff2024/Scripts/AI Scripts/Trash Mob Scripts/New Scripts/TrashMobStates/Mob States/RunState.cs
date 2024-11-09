using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunState : IState
{
    private TrashMob TrashMob;
    private NavMeshAgent _navmeshagent;

    public float runspeed = 6f;
    public float roamradius = 5f;

    public RunState(TrashMob trashMob, NavMeshAgent agent)
    {
        TrashMob = trashMob;
        _navmeshagent = agent;
    }

    public void OnEnter()
    {
        Debug.Log("Entered run");
        _navmeshagent.enabled = true;
        _navmeshagent.speed = runspeed;
    }

    public void Tick()
    {
        if (_navmeshagent.remainingDistance < 1f && !_navmeshagent.pathPending)
        {
            Debug.Log("Running");
            Vector3 roam = GetRandomLoc();
            _navmeshagent.SetDestination(roam);
        }
    }

    private Vector3 GetRandomLoc()
    {
        // Generate a random position within the roam radius
        Vector3 randdest = Random.insideUnitSphere * roamradius;
        randdest += TrashMob.transform.position;

        // Ensure the point is on the NavMesh
        NavMeshHit hit;
        NavMesh.SamplePosition(randdest, out hit, roamradius, 1);
        return hit.position;
    }

    public void OnExit() { Debug.Log("Exited run"); }
}
