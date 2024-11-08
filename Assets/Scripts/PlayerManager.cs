using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private PlayerLocomotion _playerLocomotion;
    private InputManager _inputManager;
    private CameraManager _cameraManager;
    private Animator _animator;

    public bool isInteracting;
    public bool isHeavyAttacking;
    public bool isHeavyPunching;
    
    private static readonly int IsInteracting = Animator.StringToHash("isInteracting");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
    private static readonly int IsHeavyAttacking = Animator.StringToHash("isHeavyAttacking");

    private void Awake()
    {
        _inputManager = GetComponent<InputManager>();
        _playerLocomotion = GetComponent<PlayerLocomotion>();
        _cameraManager = FindObjectOfType<CameraManager>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        ReloadCurrentAndAdditiveScene();
        _inputManager.HandleAllInput();
    }

    private void FixedUpdate()
    {
        _playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        _cameraManager.HandleAllCameraMovement();
        isInteracting = _animator.GetBool(IsInteracting);
        isHeavyAttacking = _animator.GetBool(IsHeavyAttacking);
        _animator.SetBool(IsGrounded, _playerLocomotion.isGrounded);
    }
    
    private void ReloadCurrentAndAdditiveScene()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Get the current active scene name
            string currentSceneName = SceneManager.GetActiveScene().name;
        
            // Reload the current scene
            SceneManager.LoadScene(currentSceneName);
        
            // Load the additional scene additively
            SceneManager.LoadScene("Slavica Free", LoadSceneMode.Additive);
        }
    }

}
