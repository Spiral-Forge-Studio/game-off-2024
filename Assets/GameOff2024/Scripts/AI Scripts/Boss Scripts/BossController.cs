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

    [SerializeField] private PlayerKCC _player;

    [Header ("Boss Agent Movement Parameters")]
    [SerializeField] private BossAgentParameters _bossparam;

    [Header("Attack Pattern Flags")]
    [SerializeField] private bool _doMiniperi;
    [SerializeField] private bool _doRocketperi;

    [SerializeField] public List<GameObject> _waypoints = new List<GameObject>();
    public BossWeaponManager weaponManager;
    public BossStatusSO BossStatusSO;
    private Transform playerTransform;

    private float timer;

    [SerializeField] private float shootingRange;
    [SerializeField] private float aimOffset;


    private void Awake()
    {
        _player = GameObject.Find("Player Controller").GetComponent<PlayerKCC>();
        _agent = GetComponent<NavMeshAgent>();
        _playerDetector = GetComponent<PlayerDetector>();
        _bossparam = GetComponent<BossAgentParameters>();

        _statemachine = new StateMachine();

        //Agent Movement
        _agent.enabled = false;
        _agent.stoppingDistance = 0f;

        //Boss Phase
        var idle = new BossIdle(this, _agent, _bossparam);
        var rambo = new BossRambo();
        var spine = new BossSpine();
        var minisweep = new MiniGunSweep();
        var rocketsweep = new RocketSweep();
        var backshot = new RocketBackShot();
        var miniperi = new MiniGunPerimeterSpray(this, _agent, _bossparam);
        var rocketperi = new RocketPerimeterSpray(this, _agent, _bossparam);

        At(idle, miniperi, () => _doMiniperi && idle.IsIdle ); //Third Entry Condition to go to next state. If beginning, from idle go to minisweep as first attack move
        At(idle, rocketperi, () => _doRocketperi && idle.IsIdle); //Only go to this transition if miniperi has concluded before
        At(miniperi, idle, () => miniperi.IsComplete);  //Third Entry Condition to go to return to idle. After an attack pattern, go to idle
        At(rocketperi, idle, () => rocketperi.IsComplete);//Third Entry Condition to go to return to idle. After an attack pattern, go to idle
        timer = 0;
        _statemachine.SetState(idle);

        void At(IState from, IState to, Func<bool> condition) => _statemachine.AddTransition(from, to, condition);



    }

    private void Update()
    {
        DoMiniSweep();
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


    public void DoMiniSweep()
    {
        _doMiniperi = false;
        _doRocketperi = true;
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
