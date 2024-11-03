using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerKCC currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void SetInput(ref PlayerCharacterInputs inputs)
    {
    }

    public override void EnterState()
    {
        _ctx._animator.CrossFadeInFixedTime(_ctx.RUNNING, 0.05f);   
        //Debug.Log("Enter Run State");
        //_ctx.Motor.SetCapsuleDimensions(_ctx._runningCapsuleRadius, _ctx._runningCapsuleHeight, 0);
    }

    public override void ExitState()
    {
        //_ctx.Motor.SetCapsuleDimensions(_ctx._originalCapsuleRadius, _ctx._originalCapsuleHeight, 0);
    }

    public override void AfterCharacterUpdate(float deltaTime)
    {
    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        Vector3 inputRight = Vector3.Cross(_ctx._moveInputVector, _ctx.Motor.CharacterUp);
        Vector3 reorientedInput = Vector3.Cross(_ctx.Motor.GroundingStatus.GroundNormal, inputRight).normalized * _ctx._moveInputVector.magnitude;

        Vector3 targetMovementVelocity = reorientedInput * _ctx._runningSpeed;

        // Smooth movement Velocity
        currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-_ctx._stableMovementSharpness * deltaTime));
    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
    }

    public override void CheckSwitchState()
    {
        _ctx._runToStop = false;

        if (!_ctx.IsGrounded)
        {
            SwitchState(_factory.Airborne());
        }
        else if (!_ctx._isMaintainingMomentum)
        {
            SwitchState(_factory.Walk());
        }
        else if (_ctx._jumpRequested && !_ctx._jumpConsumed &&
                ((_ctx._allowJumpingWhenSliding ? _ctx.Motor.GroundingStatus.FoundAnyGround : _ctx.Motor.GroundingStatus.IsStableOnGround) ||
                _ctx._timeSinceLastAbleToJump <= _ctx._jumpPostGroundingGraceTime))
        {
            _ctx.Motor.ForceUnground();
            SwitchState(_factory.Airborne());
        }
        else if (_ctx._moveInputVector.sqrMagnitude == 0)
        {
            _ctx._runToStop = true;
            SwitchState(_factory.Idle());
        }
    }

    public override void InitializeSubStates()
    {

    }

    public override void UpdateState()
    {
    }
}
