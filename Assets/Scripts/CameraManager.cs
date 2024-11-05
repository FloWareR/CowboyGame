using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Transform lookAt; 
    [SerializeField] private Transform playerTransform;

    [SerializeField] private LayerMask collisionLayers;

    [Header("Camera adjustments")] 
    [SerializeField] private float cameraFollowSpeed;
    [SerializeField] private float mouseSensitivityX;
    [SerializeField] private float mouseSensitivityY;
    [SerializeField] private float joystickSensitivityX;
    [SerializeField] private float joystickSensitivityY;
    [SerializeField] private float minimumPivotAngle;
    [SerializeField] private float maximumPivotAngle;
    
    [Header("Camera collision parameters")] 
    [SerializeField] private float cameraCollisionRadius;
    [SerializeField] private float cameraCollisionOffset;
    [SerializeField] private float minimumCollisionOffset;

    public bool isJoystickInput;

    private float _defaultPosition;
    private float _lookAngle;
    private float _pivotAngle;
    private Transform _targetTransform;
    private Transform _cameraTransform;
    private Vector3 _cameraFollowVelocity = Vector3.zero;
    private Vector3 _cameraVectorPosition;

    private InputManager _inputManager;

    private void Awake()
    {
        if (Camera.main != null) _cameraTransform = Camera.main.transform;
        _defaultPosition = _cameraTransform.localPosition.z;
        _targetTransform = FindObjectOfType<PlayerManager>().transform;
        _inputManager = FindObjectOfType<InputManager>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        HandleAllCameraMovement();
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollision();
    }
    
    private void FollowTarget()
    {
        var targetPosition = Vector3.SmoothDamp(transform.position, _targetTransform.position, ref _cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        var sensitivityX = isJoystickInput ? joystickSensitivityX : mouseSensitivityX;
        var sensitivityY = isJoystickInput ? joystickSensitivityY : mouseSensitivityY;

        _lookAngle += _inputManager.CameraInputX * sensitivityX;
        _pivotAngle -= _inputManager.CameraInputY * sensitivityY;
        _pivotAngle = Mathf.Clamp(_pivotAngle, minimumPivotAngle, maximumPivotAngle);

        Vector3 rotation = Vector3.zero;
        rotation.y = _lookAngle;
        var targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = _pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void HandleCameraCollision()
    {
        var targetPosition = _defaultPosition;
        RaycastHit hit;
        Vector3 direction = _cameraTransform.position - cameraPivot.position;
        direction.Normalize();
        
        if (Physics.SphereCast(cameraPivot.position, cameraCollisionRadius, direction, out hit,
                Mathf.Abs(targetPosition), collisionLayers))
        {
            var distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition = -(distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition = targetPosition - minimumCollisionOffset;
        }

        _cameraVectorPosition.z = Mathf.Lerp(_cameraTransform.localPosition.z, targetPosition, 0.2f);
        _cameraTransform.localPosition = _cameraVectorPosition;
    }

}
