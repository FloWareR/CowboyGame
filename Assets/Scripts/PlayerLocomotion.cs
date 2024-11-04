using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraObject;
    
    [Header("Movement Speed")]
    [Tooltip("Analog movement speed")] [SerializeField] private float walkSpeed;
    [Tooltip("Regular movement speed")] [SerializeField] private float runSpeed;
    [Tooltip("Boost movement speed")] [SerializeField] private float sprintSpeed;
    [SerializeField] private float rotationSpeed;
    
    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;
    [SerializeField] private LayerMask groundLayer;

    [Header("Falling")] 
    [SerializeField] private float inAirTimer;
    [SerializeField] private float leapingVelocity;
    [SerializeField] private float fallingVelocity;
    [SerializeField] private float rayCastHeightOffset;
    
    [Header("Jumping")]
    [SerializeField] [Tooltip("Must be negative")] private float gravityIntensity;
    [SerializeField] private float jumpHeight;
    
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
        if(isJumping) return;
            
        _moveDirection = (cameraObject.forward * _inputManager.VerticalInput) + (cameraObject.right * _inputManager.HorizontalInput);
        _moveDirection.Normalize();
        _moveDirection.y = 0f;
        
        if (isSprinting)
        {
            _moveDirection = _moveDirection * sprintSpeed;
        }
        else
        {
            if (_inputManager.MoveAmount >= 0.5f)
            {
                _moveDirection = _moveDirection * runSpeed;
            }
            else
            {
                _moveDirection = _moveDirection * walkSpeed;
            }
        }
        

        
        _rigidbody.velocity = _moveDirection;
    }

    private void HandleRotation()
    {
        if(isJumping) return;
        var targetDirection = (cameraObject.forward * _inputManager.VerticalInput) +
                              (cameraObject.right * _inputManager.HorizontalInput);
        targetDirection.Normalize();
        targetDirection.y = 0f;
        if (targetDirection == Vector3.zero) targetDirection = transform.forward;
        var targetRotation = Quaternion.LookRotation(targetDirection);
        var playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = playerRotation;
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        var rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;
        
        if (!isGrounded && !isJumping)
        {
            if (!_playerManager.isInteracting)
            {
                _animatorManager.PlayTargetAnimation("Falling", true);
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            _rigidbody.AddForce(transform.forward * leapingVelocity);
            _rigidbody.AddForce(-Vector3.up * (fallingVelocity * inAirTimer));
        }

        if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit,0.5f, groundLayer))
        {   
            if (!isGrounded && _playerManager.isInteracting)
            {
                _animatorManager.PlayTargetAnimation("Landing", true);
            }

            inAirTimer = 0f;
            isGrounded = true;
          
        }
        else
        {
            isGrounded = false;
        }
        
    }

    public void HandleJumping()
    {
        if (isGrounded)
        {
            _animatorManager.animator.SetBool(IsJumping, true);
            _animatorManager.PlayTargetAnimation("Jumping", false);

            var jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            var playerVelocity = _moveDirection;
            playerVelocity.y = jumpingVelocity;
            _rigidbody.velocity = playerVelocity;
        }
    }
}
