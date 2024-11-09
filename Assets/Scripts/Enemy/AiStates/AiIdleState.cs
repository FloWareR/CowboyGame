using UnityEngine;

public class AiIdleState : AiState
{
    public AiStateID GetID()
    {
        return AiStateID.Idle;
    }

    public void Enter(AiAgent agent)
    {
        
    }

    public void Update(AiAgent agent)
    {
        Vector3 playerDirection = agent.playerTranform.position - agent.transform.position;
        if (playerDirection.magnitude > agent.config.maxSightDistance) return;
        
        Vector3 agentDirectino = agent.transform.forward;
        
        playerDirection.Normalize();

        var dotProduct = Vector3.Dot(playerDirection, agentDirectino);
        if(dotProduct > 0f) agent.StateMachine.ChangeState(AiStateID.ChasePlayer);
    }

    public void Exit(AiAgent agent)
    {
        
    }
}
