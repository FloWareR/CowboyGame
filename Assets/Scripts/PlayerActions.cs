using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private ParticleSystem heavyAttack;
    [SerializeField] private ParticleSystem additionalFX;
    
    [SerializeField] private Transform graphicsOrigin;


    private AnimatorManager _animatorManager;
    private PlayerManager _playerManager;
    private PlayerLocomotion _playerLocomotion;
    
    private ParticleSystem _spawnedHeavyAttack;
    private ParticleSystem _spawnedAdditionalFx;


    private void Awake()
    {
        _spawnedAdditionalFx = Instantiate(additionalFX, transform.position, transform.rotation, graphicsOrigin);
        _spawnedAdditionalFx.Stop();
        _animatorManager = GetComponent<AnimatorManager>();
        _playerLocomotion = GetComponent<PlayerLocomotion>();
        _playerManager = GetComponent<PlayerManager>();
    }

    public void HandlePrimaryAction()
    {
        Debug.Log("SHOOT");
    }

    public void HandleUltimateAction()
    {
        if (_playerManager.isInteracting || _playerManager.isHeavyAttacking) return;
        _playerLocomotion.RotateAtLookAt();
        _playerLocomotion.StopAllMovement();
        _animatorManager.PlayTargetAnimation("SayainAttack", true);
        _spawnedAdditionalFx.Play();      
    }
}