using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private bool _playFallingAnimation;

    public PlayerFallingState(PlayerKCC currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        
    }

    public override void SetInput(ref PlayerCharacterInputs inputs)
    {
    }

    public override void EnterState()
    {
        //Debug.Log("Enter Falling State");
        _playFallingAnimation = true;
    }

    public override void ExitState()
    {
        _playFallingAnimation &= false;
    }

    public override void AfterCharacterUpdate(float deltaTime)
    {
    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        // Gravity

        if (currentVelocity.y < -10f)
        {
            currentVelocity += _ctx._gravity * deltaTime * 1.1f;
        }
        else
        {
            currentVelocity += _ctx._gravity * deltaTime;
        }

        // Drag
        currentVelocity *= (1f / (1f + (_ctx._drag * deltaTime)));
    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        if (_ctx._lookInputVector.sqrMagnitude > 0f && _ctx._orientationSharpness > 0f)
        {
            currentRotation = Quaternion.LookRotation(_ctx._lookInputVector, _ctx.Motor.CharacterUp);
        }

        Vector3 currentUp = (currentRotation * Vector3.up);

        Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, -_ctx._gravity.normalized, 1 - Mathf.Exp(-_ctx._bonusOrientationSharpness * deltaTime));
        currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
    }

    public override void CheckSwitchState()
    {
        if (_ctx.IsGrounded)
        {
            SwitchState(_factory.Grounded());
        }
    }

    public override void InitializeSubStates()
    {

    }

    public override void UpdateState()
    {
        if (_ctx.Motor.Velocity.y < -5f && _playFallingAnimation)
        {
            _playFallingAnimation = false;
        }
    }
}
