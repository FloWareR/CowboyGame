using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private float blinkIntensity;
    [SerializeField] private float blinkDuration;
    [SerializeField] private float dieForce;
    
    private UIHealthBar _healthBar;
    private AiLocomotion _aiLocomotion;
    private float blinkTimer;

    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private Ragdoll _ragdoll;
    void Start()
    {
        _aiLocomotion = GetComponent<AiLocomotion>();
        _healthBar = GetComponentInChildren<UIHealthBar>();
        currentHealth = maxHealth;
        _ragdoll = GetComponent<Ragdoll>();
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        var rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidbody in rigidbodies)
        {
            HitBox hitBox = rigidbody.gameObject.AddComponent<HitBox>();
            hitBox.healthManager = this;
        }

    }


    public void TakeDamage(float damage, Vector3 direction)
    {
        currentHealth -= damage;
        _healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        if (currentHealth <= 0)
        {
            Die(direction);
        }

        blinkTimer = blinkDuration;
    }

    private void Die(Vector3 direction)
    {
        Debug.Log(direction);
        _aiLocomotion.DisableMovement();
        _ragdoll.ActivateRagdoll();
        direction.y = 1;
        _ragdoll.ApplyForce(direction * dieForce);
        _healthBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        blinkTimer -= Time.deltaTime;
        var lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        var intensity = lerp * blinkIntensity + 1f;
        _skinnedMeshRenderer.material.color = Color.white * intensity;
    }
}

