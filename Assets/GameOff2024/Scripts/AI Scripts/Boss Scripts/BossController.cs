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
using UnityEngine.Events;

public class BossController : MonoBehaviour
{
    private NavMeshAgent _agent;

    private StateMachine _statemachine;

    private PlayerDetector _playerDetector;

    [SerializeField] private GameObject BOSS;

    [SerializeField] private PlayerKCC _player;

    [Header ("Boss Agent Movement Parameters")]
    [SerializeField] private BossAgentParameters _bossparam;
    [Header ("Boss Status Manager")]
    [SerializeField] public BossStatusManager _statusManager;

    [SerializeField] public Animator _bosslower;

    #region ---Attack Flags---
    [Header("Attack Pattern Flags")]
    [SerializeField] private bool _doMiniperi;
    [SerializeField] private bool _doRocketperi;
    [SerializeField] private bool _doMinisweep;
    [SerializeField] private bool _doRocketsweep;
    [SerializeField] private bool _doRambo;
    [SerializeField] private bool _doSpine;
    [SerializeField] private bool _doBackshot;

    #endregion

    [Header("Boss Attack State Locked")]
    [SerializeField] public bool _isLocked;
    [SerializeField] public bool _attackselected = false;

    [SerializeField] public List<GameObject> _waypoints = new List<GameObject>();
    [SerializeField] public List<GameObject> _shootpoints = new List<GameObject>();
    public BossWeaponManager weaponManager;
    public PlayerStatusSO BossStatusSO;
    private Transform playerTransform;

    private float timer;

    [SerializeField] private float shootingRange;
    [SerializeField] private float aimOffset;


    //Boss Health
    [SerializeField] private float BossMaxHealth;
    [SerializeField] private float BossCurrentHealth;

    [Header("Events")]
    public UnityEvent OnMobDestroyed;


    [Header("Upper Body Model")]
    public GameObject _upperbody;


    private void Awake()
    {

        OnMobDestroyed.AddListener(() => GameObject.Find("Score").GetComponentInChildren<ScoreManager>().UpdateScore(1000));

        #region ---Reference Lines---
        _player = GameObject.Find("Player Controller").GetComponent<PlayerKCC>();
        _bosslower = GameObject.Find("Boss").GetComponentsInChildren<Animator>()[0];
        _statusManager = GetComponent<BossStatusManager>();
        _agent = GetComponent<NavMeshAgent>();
        _playerDetector = GetComponent<PlayerDetector>();
        _bossparam = GetComponent<BossAgentParameters>();
        #endregion

        _statemachine = new StateMachine();

        //Agent Movement
        _agent.enabled = false;
        _agent.stoppingDistance = 0f;

        #region ---Boss States---
        var idle = new BossIdle(this, _agent, _bossparam, _bosslower, _upperbody);
        var rambo = new BossRambo(this, _agent, _bossparam, _bosslower, _upperbody);
        var spine = new BossSpine(this, _agent, _bossparam, _bosslower, _upperbody);
        var minisweep = new MiniGunSweep(this, _agent, _bossparam, _bosslower, _upperbody);
        var rocketsweep = new RocketSweep(this, _agent, _bossparam, _bosslower, _upperbody);
        var backshot = new RocketBackShot(this, _agent, _bossparam, _bosslower, _upperbody);
        var miniperi = new MiniGunPerimeterSpray(this, _agent, _bossparam, _bosslower, _upperbody);
        var rocketperi = new RocketPerimeterSpray(this, _agent, _bossparam, _bosslower, _upperbody);
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
        _statemachine.Tick();
        //Get Boss Health
        BossMaxHealth = _statusManager.GetCurrentMaxHealth();
        BossCurrentHealth = _statusManager.GetCurrentHealth();
        OnBossDestroy();

        //DoMiniSweep();
               


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




        //SelectAttack();

    }

    public void MoveToCenter()
    {
        _agent.SetDestination(_waypoints[0].transform.position);
    }

    private void OnBossDestroy()
    {
     if(BossCurrentHealth <= 0)
        {
            OnMobDestroyed?.Invoke();
            BOSS.SetActive(false);
            Debug.Log("Boss Destroyed");
        }   
    }

    #region ---Shooting Mechanism Related---
    public void ShootMinigunAt(Vector3 Tobeshot)
    {
        //Debug.Log("shootingat" + Tobeshot);
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

    public void HoldShootMinigunAt(Vector3 Tobeshot, float firerate)
    {
        for (int i = 0; i < firerate; i++) {
            ShootMinigunAt(Tobeshot);
        }
        
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

    #region ---Attack Phases Related---
    private void SelectAttack()
    {
        if (!_isLocked && !_attackselected)
        {
            if (BossCurrentHealth > BossMaxHealth * 0.70f)
            {
                Debug.Log("Boss in Phase 1");
                int attackcase = UnityEngine.Random.Range(0,2);
                //Debug.Log(attackcase);
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
                Debug.Log("Boss in Phase 2");
                int attackcase = UnityEngine.Random.Range(0,4);
                //Debug.Log(attackcase);
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
                Debug.Log("Boss in Phase 3");
                int attackcase = UnityEngine.Random.Range(0,2);
                //Debug.Log(attackcase);
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
                Debug.Log("Boss in Phase 4");
                int attackcase = UnityEngine.Random.Range(0,3);
                attackcase = 0;
                //Debug.Log(attackcase);
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
                Debug.Log("Boss Destroyed. Unable to Select Attack");
            }
        }

        _attackselected = true;
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
        _attackselected = false;
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
