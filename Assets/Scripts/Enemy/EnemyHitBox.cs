using UnityEngine;
using UnityEngine.Serialization;

public class EnemyHitBox : MonoBehaviour
{
    [FormerlySerializedAs("healthManager")] public EnemyHealthManager enemyHealthManager;

    public void OnWeaponDamage(ProjectileScript projectile, Vector3 direction)
    {
        enemyHealthManager.TakeDamage(projectile.damage, direction);
    }
    private void OnParticleCollision(GameObject other)
    {
        var damage = 0f;
        switch (other.name)
        {
            case "SayainAttack(Clone)":
                damage = 1f;
                break;
        }
        
        ParticleSystem particleSystem = other.GetComponent<ParticleSystem>();
        ParticleSystemRenderer particleRenderer = other.GetComponent<ParticleSystemRenderer>();
        Vector3 hitDirection = (other.transform.position - transform.position).normalized * -1f;

        enemyHealthManager.TakeDamage(damage, hitDirection);

    }
}
