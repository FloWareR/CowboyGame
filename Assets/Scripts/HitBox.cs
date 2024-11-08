using UnityEngine;
using UnityEngine.Serialization;

public class HitBox : MonoBehaviour
{
    public HealthManager healthManager;

    public void OnWeaponDamage(ProjectileScript projectile, Vector3 direction)
    {
        healthManager.TakeDamage(projectile.damage, direction);
    }
}
