using System.Collections;
using UnityEngine;

public class ArtifactHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private GameOver gameOver;

    private UIHealthBar _healthBar;
    private PlayerManager _playerManager;
    public bool isAlive = true;

    void Start()
    {
        SetAllReferences();
        SetAllParameters();

        // Subscribe to the OnPlayerDeath event
        _playerManager.OnObjectiveFailed += Die;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        _healthBar.SetHealthBarPercentage(currentHealth / maxHealth);

        if (currentHealth <= 0)
            CallDeath();
    }

    private void CallDeath()
    {
        if (!isAlive) return;
        _playerManager.OnDeath();
        isAlive = false;
        Destroy(gameObject);
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void SetAllReferences()
    {
        _healthBar = GetComponentInChildren<UIHealthBar>();
        _playerManager = FindObjectOfType<PlayerManager>();  // Find the PlayerManager in the scene
    }

    private void SetAllParameters()
    {
        currentHealth = maxHealth;
    }

    // Unsubscribe when the object is destroyed or no longer needed
    private void OnDestroy()
    {
        if (_playerManager != null)
        {
            _playerManager.OnObjectiveFailed -= Die;
        }
    }
}