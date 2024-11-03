using KinematicCharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
    protected bool _isRootState = false;
    protected PlayerKCC _ctx;
    protected PlayerStateFactory _factory;
    private PlayerBaseState _currentSuperState;
    private PlayerBaseState _currentSubState;

    protected PlayerBaseState(PlayerKCC currentContext, PlayerStateFactory playerStateFactory)
    {
        _ctx = currentContext;
        _factory = playerStateFactory;
    }

    public abstract void SetInput(ref PlayerCharacterInputs inputs);

    public void SetInputs(ref PlayerCharacterInputs inputs)
    {
        SetInput(ref inputs);

        if (_currentSubState != null)
        {
            _currentSubState.SetInputs(ref inputs);
        }
    }

    #region --- State Transitions/Updates ---
    public abstract void EnterState();
    public void EnterStates()
    {
        EnterState();

        if (_currentSubState != null)
        {
            _currentSubState.EnterState();
        }
    }

    public abstract void ExitState();
    public void ExitStates()
    {
        ExitState();

        if (_currentSubState != null)
        {
            _currentSubState.ExitState();
        }
    }

    public abstract void UpdateState();
    public void UpdateStates()
    {
        UpdateState();

        if (_currentSubState != null)
        {
            _currentSubState.UpdateState();
        }
    }
    #endregion


    #region --- KCC State Updates ---
    public abstract void BeforeCharacterUpdate(float deltaTime);
    public abstract void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime);
    public abstract void UpdateRotation(ref Quaternion currentRotation, float deltaTime);
    public abstract void AfterCharacterUpdate(float deltaTime);
    public void BeforeCharacterUpdates(float deltaTime)
    {
        BeforeCharacterUpdate(deltaTime);

        if (_currentSubState != null)
        {
            _currentSubState.BeforeCharacterUpdates(deltaTime);
        }
    }
    public void UpdateVelocities(ref Vector3 currentVelocity, float deltaTime)
    {
        UpdateVelocity(ref currentVelocity, deltaTime);

        if (_currentSubState != null)
        {
            _currentSubState.UpdateVelocities(ref currentVelocity, deltaTime);
        }
    }
    public void UpdateRotations(ref Quaternion currentRotation, float deltaTime)
    {
        UpdateRotation(ref currentRotation, deltaTime);

        if (_currentSubState != null)
        {
            _currentSubState.UpdateRotations(ref currentRotation, deltaTime);
        }
    }
    public void AfterCharacterUpdates(float deltaTime)
    {
        AfterCharacterUpdate(deltaTime);

        if (_currentSubState != null)
        {
            _currentSubState.AfterCharacterUpdates(deltaTime);
        }
    }

    #endregion

    public abstract void CheckSwitchState();

    public void CheckSwitchStates()
    {
        CheckSwitchState();

        if (_currentSubState != null)
        {
            _currentSubState.CheckSwitchState();
        }
    }

    public abstract void InitializeSubStates();

    protected void SwitchState(PlayerBaseState newState)
    {
        ExitStates();

        if (_isRootState)
        {
            _ctx.CurrentState = newState;
        }
        else if (_currentSuperState != null)
        {

            if (newState._isRootState)
            {
                _ctx.CurrentState = newState;
            }
            else
            {
                _currentSuperState.SetSubState(newState);
            }
        }

        newState.EnterStates();
    }

    protected void SetSuperState(PlayerBaseState newSuperState) 
    { 
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState) 
    { 
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }

}
