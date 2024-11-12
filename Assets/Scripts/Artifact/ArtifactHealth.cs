using System.Collections;
using UnityEngine;

public class ArtifactHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private GameOver gameOver;

    private UIHealthBar _healthBar;

    void Start()
    {
        SetAllReferences();
        SetAllParameters();
    }

    public void TakeDamage(float damage)
    {
        print("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        currentHealth -= damage;
        _healthBar.SetHealthBarPercentage(currentHealth / maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
        gameOver.GameOverScreen();
    }

    private void SetAllReferences()
    {
        _healthBar = GetComponentInChildren<UIHealthBar>();
    }

    private void SetAllParameters()
    {
        currentHealth = maxHealth;
    }
}
