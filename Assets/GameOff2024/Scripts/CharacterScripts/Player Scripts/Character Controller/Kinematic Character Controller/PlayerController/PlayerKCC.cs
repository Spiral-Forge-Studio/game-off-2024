using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;


namespace KinematicCharacterController
{
    public enum EPlayerLocomotion
    {
        // Basic Locomition
        None,
        Jumping,
        RunningJump,
        SoftLanding,
        Rolling,
        HardLanding
    }

    public enum ECharacterState
    {
        Default,
        Grounded,
        Airborne
    }

    public enum OrientationMethod
    {
        TowardsCamera,
        TowardsMovement,
    }

    public struct PlayerCharacterInputs
    {
        public float MoveAxisForward;
        public float MoveAxisRight;
        public Quaternion CameraRotation;
        public bool SpaceBar;
        public bool LeftShiftHold;

        public bool Interact;
    }

    public struct AICharacterInputs
    {
        public Vector3 MoveVector;
        public Vector3 LookVector;
    }

    public enum BonusOrientationMethod
    {
        None,
        TowardsGravity,
        TowardsGroundSlopeAndGravity,
    }


    public class PlayerKCC : MonoBehaviour, ICharacterController
    {
        #region --- Variables ---

        [Header("[DO NOT REMOVE]")]
        public KinematicCharacterMotor Motor;
        public Animator _animator;

        [Header("State Machine")]
        PlayerBaseState _currentState;
        PlayerStateFactory _states;

        public bool IsGrounded { get { return Motor.GroundingStatus.IsStableOnGround; } }

        public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

        [Header("Ground Movement")]
        public float _walkingSpeed = 10f;
        public float _runningSpeed = 20f;
        public float _stableMovementSharpness = 15f;
        public float _orientationSharpness = 10f;
        public OrientationMethod _orientationMethod = OrientationMethod.TowardsCamera;
        public bool _isMaintainingMomentum;
        public float _runningCapsuleRadius;
        public float _runningCapsuleHeight;

        [Header("[DEBUG] Ground Movement")]
        public bool _runToStop;


        [Header("Air Movement")]
        public float _maxAirMovespeed = 15f;
        public float _airAccelerationSpeed = 15f;
        public float _drag = 0.1f;

        [Header("Dashing")]
        public float _dashSpeed;
        public bool _movementAbilityRequested;
        public bool _isUsingMovementAbility;
        public float _timeSinceMovementAbilityRequested = Mathf.Infinity;
        public float _dashInternalCoolDown;
        public float _dashingCapsuleRadius;
        public float _dashingCapsuleHeight;

        [Header("Jumping")]
        public bool _allowJumpingWhenSliding = false;
        public float _jumpUpSpeed = 10f;
        public float _jumpScalableForwardSpeed = 10f;
        public float _jumpPreGroundingGraceTime = 0f;
        public float _jumpPostGroundingGraceTime = 0f;

        [Header("Parkour Variables")]
        public LayerMask _wallLayers;



        [Header("Misc")]
        public List<Collider> _ignoredColliders = new List<Collider>();
        public BonusOrientationMethod _bonusOrientationMethod = BonusOrientationMethod.None;
        public float _bonusOrientationSharpness = 10f;
        public Vector3 _gravity = new Vector3(0, -30f, 0);
        public Transform _meshRoot;
        public Transform _cameraFollowPoint;
        public float _originalCapsuleHeight;
        public float _originalCapsuleRadius;

        private Collider[] _probedColliders = new Collider[8];
        private RaycastHit[] _probedHits = new RaycastHit[8];
        public Vector3 _moveInputVector;
        public Vector3 _lookInputVector;
        public bool _jumpRequested = false;
        public bool _jumpConsumed = false;
        public bool _jumpedThisFrame = false;
        public float _timeSinceJumpRequested = Mathf.Infinity;
        public float _timeSinceLastAbleToJump = 0f;
        public Vector3 _internalVelocityAdd = Vector3.zero;

        public float _moveInputForward;
        public float _moveInputRight;
        public bool _interact;

