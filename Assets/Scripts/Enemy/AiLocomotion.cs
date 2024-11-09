using UnityEngine;
using UnityEngine.AI;

public class AiLocomotion : MonoBehaviour
{

    private NavMeshAgent _agent;
    private Animator _animator;

    
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int IsDead = Animator.StringToHash("IsDead"); 

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {


        if (_agent.hasPath)
        {
            _animator.SetFloat(Vertical, _agent.velocity.magnitude);
        }
        else
        {
            _animator.SetFloat(Vertical, 0);
        }
    }

    public void DisableMovement()
    {
        _agent.isStopped = true; 
    }
}