using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AiChasePlayer : AiState
{
    private float _timer = 0f;

    public AiStateID GetID()
    {
        return AiStateID.ChasePlayer;
    }

    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.ResetPath();
    }

    public void Update(AiAgent agent)
    {
        if (!agent.enabled) return; 
        _timer -= Time.deltaTime;
        if (!agent.navMeshAgent.hasPath) agent.navMeshAgent.destination = agent.playerTranform.position;
        
        if (_timer < 0.0f)
        {
            var direction  = (agent.playerTranform.position - agent.navMeshAgent.destination);
            direction.y = 0;
            if (direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                if (agent.navMeshAgent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathPartial)
                {
                    agent.navMeshAgent.destination = agent.playerTranform.position;
                }
            }
            _timer = agent.config.maxTime;
        }
        if (agent.navMeshAgent.remainingDistance <= agent.navMeshAgent.stoppingDistance && !agent.navMeshAgent.pathPending)
        {
                agent.StateMachine.ChangeState(AiStateID.Attack);
        }
    }

    public void Exit(AiAgent agent)
    {
        
    }
}
