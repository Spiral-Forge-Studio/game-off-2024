using KinematicCharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using Unity.VisualScripting;
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
    [SerializeField] private bool _doMinisweep;
    [SerializeField] private bool _doRocketsweep;
    [SerializeField] private bool _doRambo;
    [SerializeField] private bool _doSpine;
    [SerializeField] private bool _doBackshot;

    [Header("Boss Attack State Locked")]
    [SerializeField] public bool _isLocked;

    [SerializeField] public List<GameObject> _waypoints = new List<GameObject>();
    public BossWeaponManager weaponManager;
    public BossStatusSO BossStatusSO;
    private Transform playerTransform;

    private float timer;

    [SerializeField] private float shootingRange;
    [SerializeField] private float aimOffset;


    //Boss Health
    private float BossMaxHealth;
    private float BossCurrentHealth;


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

        #region ---Boss States---
        var idle = new BossIdle(this, _agent, _bossparam);
        var rambo = new BossRambo(this, _agent, _bossparam);
        var spine = new BossSpine(this, _agent, _bossparam);
        var minisweep = new MiniGunSweep(this, _agent, _bossparam);
        var rocketsweep = new RocketSweep(this, _agent, _bossparam);
        var backshot = new RocketBackShot(this, _agent, _bossparam);
        var miniperi = new MiniGunPerimeterSpray(this, _agent, _bossparam);
        var rocketperi = new RocketPerimeterSpray(this, _agent, _bossparam);
        #endregion

        #region ---Boss Enter Phase Condition---
        At(idle, miniperi, () => _doMiniperi && idle.IsIdle ); //Third Entry Condition to go to next state. If beginning, from idle go to minisweep as first attack move
        At(idle, rocketperi, () => _doRocketperi && idle.IsIdle); //Only go to this transition if miniperi has concluded before
        At(idle, minisweep, () => _doMinisweep && idle.IsIdle );
        At(idle, rocketsweep, () => _doRocketsweep && idle.IsIdle );
        At(idle, rambo, () => _doRambo && idle.IsIdle);
        At(idle, spine, () => _doSpine && idle.IsIdle);
        At(idle, backshot, () => _doBackshot && idle.IsIdle);

        #endregion

        #region ---Return to Idle---
        At(miniperi, idle, () => miniperi.IsComplete);  //Third Entry Condition to go to return to idle. After an attack pattern, go to idle
        At(rocketperi, idle, () => rocketperi.IsComplete);//Third Entry Condition to go to return to idle. After an attack pattern, go to idle
        At(minisweep, idle, () => minisweep.IsComplete);
        At(rocketsweep, idle, () => rocketsweep.IsComplete);
        At(rambo, idle, () => rambo.IsComplete);
        At(spine, idle, () => spine.IsComplete);
        At(backshot, idle, () => backshot.IsComplete);

        #endregion

        timer = 0;
        _statemachine.SetState(idle);

        void At(IState from, IState to, Func<bool> condition) => _statemachine.AddTransition(from, to, condition);

    }

    private void Update()
    {
        //DoMiniSweep();
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
        //ShootMinigunAt(aimPosition);
        //ShootRocketAt(aimPosition);
        //BackShotAt(aimPosition, BossStatusSO.RocketMagazineSize);




    }

    public void MoveToCenter()
    {
        _agent.SetDestination(_waypoints[0].transform.position);
    }


    public void DoMiniSweep()
    {
        _doMiniperi = false;
        _doRocketperi = false;
        _doRambo = false;
        _doSpine = true;
    }

    #region ---Shooting Mechanism Related---
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

    #endregion

    #region ---Boss Parameters SO Related---
    public void SetHealth()
    {

    }
    public float BossHealth()
    {

        return BossStatusSO.Health;
    }
    #endregion

    #region ---Attack Phases Related---
    private void SelectAttack()
    {
        if (!_isLocked)
        {
            if (BossCurrentHealth > BossMaxHealth * 0.70f)
            {
                int attackcase = UnityEngine.Random.Range(0,1);
                switch (attackcase)
                {
                    case 0:
                        _doMiniperi = true; 
                        break;
                    case 1:
                        _doRocketperi = true;
                        break;
                }
            }

            else if (BossCurrentHealth > BossMaxHealth * 0.50f && BossCurrentHealth <= BossMaxHealth * 0.70f)
            {
                int attackcase = UnityEngine.Random.Range(0,3);
                switch (attackcase)
                {
                    case 0:
                        _doMinisweep = true;
                        break;
                    case 1:
                        _doRocketsweep = true;
                        break;
                    case 2:
                        _doMiniperi = true;
                        break;
                    case 3:
                        _doRocketperi = true;
                        break;
                }
            }

            else if (BossCurrentHealth > BossMaxHealth * 0.25f && BossCurrentHealth <= BossMaxHealth * 0.50f)
            {
                int attackcase = UnityEngine.Random.Range(0,1);
                switch (attackcase)
                {
                    case 0:
                        _doRambo = true;
                        break;
                    case 1:
                        _doSpine = true;
                        break;
                }
            }

            else if (BossCurrentHealth > 0f && BossCurrentHealth <= BossMaxHealth * 0.25f)
            {
                int attackcase = UnityEngine.Random.Range(0,2);
                switch (attackcase)
                {
                    case 0:
                        _doRambo = true;
                        break;
                    case 1:
                        _doBackshot = true;
                        break;
                    case 2:
                        _doBackshot = true;
                        break;
                }
            }

            else if (BossCurrentHealth <= 0f)
            {
                Debug.Log("Boss Destroyed. Unaable to Select Attack");
            }
        }

        else return;
    }

    public void ResetAttackFlags()
    {
        _doMiniperi = false;
        _doRocketperi = false;
        _doMinisweep = false;
        _doRocketsweep = false;
        _doRambo = false;
        _doSpine = false;
        _doBackshot = false;
    }
    #endregion

    //To Do
    // 1.) Integrate Shooting Mechanics for 
    //       a. Rambo
    //       b. Spine
    //       c. Minigun and Rocket Sweep
    //       d. Minigun and Rocket Perimeter
    //       e. BackShot
    // 2.) Integrate Health Threshold for Attack Phases
    // 3.) OnDestroyBoss Function
    // 4.) ActivateBoss Function
}
