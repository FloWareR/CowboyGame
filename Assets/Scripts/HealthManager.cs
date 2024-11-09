using System.Collections;
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
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private Ragdoll _ragdoll;

    private Color _originalColor;
    private Coroutine _blinkCoroutine;
    private MaterialPropertyBlock _propertyBlock;

    void Start()
    {
        _aiLocomotion = GetComponent<AiLocomotion>();
        _healthBar = GetComponentInChildren<UIHealthBar>();
        _ragdoll = GetComponent<Ragdoll>();
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        
        currentHealth = maxHealth;

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

        if (_blinkCoroutine != null)
        {
            StopCoroutine(_blinkCoroutine);
        }
        _blinkCoroutine = StartCoroutine(BlinkHDRRed());
    }

    private void Die(Vector3 direction)
    {
        _aiLocomotion.DisableMovement();
        _ragdoll.ActivateRagdoll();
        direction.y = 1; // Push upwards a bit to simulate ragdoll force
        _ragdoll.ApplyForce(direction * dieForce);
        _healthBar.gameObject.SetActive(false);
    }
    
    private IEnumerator BlinkHDRRed()
    {
        _propertyBlock = new MaterialPropertyBlock();
        _skinnedMeshRenderer.GetPropertyBlock(_propertyBlock);

        float elapsedTime = 0f;

        for (int i = 0; i < 2; i++)
        {
            while (elapsedTime < blinkDuration)
            {
                _propertyBlock.SetFloat("_BlinkIntensity", Mathf.Lerp(blinkIntensity, 0, elapsedTime / blinkDuration));
                _skinnedMeshRenderer.SetPropertyBlock(_propertyBlock);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;

            yield return new WaitForSeconds(0.05f);
        }

        _propertyBlock.SetFloat("_BlinkIntensity", 1);
        _skinnedMeshRenderer.SetPropertyBlock(_propertyBlock);
    }
}