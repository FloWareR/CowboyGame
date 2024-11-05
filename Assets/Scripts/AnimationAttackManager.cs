using System;
using UnityEngine;

public class AnimationAttackManager : MonoBehaviour
{
    private PlayerActions _playerActions;
    private PlayerLocomotion _playerLocomotion;

    private void Awake()
    {
        _playerActions = FindObjectOfType<PlayerActions>();
        _playerLocomotion = FindObjectOfType<PlayerLocomotion>();

    }

    public void TriggerHeavyAttackEffect()
    {
        _playerActions.TriggerHeavyAttackEffect();    
    }

    public void DestroyHeavyAttackEffect()
    {
        _playerActions.DestroyHeavyAttackEffect();    
        _playerLocomotion.ReinstateMovement();
    }
}
