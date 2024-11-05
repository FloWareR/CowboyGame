using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private ParticleSystem heavyAttack;
    [SerializeField] private ParticleSystem additionalFX;

    [SerializeField] private Transform attackOrigin;

    private AnimatorManager _animatorManager;
    private PlayerLocomotion _playerLocomotion;
    private PlayerManager _playerManager;
    private ParticleSystem _spawnedHeavyAttack;
    private ParticleSystem _spawnedAdditionalFx;


    private void Awake()
    {
        _animatorManager = GetComponent<AnimatorManager>();
        _playerLocomotion = GetComponent<PlayerLocomotion>();
        _playerManager = GetComponent<PlayerManager>();
    }

    public void HandlePrimaryAction()
    {
        _playerLocomotion.StopAllMovement();
        _animatorManager.PlayTargetAnimation("SayainAttack", true);
        
        
        _spawnedAdditionalFx = Instantiate(additionalFX, transform.position, transform.rotation, attackOrigin);
        _spawnedAdditionalFx.Play();
        
        _playerLocomotion.RotateAtLookAt();

        StartCoroutine(DespawnHeavyAttackWhenDone());
    }
    
    private System.Collections.IEnumerator DespawnHeavyAttackWhenDone()
    {
        do
        {
            yield return null;
        } while (_playerManager.isInteracting);
        
        if (_spawnedHeavyAttack != null)
        {
            Destroy(_spawnedHeavyAttack.gameObject, _spawnedHeavyAttack.main.startLifetime.constantMax);
            _spawnedHeavyAttack.Stop();
        }
    }
    
    public void TriggerHeavyAttackEffect()
    {
        _spawnedHeavyAttack = Instantiate(heavyAttack, attackOrigin.position, attackOrigin.rotation, attackOrigin);
        _spawnedHeavyAttack.Play();
    }

    public void DestroyHeavyAttackEffect()
    {
        Destroy(_spawnedHeavyAttack.gameObject, _spawnedHeavyAttack.main.startLifetime.constantMax);
        _spawnedHeavyAttack.Stop();
    }
}