using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerObj;
    [SerializeField] private float rotationSpeed;
    
    [SerializeField] private Transform combatLookAt;
    [SerializeField] private GameObject basicCam;
    [SerializeField] private GameObject combatCam;
    [SerializeField] private CameraStyle _currentStyle = CameraStyle.Combat;
    
    private Vector2 _userInput;
    public enum CameraStyle 
    {
        Basic,
        Combat
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCameraStyle(CameraStyle.Basic);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCameraStyle(CameraStyle.Combat);

        var viewDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDirection.normalized;
        
        switch (_currentStyle)
        {
            case CameraStyle.Basic:
                var inputDirection = (orientation.forward * _userInput.y) + (orientation.right * _userInput.x);
                if (inputDirection != Vector3.zero)
                {
                    playerObj.forward = Vector3.Slerp(playerObj.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
                }
                
                break;
            
            case CameraStyle.Combat:
                var combatLookAtDirection = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
                orientation.forward = combatLookAtDirection.normalized;
                playerObj.forward = combatLookAtDirection.normalized;
                break;
        }

    }
 
    public void GetInput(InputAction.CallbackContext context)
    {
        _userInput = context.ReadValue<Vector2>();
    }

    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        combatCam.SetActive(false);
        basicCam.SetActive(false);
        switch (newStyle)
        {
            case CameraStyle.Basic:
                basicCam.SetActive(true);
                break;
            case CameraStyle.Combat:
                combatCam.SetActive(true);
                break;
        }

        _currentStyle = newStyle;
    }
}
