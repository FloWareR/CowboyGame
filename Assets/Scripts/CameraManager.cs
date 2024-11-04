using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Transform cameraPivot;
   
    [SerializeField] private LayerMask collisionLayers;

    [Header("Camera adjustments")] 
    [SerializeField] private float cameraFollowSpeed;
    [SerializeField] private float cameraLookSpeed;
    [SerializeField] private float cameraPivotSpeed;
    [SerializeField] private float minimumPivotAngle;
    [SerializeField] private float maximumPivotAngle;
    
    [Header("Camera collision parameters")] 
    [SerializeField] private float cameraCollisionRadius;
    [SerializeField] private float cameraCollisionOffset;
    [SerializeField] private float minimunCollisionOffset;
    
    
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

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollision();
    }
    
    private void FollowTarget()
    {
        var targetPosition = Vector3.SmoothDamp(transform.position, _targetTransform.position, ref _cameraFollowVelocity, cameraFollowSpeed );
        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        _lookAngle = _lookAngle + (_inputManager.CameraInputX * cameraLookSpeed);
        _pivotAngle = _pivotAngle - (_inputManager.CameraInputY * cameraPivotSpeed);
        _pivotAngle = Mathf.Clamp(_pivotAngle, minimumPivotAngle, maximumPivotAngle);
        
        var rotation = Vector3.zero;
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
        if (Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit,
                Mathf.Abs(targetPosition), collisionLayers))
        {
            var distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition =- (distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minimunCollisionOffset)
        {
            targetPosition = targetPosition - minimunCollisionOffset;
        }

        _cameraVectorPosition.z = Mathf.Lerp(_cameraTransform.localPosition.z, targetPosition, 0.2f);
        _cameraTransform.localPosition = _cameraVectorPosition;
    }
}
