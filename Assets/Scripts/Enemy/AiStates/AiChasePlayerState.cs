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
        agent.navMeshAgent.ResetPath(); // Reset any existing path
    }

    public void Update(AiAgent agent)
    {
        if (!agent.enabled) return; 

        _timer -= Time.deltaTime;

        // Continuously update the destination to the player's current position
        if (!agent.navMeshAgent.hasPath || agent.navMeshAgent.destination != agent.playerTranform.position)
        {
            agent.navMeshAgent.SetDestination(agent.playerTranform.position);
        }

        // Timer logic for checking player position relative to max distance
        if (_timer < 0.0f)
        {
            var direction = (agent.playerTranform.position - agent.navMeshAgent.destination);
            direction.y = 0;
            if (direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.navMeshAgent.SetDestination(agent.playerTranform.position);
                }
            }
            else
            {
                var distance = Vector3.Distance(agent.playerTranform.position, agent.transform.position);
        
                // If the player is out of range, change to MoveToObjective state
                if (distance > agent.config.maxDistance)
                {
                    agent.StateMachine.ChangeState(AiStateID.MoveToObjective);
                }
                _timer = agent.config.maxTime; // Reset the timer
            }
        }

        // Check if the AI is close enough to attack the player
        if (agent.navMeshAgent.remainingDistance <= agent.navMeshAgent.stoppingDistance && !agent.navMeshAgent.pathPending)
        {
            agent.StateMachine.ChangeState(AiStateID.Attack);
        }
    }

    public void Exit(AiAgent agent)
    {
        // Optionally stop the agent's movement when exiting the state
    }
}
