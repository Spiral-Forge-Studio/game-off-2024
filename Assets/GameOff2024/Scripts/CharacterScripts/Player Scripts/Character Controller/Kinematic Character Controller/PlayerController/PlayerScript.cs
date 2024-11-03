using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using UnityEngine.InputSystem;

namespace KinematicCharacterController
{
    public class PlayerScript : MonoBehaviour
    {
        [Header("References")]
        public PlayerKCC Character;
        public CameraManager CharacterCamera;

        [Header("Input Actions")]
        PlayerInput playerInput;

        InputAction _moveAction;
        InputAction _jumpAction;
        InputAction _run;
        InputAction _interact;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";

        private bool _openMenu;

        private Vector3 defaultPlanerDirection;

        [HideInInspector] public bool _endCutscene;
        [HideInInspector] public bool _disableMovement;
        [HideInInspector] public bool _disableRotation;
        [HideInInspector] private GameStateManager gameState;
        [HideInInspector] public float _mouseSensitivity;

        private void Awake()
        {
            _disableRotation = false;
            gameState = FindObjectOfType<GameStateManager>();
            _disableMovement = false;
            _endCutscene = false;
            _mouseSensitivity = 1f;
        }

        private void Start()
        {
            // New Input System
            playerInput = GetComponent<PlayerInput>();

            playerInput.SwitchCurrentActionMap("General");

            _moveAction = playerInput.actions.FindAction("Move");
            _jumpAction = playerInput.actions.FindAction("Jump");
            _run = playerInput.actions.FindAction("Run");
            _interact = playerInput.actions.FindAction("Interact");

            // --------------------------------------

            Cursor.lockState = CursorLockMode.Locked;

            // Tell camera to follow transform
            CharacterCamera.SetFollowTransform(Character._cameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            CharacterCamera.IgnoredColliders.Clear();
            CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());

            _openMenu = false;
        }

        private void Update()
        {
            if (!_endCutscene)
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    _openMenu = !_openMenu;
                }

                if (!_openMenu)
                {
                    gameState.isPaused = false;
                    HandleCharacterInput();

                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else if (_openMenu)
                {
                    gameState.isPaused = true;

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
        }

        private void LateUpdate()
        {
            if (!_openMenu)
            {
                // Handle rotating the camera along with physics movers
                if (CharacterCamera.RotateWithPhysicsMover && Character.Motor.AttachedRigidbody != null)
                {
                    if (Character.CurrentCharacterState == ECharacterState.Default)
                    {
                        CharacterCamera.PlanarDirection = Character.Motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * CharacterCamera.PlanarDirection;
                        CharacterCamera.PlanarDirection = Vector3.ProjectOnPlane(CharacterCamera.PlanarDirection, Character.Motor.CharacterUp).normalized;
                        defaultPlanerDirection = CharacterCamera.PlanarDirection;
                    }
                }

                HandleCameraInput();
            }
        }

        private void HandleCameraInput()
        {
            // Create the look input vector for the camera

            Vector3 lookInputVector = Vector3.up;

            if (!_disableRotation)
            {
                float mouseLookAxisUp = Input.GetAxisRaw(MouseYInput) * _mouseSensitivity;
                float mouseLookAxisRight = Input.GetAxisRaw(MouseXInput) * _mouseSensitivity;

                lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);
            }


            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }

            // Input for zooming the camera (disabled in WebGL because it can cause problems)
            float scrollInput = -Input.GetAxis(MouseScrollInput);
#if UNITY_WEBGL
        scrollInput = 0f;
#endif

            // Apply inputs to the camera
            CharacterCamera.UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);

            // Handle toggling zoom level
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                CharacterCamera.TargetDistance = (CharacterCamera.TargetDistance == 0f) ? CharacterCamera.DefaultDistance : 0f;
            }
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct

            if (!_disableMovement)
            {
                characterInputs.MoveAxisForward = _moveAction.ReadValue<Vector2>().y;
                characterInputs.MoveAxisRight = _moveAction.ReadValue<Vector2>().x;
            }

            characterInputs.CameraRotation = CharacterCamera.Transform.rotation;
            characterInputs.SpaceBar = _jumpAction.ReadValue<float>() == 1;
            characterInputs.LeftShiftHold = _run.ReadValue<float>() == 1;

            // Apply inputs to character
            Character.SetInputs(ref characterInputs);
        }
    }
}