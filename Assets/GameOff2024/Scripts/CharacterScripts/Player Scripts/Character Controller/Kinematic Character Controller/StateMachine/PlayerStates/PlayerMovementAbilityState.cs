using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAbilityState : PlayerBaseState
{
    private float _dashStartTime;

    public PlayerMovementAbilityState(PlayerKCC currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void SetInput(ref PlayerControllerInputs inputs)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Enter Movement Ability State");

        _dashStartTime = Time.time;
        _ctx._isUsingMovementAbility = true;
        _ctx._canUseMovementAbility = false;
    }

    public override void ExitState()
    {
        _ctx._timeSinceMovementAbilityLastUsed = 0;
    }

    public override void AfterCharacterUpdate(float deltaTime)
    {

    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        if (_ctx._moveInputVector.magnitude == 0)
        {
            Vector3 dashDirection = _ctx.Motor.GetDirectionTangentToSurface(_ctx.Motor.CharacterForward, _ctx.Motor.GroundingStatus.GroundNormal);
            dashDirection.y = 0;
            currentVelocity = dashDirection * _ctx._dashSpeed;
        }
        else
        {
            Vector3 inputRight = Vector3.Cross(_ctx._moveInputVector, _ctx.Motor.CharacterUp);
            Vector3 reorientedInput = Vector3.Cross(_ctx.Motor.GroundingStatus.GroundNormal, inputRight).normalized * _ctx._moveInputVector.magnitude;
            Vector3 targetMovementVelocity = reorientedInput * _ctx._dashSpeed;

            targetMovementVelocity.y = 0;

            currentVelocity = targetMovementVelocity;
        }
    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
    }

    public override void CheckSwitchState()
    {
        //if (!_ctx.IsGrounded)
        //{
        //    SwitchState(_factory.Falling());
        //}
        if (!_ctx._isUsingMovementAbility)
        {
            if (_ctx._moveInputVector.sqrMagnitude != 0)
            {
                SwitchState(_factory.Walk());
            }
            else
            {
                SwitchState(_factory.Idle());
            }
        }
    }

    public override void InitializeSubStates()
    {

    }

    public override void UpdateState()
    {
        if (Time.time - _dashStartTime > _ctx._dashDuration && _ctx._isUsingMovementAbility)
        {
            _ctx._isUsingMovementAbility = false;
        }
    }
}
