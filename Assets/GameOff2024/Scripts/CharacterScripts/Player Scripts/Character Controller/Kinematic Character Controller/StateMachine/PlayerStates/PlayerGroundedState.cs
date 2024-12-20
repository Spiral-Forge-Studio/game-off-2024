using KinematicCharacterController;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerKCC currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        _isRootState = true;
    }

    public override void SetInput(ref PlayerControllerInputs inputs)
    {
    }

    public override void EnterState()
    {
        SetSubState(_factory.Idle());
    }

    public override void ExitState()
    {

    }

    public override void AfterCharacterUpdate(float deltaTime)
    {
        //_ctx.WallCheckLogic();
    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {
        
    }

    public override void UpdateState()
    {
        
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        float currentVelocityMagnitude = currentVelocity.magnitude;

        Vector3 effectiveGroundNormal = _ctx.Motor.GroundingStatus.GroundNormal;

        // Reorient velocity on slope
        currentVelocity = _ctx.Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;
    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        // lower torso rotation
        if (_ctx._moveInputVector.sqrMagnitude > 0f && _ctx._orientationSharpness > 0f)
        {
            // Smoothly interpolate from current to target look direction
            Vector3 smoothedLookInputDirection = Vector3.Slerp(_ctx.Motor.CharacterForward, _ctx._moveInputVector, 1 - Mathf.Exp(-_ctx._orientationSharpness * deltaTime)).normalized;

            // Set the current rotation (which will be used by the KinematicCharacterMotor)
            currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, _ctx.Motor.CharacterUp);
        }

        Vector3 currentUp = (currentRotation * Vector3.up);

        Vector3 smoothedGravityDirLower = Vector3.Slerp(currentUp, -_ctx._gravity.normalized, 1 - Mathf.Exp(-_ctx._bonusOrientationSharpness * deltaTime));
        currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDirLower) * currentRotation;

        Quaternion upperXRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        _ctx._upperBodyTransform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(_ctx._lookInputVector, Vector3.up)) * upperXRotation;
    }

    public override void CheckSwitchState()
    {

    }

    public override void InitializeSubStates()
    {

    }
}
