using UnityEngine;

public class EnemyAnimationAttackManager : MonoBehaviour
{
    private Animator _animator;
    private ParticleManager _particleManager;
    private AiAgent _agent;
    [SerializeField] private Collider weaponCollider;
    
    private static readonly int IsInteracting = Animator.StringToHash("isInteracting");

    private void Awake()
    {
        _particleManager = FindObjectOfType<ParticleManager>();
        _animator = GetComponent<Animator>();
        _agent = GetComponentInParent<AiAgent>();
    }

    public void AnimationStart()
    {
        weaponCollider.enabled = true;
    }

    public void AnimationEnd()
    {
        weaponCollider.enabled = false;
        _animator.SetBool(IsInteracting, false);
    }
}
