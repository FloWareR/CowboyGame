using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{

    public AiAgentConfig config;
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            PlayerHealthManager player = other.GetComponent<PlayerHealthManager>();

            if (player != null)
            {
                player.TakeDamage(config.damage, transform.forward);
            }
        }
        
        if (other.CompareTag("Objective"))
        {
             ArtifactHealth artifact = other.GetComponentInParent<ArtifactHealth>();

            if (artifact != null)
            {
                artifact.TakeDamage(config.damage);
            }
            else
            {
                Debug.Log("PUTO");
            }
        }
    }
    
}