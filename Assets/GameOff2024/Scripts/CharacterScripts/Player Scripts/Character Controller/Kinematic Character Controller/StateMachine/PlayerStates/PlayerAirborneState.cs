using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAirborneState : PlayerBaseState
{
    public PlayerAirborneState(PlayerKCC currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        _isRootState = true;
    }

    public override void SetInput(ref PlayerCharacterInputs inputs)
    {

    }

    public override void EnterState()
    {
        //Debug.Log("Airborne");

        _ctx.CurrentCharacterState = ECharacterState.Default;

        _ctx.Motor.ForceUnground();

        if (_ctx._jumpRequested)
        {
            SetSubState(_factory.Jump());
        }
        else
        {
            SetSubState(_factory.Falling());
        }

    }

    public override void ExitState()
    {
        _ctx.Motor.SetCapsuleDimensions(_ctx._originalCapsuleRadius, _ctx._originalCapsuleHeight, 0);
    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {
    }

    public override void AfterCharacterUpdate(float deltaTime)
    {
        //_ctx.WallCheckLogic();
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        // Add move input
        if (_ctx._moveInputVector.sqrMagnitude > 0f)
        {
            Vector3 addedVelocity = _ctx._moveInputVector * _ctx._airAccelerationSpeed * deltaTime;

            Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, _ctx.Motor.CharacterUp);

            // Limit air velocity from inputs
            if (currentVelocityOnInputsPlane.magnitude < _ctx._maxAirMovespeed)
            {
                // clamp addedVel to make total vel not exceed max vel on inputs plane
                Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, _ctx._maxAirMovespeed);
                addedVelocity = newTotal - currentVelocityOnInputsPlane;
            }
            else
            {
                // Make sure added vel doesn't go in the direction of the already-exceeding velocity
                if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f)
                {
                    addedVelocity = Vector3.ProjectOnPlane(addedVelocity, currentVelocityOnInputsPlane.normalized);
                }
            }

            // Prevent air-climbing sloped walls
            if (_ctx.Motor.GroundingStatus.FoundAnyGround)
            {
                if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f)
                {
                    Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(_ctx.Motor.CharacterUp, _ctx.Motor.GroundingStatus.GroundNormal), _ctx.Motor.CharacterUp).normalized;
                    addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpenticularObstructionNormal);
                }
            }

            // Apply added velocity
            currentVelocity += addedVelocity;
        }
        else
        {
            _ctx._isMaintainingMomentum = false;
        }

        //Debug.Log("final fall: " + _ctx._finalFallVelocityY);

        if (_ctx.Motor.Velocity.y < 0f)
        {
            if (_ctx.Motor.Velocity.y < _ctx._finalFallVelocityY)
            {
                _ctx._finalFallVelocityY = _ctx.Motor.Velocity.y;
            }
        }
    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {

    }



    public override void CheckSwitchState()
    {

    }

    public override void InitializeSubStates()
    {
    }

    public override void UpdateState()
    {
    }
}
