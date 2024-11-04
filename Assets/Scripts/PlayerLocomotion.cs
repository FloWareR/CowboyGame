using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraObject;

    [Header("Movement Speed")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float airControlMultiplier;
    [SerializeField] private float groundDrag; 

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;
    [SerializeField] private LayerMask groundLayer;

    [Header("Falling")]
    [SerializeField] private float rayCastHeightOffset;
    [SerializeField] private float customGravityMultiplier;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;

    private Vector3 _moveDirection;
    private InputManager _inputManager;
    private Rigidbody _rigidbody;
    private PlayerManager _playerManager;
    private AnimatorManager _animatorManager;
    private static readonly int IsJumping = Animator.StringToHash("isJumping");

    private void Awake()
    {
        _inputManager = GetComponent<InputManager>();
        _rigidbody = GetComponent<Rigidbody>();
        _playerManager = GetComponent<PlayerManager>();
        _animatorManager = GetComponent<AnimatorManager>();

        if (Camera.main != null) cameraObject = Camera.main.transform;

        _rigidbody.drag = groundDrag; 
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();
        if (_playerManager.isInteracting) return;
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        _moveDirection = (cameraObject.forward * _inputManager.VerticalInput) + (cameraObject.right * _inputManager.HorizontalInput);
        _moveDirection.Normalize();
        _moveDirection.y = 0f;

        var targetSpeed = isSprinting && isGrounded ? sprintSpeed :
                          _inputManager.MoveAmount >= 0.5f ? runSpeed : walkSpeed;

        var moveForce = _moveDirection * (isGrounded ? targetSpeed : targetSpeed * airControlMultiplier);
        _rigidbody.AddForce(new Vector3(moveForce.x, 0, moveForce.z), ForceMode.Acceleration);

        CapSpeed(targetSpeed);
    }

    private void CapSpeed(float maxSpeed)
    {
        var horizontalVelocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            var clampedVelocity = horizontalVelocity.normalized * maxSpeed;
            _rigidbody.velocity = new Vector3(clampedVelocity.x, _rigidbody.velocity.y, clampedVelocity.z);
        }
    }

    private void HandleRotation()
    {
        var targetDirection = (cameraObject.forward * _inputManager.VerticalInput) + (cameraObject.right * _inputManager.HorizontalInput);
        targetDirection.Normalize();
        targetDirection.y = 0f;

        if (targetDirection == Vector3.zero) targetDirection = transform.forward;

        var targetRotation = Quaternion.LookRotation(targetDirection);
        var trueRotation = isGrounded ? rotationSpeed : rotationSpeed * airControlMultiplier;
        var playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, trueRotation * Time.deltaTime);
        transform.rotation = playerRotation;
    }
    
    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        var rayCastOrigin = transform.position + Vector3.up * rayCastHeightOffset;

        if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, 0.5f, groundLayer))
        {
            if (!isGrounded && _playerManager.isInteracting)
                _animatorManager.PlayTargetAnimation("Landing", true);

            isGrounded = true;
            isJumping = false;
            _rigidbody.drag = groundDrag; 
        }
        else if (isGrounded)
        {
            isGrounded = false;
            _animatorManager.PlayTargetAnimation("Falling", false);
            _rigidbody.drag = 0f; 
        }

        if (!isGrounded)
        {
            _rigidbody.AddForce(Physics.gravity * (customGravityMultiplier - 1), ForceMode.Acceleration);
        }
    }


    public void HandleJumping()
    {
        if (isGrounded)
        {
            _animatorManager.animator.SetBool(IsJumping, true);
            _animatorManager.PlayTargetAnimation("Jumping", false);

            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            isGrounded = false;
            isJumping = true;
        }
    }
}
