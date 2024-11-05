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
        public CameraFollow CharacterCamera;

        [Header("Input Actions")]
        PlayerInput playerInput;

        InputAction _moveAction;
        InputAction _dash;
        InputAction _leftShoot;
        InputAction _rightShoot;
        InputAction _leftSwap;
        InputAction _rightSwap;

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
            _dash = playerInput.actions.FindAction("Dash");

            _leftShoot = playerInput.actions.FindAction("Left Shoot");
            _rightShoot = playerInput.actions.FindAction("Right Shoot");

            _leftSwap = playerInput.actions.FindAction("Left Swap");
            _rightSwap = playerInput.actions.FindAction("Right Swap");

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
                }
                else if (_openMenu)
                {
                    gameState.isPaused = true;
                }
            }
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct

            characterInputs.MoveAxisForward = _moveAction.ReadValue<Vector2>().y;
            characterInputs.MoveAxisRight = _moveAction.ReadValue<Vector2>().x;
            characterInputs.CameraRotation = CharacterCamera.transform.rotation;
            characterInputs.Dash = _dash.ReadValue<float>() == 1;
            characterInputs.LeftShoot = _leftShoot.ReadValue<float>() == 1;
            characterInputs.RightShoot = _rightShoot.ReadValue<float>() == 1;
            characterInputs.LeftSwap = _leftSwap.ReadValue<float>() == 1;
            characterInputs.RightSwap= _rightSwap.ReadValue<float>() == 1;

            // Apply inputs to character
            Character.SetInputs(ref characterInputs);
        }
    }
}