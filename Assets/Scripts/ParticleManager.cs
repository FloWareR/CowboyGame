using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem heavyAttack;
    [SerializeField] private ParticleSystem additionalFX;
    [SerializeField] private ParticleSystem runTrail;
    
    [SerializeField] private Transform attackOrigin;
    
    private PlayerManager _playerManager;
    private ParticleSystem _spawnedHeavyAttack;
    private ParticleSystem _spawnedAdditionalFx;
    private ParticleSystem _runTrail;
    
    private void Awake()
    {
        _spawnedHeavyAttack = Instantiate(heavyAttack, attackOrigin.position, attackOrigin.rotation, attackOrigin.transform);
        _spawnedHeavyAttack.Stop();
        _playerManager = GetComponent<PlayerManager>();
        _runTrail = Instantiate(runTrail, transform.position, transform.rotation, transform.transform);
    }
    
    public void TriggerHeavyAttackEffect()
    {
        _spawnedHeavyAttack.Play();
    }

    public void DestroyHeavyAttackEffect()
    {
        _spawnedHeavyAttack.Stop();
    }

    public void TriggerRunTrail()
    {
        if (!_runTrail.isPlaying)
        {
            _runTrail.Play();
        }
    }

    public void StopRunTrail()
    {
        if (!_runTrail.isStopped)
        {
            _runTrail.Stop();
        }
    }

}
