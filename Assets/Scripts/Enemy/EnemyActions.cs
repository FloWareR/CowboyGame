using System;
using UnityEngine;

public class EnemyActions : MonoBehaviour
{
    private AnimatorManager _animatorManager;
    
    private static readonly int IsInteracting = Animator.StringToHash("isInteracting");


    private void Awake()
    {
        SetAllReferences();
    }

    public void HandlePrimaryAttack()
    {
        _animatorManager.PlayTargetAnimation("PrimaryAttack", true);
    }
    
    private void SetAllReferences()
    {
        _animatorManager = GetComponentInChildren<AnimatorManager>();
    }
}
