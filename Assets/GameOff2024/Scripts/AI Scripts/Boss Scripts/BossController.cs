using KinematicCharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class BossController : MonoBehaviour
{
    private NavMeshAgent _agent;

    private StateMachine _statemachine;

    private PlayerDetector _playerDetector;

    public BossWeaponManager weaponManager;
    public BossStatusSO BossStatusSO;
    private Transform playerTransform;

    private float timer;

    [SerializeField] private float shootingRange;
    [SerializeField] private float aimOffset;


    [SerializeField] List<GameObject> _waypoints = new List<GameObject>();
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _playerDetector = GetComponent<PlayerDetector>();
        _statemachine = new StateMachine();

        //Boss Phase
        var idle = new BossIdle(this);
        var rambo = new BossRambo();
        var spine = new BossSpine();
        var minisweep = new MiniGunSweep();
        var rocketsweep = new RocketSweep();
        var backshot = new RocketBackShot();
        var miniperi = new MiniGunPerimeterSpray();
        var rocketperi = new RocketPerimeterSpray();

        timer = 0;
        _statemachine.SetState(idle);
    }

    private void Update()
    {
        _statemachine.Tick();


        //Weapon pew pew you gotta put in states
        // Get player transform if not already assigned
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogWarning("Player not found!");
                return;
            }
        }
        Vector3 aimPosition = playerTransform.position;
        ShootMinigunAt(aimPosition);
        //ShootRocketAt(aimPosition);
        BackShotAt(aimPosition, BossStatusSO.RocketMagazineSize);




    }

    public void MoveToCenter()
    {
        _agent.SetDestination(_waypoints[0].transform.position);
    }

    public void AttackPattern_PerimeterSpray()
    {

    }
    public void ShootMinigunAt(Vector3 Tobeshot)
    {
        BossCombatInputs aiInputsForShooting = new BossCombatInputs
        {
            LeftShoot = true, // Simulate minigun shooting
            mousePos = Tobeshot // Aim at the player
        };
        weaponManager.SetInputs(ref aiInputsForShooting);
    }
    public void ShootRocketAt(Vector3 Tobeshot)
    {
        BossCombatInputs aiInputsForShooting = new BossCombatInputs
        {
            RightShoot = true,  // Start the rocket shooting
            mousePos = Tobeshot // Aim at the target position
        };
        // Simulate pressing the button
        weaponManager.SetInputs(ref aiInputsForShooting);
        aiInputsForShooting = new BossCombatInputs
        {
            RightShoot = false,  // Start the rocket shooting
            mousePos = Tobeshot // Aim at the target position
        };
        weaponManager.SetInputs(ref aiInputsForShooting);
    }

    public void BackShotAt(Vector3 Tobeshot, int Amount)
    {
        if (Time.time - timer > 1/BossStatusSO.RocketReleaseFireRate)
        {
            timer = Time.time;
            weaponManager.FireRocketProjectiles(Tobeshot, Amount);
        }
    }


}