        // State Management
        public EPlayerLocomotion newPlayerAction;
        public ECharacterState CurrentCharacterState;

        [Header("Animation Logic Helper Variables")]

        public readonly int STANDING_IDLE = Animator.StringToHash("Standing Idle");
        public readonly int RUNNING = Animator.StringToHash("Running");
        public readonly int JOGGING = Animator.StringToHash("Jogging");
        public readonly int JUMPING = Animator.StringToHash("Jumping");

        public float _fallToRollVelocityY;
        public float _finalFallVelocityY;

        #endregion

        private void Awake()
        {
            // State Machine
            _states = new PlayerStateFactory(this);
            _currentState = _states.Grounded();
            _currentState.EnterStates();

            // ----------------------------------- //

            _animator = GetComponent<Animator>();

            // ----------------------------------- //

            // Handle initial state
            TransitionToState(ECharacterState.Default);

            newPlayerAction = EPlayerLocomotion.None;

            // Assign the characterController to the motor
            Motor.CharacterController = this;
        }

        private void Start()
        {
            Motor.SetCapsuleCollisionsActivation(true);

            _originalCapsuleHeight = Motor.Capsule.height;
            _originalCapsuleRadius = Motor.Capsule.radius;
        }

        private void Update()
        {
            _currentState.UpdateStates();
            _currentState.CheckSwitchStates();
        }

        #region --- State Machine ---

        /// <summary>
        /// Handles movement state transitions and enter/exit callbacks
        /// </summary>
        public void TransitionToState(ECharacterState newState)
        {
            ECharacterState tmpInitialState = CurrentCharacterState;
            OnStateExit(tmpInitialState, newState);
            CurrentCharacterState = newState;
            OnStateEnter(newState, tmpInitialState);
        }

        /// <summary>
        /// Event when entering a state
        /// </summary>
        public void OnStateEnter(ECharacterState state, ECharacterState fromState)
        {
            switch (state)
            {
                case ECharacterState.Default:
                    {
                        break;
                    }
            }
            //Debug.Log("Current State: " + CurrentCharacterState);
        }

        /// <summary>
        /// Event when exiting a state
        /// </summary>
        public void OnStateExit(ECharacterState state, ECharacterState toState)
        {
            switch (state)
            {
                case ECharacterState.Default:
                    {
                        break;
                    }
            }
        }

        #endregion

        #region --- Inputs ---
        /// <summary>
        /// This is called every frame by the player in order to tell the character what its inputs are
        /// </summary>
        public void SetInputs(ref PlayerCharacterInputs inputs)
        {
            _moveInputForward = inputs.MoveAxisForward;
            _moveInputRight = inputs.MoveAxisRight;

            // Clamp input
            Vector3 moveInputVector = Vector3.ClampMagnitude(new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward), 1f);

            // Calculate camera direction and rotation on the character plane
            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, Motor.CharacterUp).normalized;
            
