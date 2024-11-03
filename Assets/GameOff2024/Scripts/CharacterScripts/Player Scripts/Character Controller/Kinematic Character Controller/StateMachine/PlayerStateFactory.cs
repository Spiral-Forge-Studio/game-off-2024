using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KinematicCharacterController
{
    public enum EPlayerActionState
    {
        Default,
        Grounded,
        Airborne,

        Walk,
        Idle,
        Falling,
        Run,
        Jump,
        Landing,
    }

    public class PlayerStateFactory
    {
        PlayerKCC _context;
        public Dictionary<EPlayerActionState, PlayerBaseState> _states = new Dictionary<EPlayerActionState, PlayerBaseState>();

        public PlayerStateFactory(PlayerKCC currentContext)
        {
            _context = currentContext;

            _states.Add(EPlayerActionState.Walk, new PlayerWalkState(_context, this));
            _states.Add(EPlayerActionState.Idle, new PlayerIdleState(_context, this));
            _states.Add(EPlayerActionState.Falling, new PlayerFallingState(_context, this));
            _states.Add(EPlayerActionState.Run, new PlayerRunState(_context, this));
            _states.Add(EPlayerActionState.Jump, new PlayerJumpState(_context, this));

            _states.Add(EPlayerActionState.Grounded, new PlayerGroundedState(_context, this));
            _states.Add(EPlayerActionState.Airborne, new PlayerAirborneState(_context, this));
        }

        public PlayerBaseState Walk() { return _states[EPlayerActionState.Walk]; }
        public PlayerBaseState Idle() { return _states[EPlayerActionState.Idle]; }
        public PlayerBaseState Falling() { return _states[EPlayerActionState.Falling]; }
        public PlayerBaseState Run() { return _states[EPlayerActionState.Run]; }
        public PlayerBaseState Jump() { return _states[EPlayerActionState.Jump]; }

        public PlayerBaseState Grounded() { return _states[EPlayerActionState.Grounded]; }
        public PlayerBaseState Airborne() { return _states[EPlayerActionState.Airborne]; }
    }
}

