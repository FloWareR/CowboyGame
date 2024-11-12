using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;

    public float meleeDamage;
    public float ultimateDamage;
    public float fireRate; 

    private float nextFireTime = 0f;

    private AnimatorManager _animatorManager;
    private PlayerManager _playerManager;
    private PlayerLocomotion _playerLocomotion;
    private ParticleManager _particleManager;

    private void Awake()
    {
        _animatorManager = GetComponent<AnimatorManager>();
        _playerLocomotion = GetComponent<PlayerLocomotion>();
        _playerManager = GetComponent<PlayerManager>();
        _particleManager = FindObjectOfType<ParticleManager>();
    }

    public void HandlePrimaryAction()
    {
        // Check if player is sprinting or interacting, or if cooldown is active
        if (_playerLocomotion.isSprinting || _playerManager.isInteracting) return;

        // Check if enough time has passed to allow another shot
        if (Time.time < nextFireTime) return;

        // Shoot the projectile
        _particleManager.SpawnTemporaryParticle("magicProjectile", attackPoint.position, attackPoint.rotation);
        
        // Set the next fire time based on the fire rate
        nextFireTime = Time.time + fireRate;
    }

    public void HandleUltimateAction()
    {
        if (_playerManager.isInteracting || _playerManager.isHeavyAttacking) return;
        _playerLocomotion.RotateAtLookAt();
        _playerLocomotion.StopAllMovement();
        _animatorManager.PlayTargetAnimation("SayainAttack", true);
        _particleManager.ToggleParticleSystem("heavyAttackAura", true);
    }

    public void HandleSecondaryAction()
    {
        if (_playerManager.isInteracting) return;
    }

    public void HandleMeleeAction()
    {
        if (_playerManager.isInteracting) return;
        _animatorManager.PlayTargetAnimation("HeavyPunching", true);
    }
}
