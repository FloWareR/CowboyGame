using UnityEngine;

public class AiLookForTarget : AiState
{
    public AiStateID GetID()
    {
        return AiStateID.LookForTarget;
    }

    public void Enter(AiAgent agent)
    {
        
    }

    public void Update(AiAgent agent)
    {
        var playerDirection = agent.playerTranform.position - agent.transform.position;
        if (playerDirection.magnitude <= agent.config.maxSightDistance)
        {
            agent.StateMachine.ChangeState(AiStateID.ChasePlayer);
        }
        else
        {
            agent.StateMachine.ChangeState(AiStateID.MoveToObjective);

        }
        
        var agentDirectino = agent.transform.forward;
    }

    public void Exit(AiAgent agent)
    {
        
    }
}
