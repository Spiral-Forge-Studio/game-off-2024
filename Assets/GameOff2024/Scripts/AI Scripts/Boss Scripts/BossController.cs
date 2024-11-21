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

    private Transform playerTransform;

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
        ShootRocketAt(aimPosition);


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
        float timer = Time.time;
        // Simulate pressing the button
        weaponManager.SetInputs(ref aiInputsForShooting);
        while (Time.time - timer > 0.2f)
        {
            //do nothing
        }
        aiInputsForShooting = new BossCombatInputs
        {
            RightShoot = false,  // Start the rocket shooting
            mousePos = Tobeshot // Aim at the target position
        };
        // Wait for a time shorter than the rocket_setHoldTime (e.g., 0.2f)
        weaponManager.SetInputs(ref aiInputsForShooting);
    }


    // Coroutine to simulate a quick rocket fire (shorter than the hold time)
    IEnumerator SimulateQuickRocketFire(Vector3 targetPosition)
    {
        // Create inputs for shooting
        BossCombatInputs aiInputsForShooting = new BossCombatInputs
        {
            RightShoot = true,  // Start the rocket shooting
            mousePos = targetPosition // Aim at the target position
        };

        // Simulate pressing the button
        weaponManager.SetInputs(ref aiInputsForShooting);

        // Wait for a time shorter than the rocket_setHoldTime (e.g., 0.2f)
        yield return new WaitForSeconds(0.2f);

        // Simulate releasing the button
        aiInputsForShooting.RightShoot = false;
        weaponManager.SetInputs(ref aiInputsForShooting);
    }

    void TriggerQuickRocketFire()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) < shootingRange)
        {
            Vector3 aimPosition = playerTransform.position; // Target player's position
            StartCoroutine(SimulateQuickRocketFire(aimPosition)); // Simulate quick press
        }
    }


}
