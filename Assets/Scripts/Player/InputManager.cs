using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [NonSerialized] public float VerticalInput;
    [NonSerialized] public float HorizontalInput;
    [NonSerialized] public float CameraInputX;
    [NonSerialized] public float CameraInputY;
    [NonSerialized] public float MoveAmount;
    [NonSerialized] public bool SprintInput;
    [NonSerialized] public bool JumpInput;
    [NonSerialized] public bool PrimaryAttackInput;
    [NonSerialized] public bool UltimateAttackInput;
    [NonSerialized] public bool SecondaryAttackInput;
    [NonSerialized] public bool MeleeAttackInput;


    
    public Vector2 movementInput;
    public Vector2 cameraInput;

    private bool _isJoystickInput;
    
    private IMC_Default _playerControls;
    private AnimatorManager _animatorManager;
    private PlayerLocomotion _playerLocomotion;
    private CameraManager _cameraManager;
    private PlayerActions _playerActions;
    private PlayerManager _playerManager;
    


    private void Awake()
    {
        _animatorManager = GetComponent<AnimatorManager>();
        _playerLocomotion = GetComponent<PlayerLocomotion>();
        _cameraManager = FindObjectOfType<CameraManager>();
        _playerActions = GetComponent<PlayerActions>();
        _playerManager = GetComponent<PlayerManager>();
    }

    private void OnEnable()
    {
        if (_playerControls == null)
        {
            _playerControls = new IMC_Default();
            _playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            _playerControls.PlayerMovement.Camera.performed += InputOriginChecker;

            _playerControls.PlayerActions.Sprint.performed += i => SprintInput = true;
            _playerControls.PlayerActions.Sprint.canceled += i => SprintInput = false;

            _playerControls.PlayerActions.Jump.performed += i => JumpInput = true;
            _playerControls.PlayerActions.Jump.canceled += i => JumpInput = false;

            _playerControls.PlayerActions.UltimateAttack.performed += i => UltimateAttackInput = true;
            _playerControls.PlayerActions.UltimateAttack.canceled += i => UltimateAttackInput = false;
            
            _playerControls.PlayerActions.PrimaryAttack.performed += i => PrimaryAttackInput = true;
            _playerControls.PlayerActions.PrimaryAttack.canceled += i => PrimaryAttackInput = false;
            
            _playerControls.PlayerActions.SecondaryAttack.performed += i => SecondaryAttackInput = true;
            _playerControls.PlayerActions.SecondaryAttack.canceled += i => SecondaryAttackInput = false;            
            
            _playerControls.PlayerActions.MeleeAttack.performed += i => MeleeAttackInput = true;
            _playerControls.PlayerActions.MeleeAttack.canceled += i => MeleeAttackInput = false;

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
    }

    public void HandleActions()
    {
        HandleSprintingInput();
        HandleJumpInput();
        HandlePrimaryAttack();
        HandleUltimateAttack();
        HandleSecondaryAttack();
        HandleMeleeAttack();
    }
    
    private void InputOriginChecker(InputAction.CallbackContext context)
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
        _animatorManager.UpdateAnimatorValues(HorizontalInput, VerticalInput, _playerLocomotion.isSprinting);
    }
    
    private void HandleSprintingInput()
    {
        if (SprintInput && VerticalInput > 0.5f && Mathf.Abs(HorizontalInput) < 0.25f  && _playerLocomotion.isGrounded)
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

    private void HandlePrimaryAttack()
    {
        if (PrimaryAttackInput)
        {
            PrimaryAttackInput = false;
            _playerActions.HandlePrimaryAction();
        }
    }
    
    private void HandleUltimateAttack()
    {
        if (UltimateAttackInput)
        {
            PrimaryAttackInput = false;
            _playerActions.HandleUltimateAction();
        }
    }
    
    
    private void HandleSecondaryAttack()
    {
        if (SecondaryAttackInput)
        {
            _playerActions.HandleSecondaryAction();
            _cameraManager.isZoomedIn = true;
        }
        else
        {
            _cameraManager.isZoomedIn = false;
        }
    }

    private void HandleMeleeAttack()
    {
        if (MeleeAttackInput)
        {
            MeleeAttackInput = false;
            _playerActions.HandleMeleeAction();
        }
    }
    
}
