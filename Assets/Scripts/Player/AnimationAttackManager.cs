using UnityEngine;

public class AnimationAttackManager : MonoBehaviour
{
    private Animator _animator;
    private ParticleManager _particleManager;
    private static readonly int IsInteracting = Animator.StringToHash("isInteracting");

    private void Awake()
    {
        _particleManager = FindObjectOfType<ParticleManager>();
        _animator = GetComponent<Animator>();
    }

    public void TriggerHeavyAttackEffect()
    {
        _particleManager.ToggleParticleSystem("heavyAttack", true);   
    }

    public void DestroyHeavyAttackEffect()
    {
        _particleManager.ToggleParticleSystem("heavyAttack", false); 
    }

    public void ExitPunchingAnimation()
    {
        _animator.SetBool(IsInteracting, false);
    }
}
