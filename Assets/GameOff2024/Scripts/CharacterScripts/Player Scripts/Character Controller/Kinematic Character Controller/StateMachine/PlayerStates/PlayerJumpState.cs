using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerJumpState : PlayerBaseState
{

    private bool _jumped;

    public PlayerJumpState(PlayerKCC currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        
    }

    public override void SetInput(ref PlayerCharacterInputs inputs)
    {

    }

    public override void EnterState()
    {
        //Debug.Log("Enter JUMP STATE");

        _ctx._animator.CrossFadeInFixedTime(_ctx.JUMPING, 0.1f);

        _jumped = false;
    }

    public override void ExitState()
    {
        _jumped = false;
    }

    public override void AfterCharacterUpdate(float deltaTime)
    {
    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {

    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        // Calculate jump direction before ungrounding
        Vector3 jumpDirection = _ctx.Motor.CharacterUp;
        if (_ctx.Motor.GroundingStatus.FoundAnyGround && !_ctx.Motor.GroundingStatus.IsStableOnGround)
        {
            jumpDirection = _ctx.Motor.GroundingStatus.GroundNormal;
        }

        // Makes the character skip ground probing/snapping on its next update. 
        // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
        _ctx.Motor.ForceUnground();

        // Add to the return velocity and reset jump state
        currentVelocity += (jumpDirection * _ctx._jumpUpSpeed) - Vector3.Project(currentVelocity, _ctx.Motor.CharacterUp);
        currentVelocity += (_ctx._moveInputVector * _ctx._jumpScalableForwardSpeed);

        _ctx._jumpRequested = false;
        _ctx._jumpConsumed = true;
        _ctx._jumpedThisFrame = true;
        _ctx._movementAbilityRequested = false;
        _jumped = true;
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
        if (_jumped)
        {
            SwitchState(_factory.Falling());
            // --------- Mantling -----------//
            //if (_ctx.LowObstacleInFront())
            //{
            //    // lowVault
            //    SwitchState(_factory.Vaulting());
            //}
            //else if (_ctx.MidObstacleInFront())
            //{
            //    // mediumVault
            //    SwitchState(_factory.Vaulting());
            //}
            // ------------------------------//
            //else
            //{
            //    SwitchState(_factory.Falling());
            //}
        }
    }

    public override void InitializeSubStates()
    {
    }

    public override void UpdateState()
    {
    }
}
