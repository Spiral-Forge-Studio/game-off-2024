using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralState : PlayerBaseState
{
    public GeneralState(PlayerKCC currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void SetInput(ref PlayerCharacterInputs inputs)
    {
    }

    public override void EnterState()
    {
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
