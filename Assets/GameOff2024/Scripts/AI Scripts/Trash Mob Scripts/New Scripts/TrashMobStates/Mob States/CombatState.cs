using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatState : IState
{
    private readonly TrashMob mob;
    private UnityEngine.AI.NavMeshAgent _agent;
    //private readonly TrashMobParameters _parameters;

    private Animator _animator;
    private float _weaponrange;

    [HideInInspector] public float _combatspeed;
    public CombatState(TrashMob trashMob, UnityEngine.AI.NavMeshAgent agent, TrashMobParameters parameters, NPCWeaponType weapontype, WeaponParameters param, Animator _animator)
    {
        this._animator = _animator;
        mob = trashMob;
        _agent = agent;
        //_parameters = parameters;
        _combatspeed = parameters.combatspeed;
        
        switch(weapontype)
        {
            case NPCWeaponType.Rifle:
                _weaponrange = param.riflerange;
                break;
            case NPCWeaponType.Shotgun:
                _weaponrange = param.shotgunrange;
                break;
            case NPCWeaponType.Rocket:
                _weaponrange = param.rocketrange;
                break;
        }
    }
    public void OnEnter() 
    { 
        _agent.enabled = false; 
        _agent.speed = _combatspeed; 
        _agent.stoppingDistance = _weaponrange;

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
        {
            _animator.CrossFadeInFixedTime("Movement", 0.05f);
        }
    }
    public void Tick() { }
    public void OnExit() { }
}
