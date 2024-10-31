using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private Transform orientation;
    [SerializeField] private float drag;
    [SerializeField] private float maxJumpForce;
    [SerializeField] private float forceIncreaseRate;
    
    [SerializeField, Range(0.01f, 1)] private float airControl;
    
        
    [Header("Ground Check")] 
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask Ground;

    private bool _isReadyToJump;
    private bool _isGrounded;
    private bool _isJumping;
    private float _jumpForce;
    private Vector2 _userInput;
    private Vector3 _moveDirection;
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    private const float MOVEDIRECTIONMULTIPLIER = 50f;
    private const float JUMPFORCEMULTIPLIER = 50f;
    private bool _previouslyGrounded;

    
    
    
    void Start()
    {
        ResetJump();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponentInChildren<CapsuleCollider>();
        _rigidbody.freezeRotation = true;
    }

    private void Update()
    {
        Vector3 raycastOrigin = transform.position + Vector3.down * (playerHeight / 2);
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, (playerHeight / 2) + 0.1f, Ground);
        Debug.DrawRay(raycastOrigin, Vector3.down * 0.1f, Color.red);
        SpeedControl();
        JumpCheck();
        
        if (_isGrounded != _previouslyGrounded) 
        {
            _rigidbody.drag = _isGrounded ? drag : 0;
            _previouslyGrounded = _isGrounded;
        }
    }
    

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        //Calculate Movement Direction
        var currentMoveSpeed = moveSpeed;
        _moveDirection = orientation.forward * _userInput.y + orientation.right * _userInput.x;
        if (_isJumping && _isGrounded)
        {
            currentMoveSpeed = crouchSpeed;
        }

        if (!_isGrounded)
        {
            _rigidbody.AddForce(_moveDirection * ((currentMoveSpeed * MOVEDIRECTIONMULTIPLIER) * airControl), ForceMode.Force);   

        }
        else
        {
            _rigidbody.AddForce(_moveDirection * (currentMoveSpeed * MOVEDIRECTIONMULTIPLIER), ForceMode.Force);   
        }
    }


    private void SpeedControl()
    {
        var flatVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
        if (flatVelocity.magnitude > moveSpeed)
        {
            var limitedVelocity = flatVelocity.normalized * moveSpeed;
            _rigidbody.velocity = new Vector3(limitedVelocity.x, _rigidbody.velocity.y, limitedVelocity.z);
        }
    }
    

    private void ResetJump()
    {
        _isReadyToJump = true;
    }
    
    public void GetInput(InputAction.CallbackContext context)
    {
        _userInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!_isGrounded || !_isReadyToJump) return;
        
        if (context.performed)
        {   
            _jumpForce = 4f;
            _isJumping = true;
        }

        if (context.canceled)
        {
            PerformJump(_jumpForce);
            _isJumping = false;
            _jumpForce = 0f;
            _isReadyToJump = false;
        }
        
    }
    private void PerformJump(float jumpForce)
    {
        jumpForce = Math.Clamp(jumpForce, 4f, maxJumpForce);
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
        _rigidbody.AddForce(transform.up * (jumpForce * JUMPFORCEMULTIPLIER), ForceMode.Impulse);
        ResetCollider();
    }
    
    private void JumpCheck()
    {
        if (_isGrounded && !_isJumping) _isReadyToJump = true;
        if (!_isJumping) return;
        
        _collider.height = 1.15f;
        _jumpForce += forceIncreaseRate * Time.deltaTime;
        _jumpForce = Math.Clamp(_jumpForce, 0f, maxJumpForce);
    }

    private void ResetCollider()
    {
        _collider.height = 1.85f;
    }
}
