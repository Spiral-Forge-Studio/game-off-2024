using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.AI;

public class ShootState : IState
{
    private PlayerKCC _player;
    private Transform _firepoint;
    private NPCProjectileShooter _projectileShooter;
    private Animator _animator;
    private readonly TrashMob mob;
    private UnityEngine.AI.NavMeshAgent _agent;


    [HideInInspector] public Vector3 _playerpos;
    private float rotationSpeed = 5f;

    //Shooting Handlers
    private bool _ismovinglaterally = false;

    //Raycasting
    private LayerMask _obstacles;
    private float fallbacktimer = 0.5f;
    private float fallbackcooldown = 0f;
    public ShootState(TrashMob trashmob, UnityEngine.AI.NavMeshAgent agent, NPCProjectileShooter _shooter, Animator animator)
    {
        mob = trashmob;
        _agent = agent;
        _projectileShooter = _shooter;
        _animator = animator;
        //Get WeaponParams

        //Assign Layers
        _obstacles = LayerMask.GetMask("Ground");

    }


    public void Tick() 
    {
        //_animator.SetFloat("AI Speed", _agent.velocity.magnitude);
        _playerpos = _player.transform.position; 
        

        if (LineOfSight(_player.transform.position))
        {

            /*if (_agent.remainingDistance <= _agent.stoppingDistance && _agent.velocity.magnitude <= 1f)
            {
                _projectileShooter.TryShoot(_playerpos);
            }
            */

            if(_agent.velocity.magnitude <= 0.1f && !_projectileShooter.isreloading)
            {
                LookAtPlayer(_playerpos);
                _projectileShooter.TryShoot(_playerpos);
            }

            if(_projectileShooter.isreloading)
            {
                //Debug.Log("Evading");
                MoveLaterally();
                _ismovinglaterally = false;
            }
      
        }

        else
        {
            if (fallbackcooldown > 0f)
            {
                fallbackcooldown -= Time.deltaTime;
            }

            else
            {
                Vector3 newpos = FindNewPosition();
                if (newpos != Vector3.zero)
                {
                    _agent.SetDestination(newpos);
                    fallbackcooldown = fallbacktimer;
                }

                else
                {
                    Debug.LogWarning("No valid position");
                }
            }
            
        }



        if ((_playerpos - _agent.transform.position).magnitude > 15f)
        {
            _agent.SetDestination(_playerpos);
        }
        

    }
    public void OnEnter() 
    {
        _ismovinglaterally = false;
        //NavMeshdata
        _agent.enabled = true;
        //_agent.SetDestination(_playerpos);

        //Get Player data
        GameObject player = GameObject.Find("Player Controller");

        if (player != null) { _player = player.GetComponent<PlayerKCC>(); }

        else if (player == null) { Debug.LogWarning("Player Object Not Found"); }

    }
    public void OnExit() 
    {
    }

    private void LookAtPlayer(Vector3 _position)
    {
        //Orient NPC towards Player Location

        // Calculate the direction to the player
        Vector3 directionToPlayer = _position - mob.transform.position;
        directionToPlayer.y = 0; // Keep the rotation only on the Y-axis to avoid tilting

        // If the direction is not zero, rotate towards the player
        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // Smoothly rotate the NPC towards the player
            mob.transform.rotation = Quaternion.Slerp(mob.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

    }

    private bool LineOfSight (Vector3 _position)
    {
        Vector3 direction = _position - mob.transform.position;
        float distance = direction.magnitude;

        if (Physics.Raycast(mob.transform.position, direction, out RaycastHit hit, distance,_obstacles))
        {
            return hit.collider.gameObject == _player.gameObject;
        }

        return true;
    }

    private Vector3 FindNewPosition()
    {
        Debug.Log("Computing New Position");

        float searchRadius = 10f;
        float maxAttempts = 10f;

        for(int i = 0; i < maxAttempts; i++)
        {
            //Generate 
            Vector3 randomDirection = Random.insideUnitSphere * searchRadius;
            randomDirection.y = 0;
            Vector3 candidatePosition = mob.transform.position + randomDirection;

            //Check if the candidate position has line of sight
            if (NavMesh.SamplePosition(candidatePosition, out NavMeshHit hit, searchRadius, _agent.areaMask) && LineOfSight(hit.position))
            {
                return hit.position;
            }
        }

        return Vector3.zero;
    }

   private void MoveLaterally()
    {
        Debug.Log("Moving Laterally");
        _ismovinglaterally = true;
        // Check left and right directions
        Vector3 left = mob.transform.position - mob.transform.right * 1f;
        Vector3 right = mob.transform.position + mob.transform.right * 1f;
        if (LineOfSight(left))
        {
            _agent.SetDestination(left);
        }
        else if (LineOfSight(right))
        {
            _agent.SetDestination(right);
        }
    }
}
