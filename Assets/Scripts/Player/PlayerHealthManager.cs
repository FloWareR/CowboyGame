using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private float blinkIntensity;
    [SerializeField] private float blinkDuration;

    private SkinnedMeshRenderer[] _skinnedMeshRenderers;

    private Color _originalColor;
    private Coroutine _blinkCoroutine;
    private MaterialPropertyBlock _propertyBlock;
    private PlayerManager _playerManager;
    
    void Start()
    {
        SetAllReferences();
        SetAllParameters();
    }
    
    public void TakeDamage(float damage, Vector3 direction)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0) Die(direction);

        if (_blinkCoroutine != null) StopCoroutine(_blinkCoroutine);
        foreach (var mesh in _skinnedMeshRenderers)
        {
            _blinkCoroutine = StartCoroutine(BlinkHDRRed(mesh));

        }
    }
    
    private IEnumerator BlinkHDRRed(SkinnedMeshRenderer _skinnedMeshRenderer)
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

            yield return new WaitForSeconds(0.025f);
        }

        _propertyBlock.SetFloat("_BlinkIntensity", 1);
        _skinnedMeshRenderer.SetPropertyBlock(_propertyBlock);
    }

    
    
    private void Die(Vector3 direction)
    {
        _playerManager.OnDeath();
    }
    
    
    private void SetAllParameters()
    {
        currentHealth = maxHealth;
    }

    private void SetAllReferences()
    {
        _playerManager = GetComponent<PlayerManager>();
        _skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
}
