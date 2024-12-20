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
        TowardsMouse
    }

    public struct PlayerControllerInputs
    {
        public float MoveAxisForward;
        public float MoveAxisRight;
        public Quaternion CameraRotation;
        public bool Dash;

        public Vector3 mousePos;
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

        [Header("GOD MODE (make sure to turn off after using...)")]
        public bool _noFalling;
        [HideInInspector] public bool _preventFalling;

        [Header("[DO NOT REMOVE]")]
        public KinematicCharacterMotor Motor;
        public Transform _upperBodyTransform;
        public Animator _lowerBodyAnim;


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

        [Header("Movement Ability")]
        public float _dashSpeed;
        public float _dashDuration;
        [HideInInspector] public float _dashInternalCooldown;

        [HideInInspector] public bool _movementAbilityRequested;
        [HideInInspector] public bool _isUsingMovementAbility;
        [HideInInspector] public bool _canUseMovementAbility;
        [HideInInspector] public float _timeSinceMovementAbilityLastUsed = Mathf.Infinity;
        public TrailRenderer[] trails;
        public float trailDuration;
        public AudioSource movementSource;

        [HideInInspector] public float currentDashTime;

        [Header("Targeting")]
        public Vector3 _mousePos;

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
        public Vector3 _internalVelocityAdd = Vector3.zero;

        public float _moveInputForward;
        public float _moveInputRight;

        // State Management
        public EPlayerLocomotion newPlayerAction;

        [Header("Animation Logic Helper Variables")]

        public readonly int IDLE = Animator.StringToHash("Idle");
        public readonly int WALKING = Animator.StringToHash("Walking");


        #endregion

        private void Awake()
        {
            // State Machine
            _states = new PlayerStateFactory(this);
            _currentState = _states.Grounded();
            _currentState.EnterStates();

            newPlayerAction = EPlayerLocomotion.None;

            // Assign the characterController to the motor
            Motor.CharacterController = this;

            _preventFalling = false;
        }

        public PlayerStatusSO playerStats;

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


            Quaternion upperXRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            _upperBodyTransform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(_lookInputVector, Vector3.up)) * upperXRotation;
        }

        #region --- Inputs ---
        /// <summary>
        /// This is called every frame by the player in order to tell the character what its inputs are
        /// </summary>
        public void SetInputs(ref PlayerControllerInputs inputs)
        {
            _moveInputForward = inputs.MoveAxisForward;
            _moveInputRight = inputs.MoveAxisRight;

            if (inputs.Dash && _canUseMovementAbility)
            {
                _movementAbilityRequested = true;
            }
            else
            {
                _movementAbilityRequested = false;
            }

            // Clamp input
            Vector3 moveInputVector = Vector3.ClampMagnitude(new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward), 1f);

            // Calculate camera direction and rotation on the character plane
            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, Motor.CharacterUp).normalized;
            
            if (cameraPlanarDirection.sqrMagnitude == 0f)
            {
                cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.up, Motor.CharacterUp).normalized;
            }

            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Motor.CharacterUp);

            // Move and look inputs
            _moveInputVector = cameraPlanarRotation * moveInputVector;

            //Vector3 mouseAimDirection = new Vector3(inputs.mousePos.x - transform.position.x, transform.position.y, inputs.mousePos.z - transform.position.z);
            Vector3 mouseAimDirection = Vector3.ProjectOnPlane(inputs.mousePos - transform.position, Motor.CharacterUp);

            switch (_orientationMethod)
            {
                case OrientationMethod.TowardsCamera:
                    _lookInputVector = cameraPlanarDirection;
                    break;
                case OrientationMethod.TowardsMovement:
                    _lookInputVector = _moveInputVector.normalized;
                    break;
                case OrientationMethod.TowardsMouse:
                    _lookInputVector = mouseAimDirection.normalized;
                    break;
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
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its rotation should be right now. 
        /// This is the ONLY place where you should set the character's rotation
        /// </summary>
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            _currentState.UpdateRotations(ref currentRotation, deltaTime);
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its velocity should be right now. 
        /// This is the ONLY place where you can set the character's velocity
        /// </summary>
        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            // Handle Dashing
            _timeSinceMovementAbilityLastUsed += deltaTime;

            _currentState.UpdateVelocities(ref currentVelocity, deltaTime);

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

            

            if (!_isUsingMovementAbility && _timeSinceMovementAbilityLastUsed > _dashInternalCooldown)
            {
                _canUseMovementAbility = true;
                currentDashTime = playerStats.DashCooldown;
            }
            else if (!_isUsingMovementAbility)
            {
                currentDashTime += deltaTime;
            }

        }

        public void PostGroundingUpdate(float deltaTime)
        {

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
            _internalVelocityAdd += velocity;
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
            Debug.Log(hitCollider.gameObject.name);
        }

        public IEnumerator enableDashTrails()
        {
            for (int i = 0; i < trails.Length; i++)
            {
                trails[i].enabled = true;
            }

            yield return new WaitForSeconds(trailDuration);

            for (int i = 0; i < trails.Length; i++)
            {
                trails[i].enabled = false;
            }
        }
    }
}