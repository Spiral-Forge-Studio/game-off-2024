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

    public override void SetInput(ref PlayerControllerInputs inputs)
    {

    }

    public override void EnterState()
    {
        _ctx._lowerBodyAnim.CrossFadeInFixedTime(_ctx.IDLE, 0.2f);
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
        if (!_ctx.IsGrounded && !_ctx._noFalling)
        {
            SwitchState(_factory.Falling());
        }
        else if (_ctx._movementAbilityRequested && _ctx._canUseMovementAbility)
        {
            SwitchState(_factory.Dash());
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
