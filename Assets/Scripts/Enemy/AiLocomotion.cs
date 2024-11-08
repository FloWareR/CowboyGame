using System;
using UnityEngine;
using UnityEngine.AI;

public class AiLocomotion : MonoBehaviour
{
    [SerializeField] private Transform player;
    
    private NavMeshAgent _agent;
    private Animator 
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _agent.destination = player.transform.position;
    }


}
