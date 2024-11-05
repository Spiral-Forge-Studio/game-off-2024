using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KinematicCharacterController
{
    public enum EPlayerActionState
    {
        Grounded,
        Walk,
        Idle,
        Dash
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
            _states.Add(EPlayerActionState.Grounded, new PlayerGroundedState(_context, this));
            _states.Add(EPlayerActionState.Dash, new PlayerMovementAbilityState(_context, this));
        }

        public PlayerBaseState Walk() { return _states[EPlayerActionState.Walk]; }
        public PlayerBaseState Idle() { return _states[EPlayerActionState.Idle]; }

        public PlayerBaseState Grounded() { return _states[EPlayerActionState.Grounded]; }
        public PlayerBaseState Dash() { return _states[EPlayerActionState.Dash]; }
    }
}

