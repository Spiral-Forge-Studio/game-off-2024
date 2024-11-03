using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    private AnimatorStateInfo m_animState;
    private bool _playIdleAnim;

    public PlayerIdleState(PlayerKCC currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void SetInput(ref PlayerCharacterInputs inputs)
    {

    }

    public override void EnterState()
    {

        _ctx._animator.CrossFadeInFixedTime(_ctx.STANDING_IDLE, 0.1f);

        //Debug.Log("Enter Idle State");
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
        currentVelocity = Vector3.zero;
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
            SwitchState(_factory.Airborne());
        }

        else if (_ctx._moveInputVector.sqrMagnitude != 0)
        {
            SwitchState(_factory.Walk());
        }

    }

    public override void InitializeSubStates()
    {
    }

    public override void UpdateState()
    {

    }
}
