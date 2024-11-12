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
        var playerDirection = agent.playerTranform.position - agent.transform.position;
        if (playerDirection.magnitude > agent.config.maxSightDistance) return;
        var agentDirectino = agent.transform.forward;
        playerDirection.Normalize();
        var dotProduct = Vector3.Dot(playerDirection, agentDirectino);
        if(dotProduct > 0f) agent.StateMachine.ChangeState(AiStateID.ChasePlayer);
        if (!agent.navMeshAgent.hasPath) agent.navMeshAgent.destination = agent.objectiveTransform.position;
        if (agent.navMeshAgent.remainingDistance <= agent.navMeshAgent.stoppingDistance && !agent.navMeshAgent.pathPending)
        {
            agent.StateMachine.ChangeState(AiStateID.AttackObjective);
        }
        _timer -= Time.deltaTime;

    }

    public void Exit(AiAgent agent)
    {
        
    }
}
