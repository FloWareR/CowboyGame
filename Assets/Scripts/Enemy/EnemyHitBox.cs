using System;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyHitBox : MonoBehaviour
{
    public EnemyHealthManager enemyHealthManager;
    public PlayerActions playerActions;

    private void Start()
    {

        playerActions = FindObjectOfType<PlayerActions>();
    }

    public void OnWeaponDamage(ProjectileScript projectile, Vector3 direction)
    {
        enemyHealthManager.TakeDamage(projectile.damage, direction);
    }

    public void OnMeleeDamage(float damage, Vector3 direction)
    {
        enemyHealthManager.TakeDamage(damage, direction);
    }
    
    private void OnParticleCollision(GameObject other)
    {   
        var damage = 0f;
        switch (other.name)
        {
            case "SayainAttack(Clone)":
                damage = playerActions.ultimateDamage;
                break;
        }
        
        ParticleSystem particleSystem = other.GetComponent<ParticleSystem>();
        ParticleSystemRenderer particleRenderer = other.GetComponent<ParticleSystemRenderer>();
        Vector3 hitDirection = (other.transform.position - transform.position).normalized * -1f;

        enemyHealthManager.TakeDamage(damage, hitDirection);

    }
}