            if (cameraPlanarDirection.sqrMagnitude == 0f)
            {
                cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.up, Motor.CharacterUp).normalized;
            }

            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Motor.CharacterUp);

            // Jumping input
            SetJumpInputs(inputs.SpaceBar);

            switch (CurrentCharacterState)
            {
                case ECharacterState.Default:
                    {
                        // Move and look inputs
                        _moveInputVector = cameraPlanarRotation * moveInputVector;

                        switch (_orientationMethod)
                        {
                            case OrientationMethod.TowardsCamera:
                                _lookInputVector = cameraPlanarDirection;
                                break;
                            case OrientationMethod.TowardsMovement:
                                _lookInputVector = _moveInputVector.normalized;
                                break;
                        }

                        break;
                    }
            }

            if (inputs.LeftShiftHold)
            {
                _isMaintainingMomentum = true;
            }
            else
            {
                _isMaintainingMomentum = false;
            }

            _currentState.SetInputs(ref inputs);
        }

        /// <summary>
        /// This is called every frame by the AI script in order to tell the character what its inputs are
        /// </summary>
        public void SetInputs(ref AICharacterInputs inputs)
        {
            _moveInputVector = inputs.MoveVector;
            _lookInputVector = inputs.LookVector;
        }

        public void SetJumpInputs(bool jump)
        {
            if (jump)
            {
                _timeSinceJumpRequested = 0f;
                _jumpRequested = true;
            }
        }

        public Vector2 SquareToCircle(Vector2 input)
        {
            return (input.sqrMagnitude >= 1f)? input.normalized : input;
        }

        #endregion


        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called before the character begins its movement update
        /// </summary>
        public void BeforeCharacterUpdate(float deltaTime)
        {
            _currentState.BeforeCharacterUpdates(deltaTime);


            switch (CurrentCharacterState)
            {
                case ECharacterState.Default:
                    {
                        break;
                    }
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its rotation should be right now. 
        /// This is the ONLY place where you should set the character's rotation
        /// </summary>
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            switch (CurrentCharacterState)
            {
                case ECharacterState.Default:
                    {
                        _currentState.UpdateRotations(ref currentRotation, deltaTime);
                        //UpdateRotation_Default(ref currentRotation, deltaTime);

                        break;
                    }
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its velocity should be right now. 
        /// This is the ONLY place where you can set the character's velocity
        /// </summary>
        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {

            // Handle jumping
            _jumpedThisFrame = false;
            _timeSinceJumpRequested += deltaTime;

            // Handle Dashing
            _timeSinceMovementAbilityRequested += deltaTime;

            switch (CurrentCharacterState)
            {
                case ECharacterState.Default:
                    {
                        _currentState.UpdateVelocities(ref currentVelocity, deltaTime);

                        break;
                    }
            }

            // Take into account additive velocity
            if (_internalVelocityAdd.sqrMagnitude > 0f)
            {
                currentVelocity += _internalVelocityAdd;
                _internalVelocityAdd = Vector3.zero;
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called after the character has finished its movement update
        /// </summary>
        public void AfterCharacterUpdate(float deltaTime)
        {
            _currentState.AfterCharacterUpdates(deltaTime);

            // Handle jump-related values
            {
                if (_isMaintainingMomentum && _moveInputVector.sqrMagnitude == 0)
                {
                    _isMaintainingMomentum = false;
                }

                // [RETAIN]
                if (_movementAbilityRequested && _timeSinceMovementAbilityRequested > _dashInternalCoolDown)
                {
                    _movementAbilityRequested = false;

                    _isUsingMovementAbility = false;
                }

                // Handle jumping pre-ground grace period
                if (_jumpRequested && _timeSinceJumpRequested > _jumpPreGroundingGraceTime)
                {
                    _jumpRequested = false;
                }

                if (_allowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround)
                {
                    // If we're on a ground surface, reset jumping values
                    if (!_jumpedThisFrame)
                    {
                        _jumpConsumed = false;
                    }

                    _timeSinceLastAbleToJump = 0f;
                }

                else
                {
                    // Keep track of time since we were last able to jump (for grace period)
                    _timeSinceLastAbleToJump += deltaTime;
                }
            }
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            // Handle landing and leaving ground
            if (Motor.GroundingStatus.IsStableOnGround && !Motor.LastGroundingStatus.IsStableOnGround)
            {
                OnLanded();
            }
            else if (!Motor.GroundingStatus.IsStableOnGround && Motor.LastGroundingStatus.IsStableOnGround)
            {
                OnLeaveStableGround();
            }
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            if (_ignoredColliders.Count == 0)
            {
                return true;
            }

            if (_ignoredColliders.Contains(coll))
            {
                return false;
            }

            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            //Debug.Log(hitCollider.gameObject.name);
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            //Debug.Log("mmovehit " + hitCollider.gameObject.name);
        }

        public void AddVelocity(Vector3 velocity)
        {
            switch (CurrentCharacterState)
            {
                case ECharacterState.Default:
                    {
                        Debug.Log("add velocity called");
                        _internalVelocityAdd += velocity;
                        break;
                    }
            }
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        protected void OnLanded()
        {
        }

        protected void OnLeaveStableGround()
        {
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
            Debug.Log(hitCollider.gameObject.name);
        }
    }
}