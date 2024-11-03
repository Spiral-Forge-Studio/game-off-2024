using KinematicCharacterController;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerKCC currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        _isRootState = true;
    }

    public override void SetInput(ref PlayerCharacterInputs inputs)
    {
    }

    public override void EnterState()
    {
        //Debug.Log("Grounded, " + _ctx._finalFallVelocityY + ", " + _ctx._isMaintainingMomentum);

        if (_ctx._isMaintainingMomentum)
        {
            SetSubState(_factory.Run());
        }
        else
        {
            SetSubState(_factory.Idle());
        }

        _ctx.CurrentCharacterState = ECharacterState.Default;
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

        currentVelocity += _ctx._gravity * deltaTime;
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

    public override void CheckSwitchState()
    {

    }

    public override void InitializeSubStates()
    {

    }
}
