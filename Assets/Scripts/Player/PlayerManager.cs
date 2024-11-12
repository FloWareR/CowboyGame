using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    private PlayerLocomotion _playerLocomotion;
    private InputManager _inputManager;
    private CameraManager _cameraManager;
    private Animator _animator;
    private ParticleManager _particleManager;
    
    [SerializeField] private GameObject playerGraphics;
    [SerializeField] private GameObject playerRagdoll;
    [SerializeField] private string deathParticleName = "playerDeath";
    [SerializeField] private float ragdollSettleTime;

    public bool isAlive = true;
    public bool isInteracting;
    public bool isHeavyAttacking;
    public bool isHeavyPunching;

    private static readonly int IsInteracting = Animator.StringToHash("isInteracting");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
    private static readonly int IsHeavyAttacking = Animator.StringToHash("isHeavyAttacking");

    // Define the event and delegate
    public delegate void ObjectiveFailedEventHandler();
    public event ObjectiveFailedEventHandler OnObjectiveFailed;

    private void Awake()
    {
        playerRagdoll.SetActive(false);
        _inputManager = GetComponent<InputManager>();
        _playerLocomotion = GetComponent<PlayerLocomotion>();
        _cameraManager = FindObjectOfType<CameraManager>();
        _animator = GetComponentInChildren<Animator>();
        _particleManager = FindObjectOfType<ParticleManager>();
    }

    private void Update()
    {
        ReloadCurrentAndAdditiveScene();
        _inputManager.HandleAllInput();
        if (!isAlive) return;
        _inputManager.HandleActions();
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;
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
        }
    }

    public void OnDeath()
    {
        isAlive = false;
        gameObject.SetActive(true);
        playerGraphics.SetActive(false);
        playerRagdoll.SetActive(true);

        OnObjectiveFailed?.Invoke();
    }
}
