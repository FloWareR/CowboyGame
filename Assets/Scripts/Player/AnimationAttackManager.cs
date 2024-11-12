using UnityEngine;
using UnityEngine.Serialization;

public class AnimationAttackManager : MonoBehaviour
{
    private Animator _animator;
    private ParticleManager _particleManager;
    [SerializeField] private Collider meleeCollider;
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

    public void TriggerPunchingAnimation()
    {
        meleeCollider.enabled = true;
        _particleManager.ToggleParticleSystem("meleeSlash", true);
    }
    
    public void ExitPunching()
    {
        meleeCollider.enabled = false;
    }
    public void ExitPunchingAnimation()
    {
        meleeCollider.enabled = false;
        _particleManager.ToggleParticleSystem("meleeSlash", false);
        _animator.SetBool(IsInteracting, false);
    }


    
}
