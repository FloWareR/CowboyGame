using UnityEngine;

public class AiMoveToObjective : AiState
{
    private float _timer = 0f;

    public AiStateID GetID()
    {
        return AiStateID.MoveToObjective;
    }

    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.ResetPath();
    }

    public void Update(AiAgent agent)
    {
        if (!agent.enabled) return; 

        // Check if the player is within sight
        var playerDirection = agent.playerTranform.position - agent.transform.position;
        if (playerDirection.magnitude <= agent.config.maxSightDistance)
        {
            var agentDirection = agent.transform.forward;
            playerDirection.Normalize();
            var dotProduct = Vector3.Dot(playerDirection, agentDirection);
            if (dotProduct > 0f)
            {
                agent.StateMachine.ChangeState(AiStateID.ChasePlayer);
                return;
            }
        }

        // Move towards the objective if not chasing the player
        if (!agent.navMeshAgent.hasPath || agent.navMeshAgent.destination != agent.objectiveTransform.position)
        {
            Debug.Log("Setting destination to objective.");
            agent.navMeshAgent.SetDestination(agent.objectiveTransform.position);
        }

        // Check if reached the objective
        if (agent.navMeshAgent.remainingDistance <= agent.navMeshAgent.stoppingDistance && !agent.navMeshAgent.pathPending)
        {
            Debug.Log("Changing state to AttackObjective.");

            agent.StateMachine.ChangeState(AiStateID.AttackObjective);
        }

        _timer -= Time.deltaTime;

    }


    public void Exit(AiAgent agent)
    {
        
    }
}
