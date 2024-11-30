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
        private WeaponManager WeaponManager;
        public GameObject mouseInGameUI;

        [Header("Input Actions")]
        PlayerInput playerInput;

        InputAction _moveAction;
        InputAction _dash;
        InputAction _leftShoot;
        InputAction _rightShoot;
        InputAction _rightHold;
        InputAction _reload;

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
            WeaponManager = FindObjectOfType<WeaponManager>();
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

            _rightHold = playerInput.actions.FindAction("Right Hold");

            _reload = playerInput.actions.FindAction("Reload");

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
            PlayerControllerInputs controllerInputs = new PlayerControllerInputs();
            PlayerCombatInputs combatInputs = new PlayerCombatInputs();

            // Build the CharacterInputs struct
            controllerInputs.MoveAxisForward = _moveAction.ReadValue<Vector2>().y;
            controllerInputs.MoveAxisRight = _moveAction.ReadValue<Vector2>().x;
            controllerInputs.CameraRotation = CharacterCamera.transform.rotation;
            controllerInputs.Dash = _dash.ReadValue<float>() == 1;

            combatInputs.LeftShoot = _leftShoot.ReadValue<float>() == 1;

            combatInputs.RightShoot = _rightShoot.phase == InputActionPhase.Started;

            // Check if the right shoot action was released
            combatInputs.RightHold = _rightShoot.phase == InputActionPhase.Performed;
            combatInputs.RightRelease = _rightShoot.phase == InputActionPhase.Waiting;

            controllerInputs.mousePos = CharacterCamera.mouseFollowPoint;
            combatInputs.mousePos = CharacterCamera.mouseFollowPoint;
            combatInputs.Reload = _reload.phase == InputActionPhase.Performed;

            mouseInGameUI.transform.position = CharacterCamera.mouseFollowPoint;

            //Debug.Log(_reload.phase);

            // Apply inputs to character
            Character.SetInputs(ref controllerInputs);
            WeaponManager.SetInputs(ref combatInputs);
        }
    }
}