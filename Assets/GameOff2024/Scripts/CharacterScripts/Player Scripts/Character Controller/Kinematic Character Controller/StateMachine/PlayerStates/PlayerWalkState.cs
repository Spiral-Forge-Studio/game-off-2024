using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerKCC currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void SetInput(ref PlayerControllerInputs inputs)
    {
    }

    public override void EnterState()
    {
        _ctx._lowerBodyAnim.CrossFadeInFixedTime(_ctx.WALKING, 0.2f);
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
        if (!_ctx.IsGrounded && !_ctx._noFalling)
        {
            SwitchState(_factory.Falling());
        }
        else if (_ctx._movementAbilityRequested && _ctx._canUseMovementAbility)
        {
            SwitchState(_factory.Dash());
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
