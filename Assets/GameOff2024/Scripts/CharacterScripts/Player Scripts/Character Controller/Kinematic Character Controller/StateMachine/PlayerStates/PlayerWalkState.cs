using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerKCC currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void SetInput(ref PlayerCharacterInputs inputs)
    {
    }

    public override void EnterState()
    {
        //Debug.Log("Enter Walk State");

        _ctx._animator.CrossFadeInFixedTime(_ctx.JOGGING, 0.1f);
    }

    public override void ExitState()
    {
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

        Vector3 targetMovementVelocity = reorientedInput * _ctx._walkingSpeed;

        // Smooth movement Velocity
        currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-_ctx._stableMovementSharpness * deltaTime));
    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
    }

    public override void CheckSwitchState()
    {
        if (!_ctx.IsGrounded)
        {
            SwitchState(_factory.Airborne());
        }
        else if (_ctx._jumpRequested && !_ctx._jumpConsumed &&
                ((_ctx._allowJumpingWhenSliding ? _ctx.Motor.GroundingStatus.FoundAnyGround : _ctx.Motor.GroundingStatus.IsStableOnGround) ||
                _ctx._timeSinceLastAbleToJump <= _ctx._jumpPostGroundingGraceTime))
        {
            _ctx.Motor.ForceUnground();
            SwitchState(_factory.Airborne());
        }
        else if (_ctx._isMaintainingMomentum)
        {
            SwitchState(_factory.Run());
        }
        else if (_ctx._moveInputVector.sqrMagnitude == 0)
        {
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
