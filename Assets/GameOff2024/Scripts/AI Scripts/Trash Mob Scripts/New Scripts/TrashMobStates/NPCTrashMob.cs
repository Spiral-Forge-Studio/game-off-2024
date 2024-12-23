using KinematicCharacterController;
using System;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class TrashMob : MonoBehaviour
{
    //Import NPC Parameters
    private TrashMobParameters _parameters;
    private NavMeshAgent _agent;
    //private PlayerKCC _player;
    //[HideInInspector] public Vector3 _playerpos;
    private StateMachine _statemachine;

    //Shooting related
    private NPCProjectileShooter _poolshooter;

    //Set NPC Weapon Type
    [SerializeField] private NPCWeaponType _weaponType;
    [SerializeField] private WeaponParameters _weaponparam;

    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _parameters = GetComponent<TrashMobParameters>();
        _animator = GetComponentInChildren<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _poolshooter = GetComponentInChildren<NPCProjectileShooter>();
        _weaponparam = GetComponentInChildren<WeaponParameters>();  
        if(_poolshooter == null) { Debug.LogWarning("Shooter not referenced"); }
        if (_weaponparam == null) { Debug.LogWarning("Shooter not referenced"); }

        var playerdetector = GetComponentInChildren<PlayerDetector>();

        _statemachine = new StateMachine();

        var idle = new IdleState(this, _agent, _parameters, _animator);
        var combat = new CombatState(this, _agent, _parameters, _weaponType, _weaponparam, _animator);
        var shoot = new ShootState(this, _agent, _poolshooter, _animator);



        //Idle -> Combat
        //Combat -> Shoot -> Reload -> Dodge

        At(combat, shoot, () => playerdetector.PlayerInRange);
        At(idle, combat, () => playerdetector.PlayerInRange);
        _statemachine.AddAnyTransition(idle, () => playerdetector.PlayerInRange == false); // Functions like interrupt then to priority
       


        _statemachine.SetState(idle);

        void At(IState from, IState to, Func<bool> condition) => _statemachine.AddTransition(from, to, condition);
    }

    private void Update() 
    {   
        _animator.SetFloat("AI Speed", _agent.velocity.magnitude);
        _statemachine.Tick();
    }

}
