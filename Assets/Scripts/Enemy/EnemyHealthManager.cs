using System.Collections;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private float blinkIntensity;
    [SerializeField] private float blinkDuration;

    private UIHealthBar _healthBar;
    private AiLocomotion _aiLocomotion;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private AiAgent _agent;

    private Color _originalColor;
    private Coroutine _blinkCoroutine;
    private MaterialPropertyBlock _propertyBlock;

    void Start()
    {
        SetAllReferences();
        SetAllParameters();
        #region Add HitBox Component to all rigidbodies in children of gameobject
        var rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidbody in rigidbodies)
        {
            EnemyHitBox enemyHitBox = rigidbody.gameObject.AddComponent<EnemyHitBox>();
            enemyHitBox.enemyHealthManager = this;
        }
        
        #endregion
    }

    public void TakeDamage(float damage, Vector3 direction)
    {
        currentHealth -= damage;
        _healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        
        if (currentHealth <= 0) Die(direction);

        if (_blinkCoroutine != null) StopCoroutine(_blinkCoroutine);
        
        _blinkCoroutine = StartCoroutine(BlinkHDRRed());
    }

    private void Die(Vector3 direction)
    {
        var deathState = _agent.StateMachine.GetState(AiStateID.Death) as AiDeathState;
        deathState.direction = direction;
        _agent.StateMachine.ChangeState(AiStateID.Death);
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







    private void SetAllReferences()
    {
        _agent = GetComponent<AiAgent>();
        _healthBar = GetComponentInChildren<UIHealthBar>();
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private void SetAllParameters()
    {
        currentHealth = maxHealth;
    }
}