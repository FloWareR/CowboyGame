using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeColliderScript : MonoBehaviour
{

    private PlayerActions _playerActions;

    private void Awake()
    {
        _playerActions = FindObjectOfType<PlayerActions>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<EnemyHitBox>(out var hitBox)) return;
        var direction = (other.transform.position - transform.position).normalized; 
        hitBox.OnMeleeDamage(_playerActions.meleeDamage, direction);
    }

    
}
