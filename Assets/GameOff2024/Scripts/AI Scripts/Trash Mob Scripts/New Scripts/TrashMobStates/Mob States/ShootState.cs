using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class ShootState : IState
{
    private PlayerKCC _player;
    private Transform _firepoint;
    private NPCProjectileShooter _projectileShooter;
    private readonly TrashMob mob;
    private UnityEngine.AI.NavMeshAgent _agent;


    [HideInInspector] public Vector3 _playerpos;
    private float rotationSpeed = 3f;
    public ShootState(TrashMob trashmob, UnityEngine.AI.NavMeshAgent agent, NPCProjectileShooter _shooter)
    {

        mob = trashmob;
        _agent = agent;
        _projectileShooter = _shooter;
        //Get WeaponParams

    }


    public void Tick() 
    { 
        _playerpos = _player.transform.position; 
        LookAtPlayer(_playerpos);

        _projectileShooter.TryShoot(_playerpos);

    }
    public void OnEnter() 
    {
        //NavMeshdata
        _agent.enabled = true;

        
        Debug.Log("Entered ShootState");

        //Get Player data
        GameObject player = GameObject.Find("Player Controller");

        if (player != null) { _player = player.GetComponent<PlayerKCC>(); }

        else if (player == null) { Debug.LogWarning("Player Object Not Found"); }

    }
    public void OnExit() { }

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
}