using UnityEngine;
using UnityEngine.AI;

public class AiLocomotion : MonoBehaviour
{
    [SerializeField] private float maxTime;
    [SerializeField] private float maxDistance;
    [SerializeField] private Transform player;

    private NavMeshAgent _agent;
    private Animator _animator;

    private float _timer = 0f;
    private bool _isDead = false;
    
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int IsDead = Animator.StringToHash("IsDead"); 

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (_isDead) return; 

        _timer -= Time.deltaTime;
        if (_timer < 0.0f)
        {
            var sqDistance = (player.position - _agent.destination).sqrMagnitude;
            if (sqDistance > maxDistance * maxDistance)
            {
                _agent.destination = player.position;
            }

            _timer = maxTime;
        }

        _animator.SetFloat(Vertical, _agent.velocity.magnitude);
    }

    public void DisableMovement()
    {
        if (_isDead) return; 

        _isDead = true;
        _agent.isStopped = true; 
        
    }
}