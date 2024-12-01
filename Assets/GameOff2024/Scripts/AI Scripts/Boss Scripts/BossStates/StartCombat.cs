using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCombat : IState
{
    private GameObject _center;
    private GameObject _player;
    private Animator _animator;

    private bool _isComplete;

    public bool CombatStart  => _isComplete;
    public StartCombat(GameObject center, GameObject player, Animator animation)
    {
        _center = center;
        _player = player;
        _animator = animation;
    }
    public void Tick() 
    {
        CalculatePosition();
    }
    public void OnEnter() { _animator.CrossFade("Armature|SB_Boss_Lower_Idle", 0.2f);  Debug.Log("Waiting for Player"); _isComplete = false; }

    public void OnExit() { }

    private void CalculatePosition()
    {
        float position = (_center.transform.position - _player.transform.position).magnitude;
        if(position < 18f)
        {
            _isComplete = true;
        }
    }
}
