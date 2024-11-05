using System;
using UnityEngine;

public class AnimationAttackManager : MonoBehaviour
{
    private PlayerLocomotion _playerLocomotion;
    private ParticleManager _particleManager;
    private PlayerManager _playerManager;

    private void Awake()
    {
        _playerLocomotion = FindObjectOfType<PlayerLocomotion>();
        _particleManager = FindObjectOfType<ParticleManager>();
        _playerManager = FindObjectOfType<PlayerManager>();
    }

    public void TriggerHeavyAttackEffect()
    {
        _particleManager.TriggerHeavyAttackEffect();    
    }

    public void DestroyHeavyAttackEffect()
    {
        _particleManager.DestroyHeavyAttackEffect();    
    }
}
