using UnityEngine;
using UnityEngine.AI;

public class DebugNavMeshComponent : MonoBehaviour
{
    public bool velocity;
    public bool desiredVelocity;
    public bool path;
    
    private NavMeshAgent _agent;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void OnDrawGizmos()
    {
        if (velocity)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + _agent.velocity);
        }

        if (desiredVelocity)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + _agent.desiredVelocity);
        }

        if (path)
        {
            Gizmos.color = Color.red;
            var agentPath = _agent.path;
            var prevCorner = transform.position;
            foreach (var corner in agentPath.corners)
            {
                Gizmos.DrawLine(prevCorner, corner);
                Gizmos.DrawSphere(corner, 0.1f);
                prevCorner = corner;
            }
        }
    }
}
