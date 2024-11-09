using UnityEngine;
using UnityEngine.Serialization;

public class HitBox : MonoBehaviour
{
    public HealthManager healthManager;

    public void OnWeaponDamage(ProjectileScript projectile, Vector3 direction)
    {
        healthManager.TakeDamage(projectile.damage, direction);
    }
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);
        var damage = 0f;
        switch (other.name)
        {
            case "SayainAttack(Clone)":
                damage = 1f;
                break;
        }
        
        ParticleSystem particleSystem = other.GetComponent<ParticleSystem>();
        ParticleSystemRenderer particleRenderer = other.GetComponent<ParticleSystemRenderer>();
        Vector3 hitDirection = (other.transform.position - transform.position).normalized;

        healthManager.TakeDamage(damage, hitDirection);

    }
}
