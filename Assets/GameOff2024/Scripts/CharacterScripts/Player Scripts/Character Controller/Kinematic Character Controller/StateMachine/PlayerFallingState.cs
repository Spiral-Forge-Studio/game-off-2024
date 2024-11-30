using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    public PlayerFallingState(PlayerKCC currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        _isRootState = true;
    }

    public override void AfterCharacterUpdate(float deltaTime)
    {

    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {

    }

    public override void CheckSwitchState()
    {
        if (_ctx.IsGrounded)
        {
            if (_ctx._noFalling)
            {
                _ctx._preventFalling = true;
            }
            SwitchState(_factory.Grounded());
        }
    }

    public override void EnterState()
    {

    }

    public override void ExitState()
    {

    }

    public override void InitializeSubStates()
    {

    }

    public override void SetInput(ref PlayerControllerInputs inputs)
    {

    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        if (_ctx._lookInputVector.sqrMagnitude > 0f && _ctx._orientationSharpness > 0f)
        {
            // Smoothly interpolate from current to target look direction
            Vector3 smoothedLookInputDirection = Vector3.Slerp(_ctx.Motor.CharacterForward, _ctx._lookInputVector, 1 - Mathf.Exp(-_ctx._orientationSharpness * deltaTime)).normalized;

            // Set the current rotation (which will be used by the KinematicCharacterMotor)
            currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, _ctx.Motor.CharacterUp);
        }

        Vector3 currentUp = (currentRotation * Vector3.up);

        Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, -_ctx._gravity.normalized, 1 - Mathf.Exp(-_ctx._bonusOrientationSharpness * deltaTime));
        currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
    }

    public override void UpdateState()
    {

    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        currentVelocity += _ctx._gravity * deltaTime;
    }
}
