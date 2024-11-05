using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InputManager : MonoBehaviour
{
    [NonSerialized] public float VerticalInput;
    [NonSerialized] public float HorizontalInput;
    [NonSerialized] public float CameraInputX;
    [NonSerialized] public float CameraInputY;
    [NonSerialized] public float MoveAmount;
    [NonSerialized] public bool SprintInput;
    [NonSerialized] public bool JumpInput;

        
    private IMC_Default _playerControls;
    private AnimatorManager _animatorManager;
    private PlayerLocomotion _playerLocomotion;
    private CameraManager _cameraManager;
    
    private bool _isJoystickInput;
    
    public Vector2 movementInput;
    public Vector2 cameraInput;


    private void Awake()
    {
        _animatorManager = GetComponent<AnimatorManager>();
        _playerLocomotion = GetComponent<PlayerLocomotion>();
        _cameraManager = FindObjectOfType<CameraManager>();
    }

    private void OnEnable()
    {
        if (_playerControls == null)
        {
            _playerControls = new IMC_Default();
            _playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            _playerControls.PlayerMovement.Camera.performed += HandleCameraInput;

            _playerControls.PlayerActions.Sprint.performed += i => SprintInput = true;
            _playerControls.PlayerActions.Sprint.canceled += i => SprintInput = false;

            _playerControls.PlayerActions.Jump.performed += i => JumpInput = true;
            _playerControls.PlayerActions.Jump.canceled += i => JumpInput = false;
        }
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    public void HandleAllInput()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpInput();
    }
    
    private void HandleCameraInput(InputAction.CallbackContext context)
    {
        cameraInput = context.ReadValue<Vector2>();
        _isJoystickInput = context.control.device is UnityEngine.InputSystem.Gamepad;
    }
    
    private void HandleMovementInput()
    {
        VerticalInput = movementInput.y;
        HorizontalInput = movementInput.x;

        CameraInputY = cameraInput.y;
        CameraInputX = cameraInput.x;

        _cameraManager.isJoystickInput = _isJoystickInput;
        
        MoveAmount = Mathf.Clamp01(Mathf.Abs(HorizontalInput) + Mathf.Abs(VerticalInput));
        _animatorManager.UpdateAnimatorValues(0f, MoveAmount, _playerLocomotion.isSprinting);
    }
    
    private void HandleSprintingInput()
    {
        if (SprintInput && MoveAmount > 0.5f)
        {
            _playerLocomotion.isSprinting = true;
        }
        else
        {
            _playerLocomotion.isSprinting = false;
        }
    }

    private void HandleJumpInput()
    {
        if (JumpInput)
        {
            JumpInput = false;
            _playerLocomotion.HandleJumping();
        }
    }
    
}
