using UnityEngine;

public class AiAttackState : AiState
{
    public AiStateID GetID()
    {
        return AiStateID.Attack;
    }

    public void Enter(AiAgent agent)
    {
        agent.animatorManager.PlayTargetAnimation("PrimaryAttack", true);
    }

    public void Update(AiAgent agent)
    {
        if (agent.isInteracting) return;
        float distanceToPlayer = Vector3.Distance(agent.transform.position, agent.playerTranform.position);

        if (distanceToPlayer <= agent.config.stoppingDistance)
        {
            if (!agent.isInteracting)
            {
                agent.animatorManager.PlayTargetAnimation("PrimaryAttack", true);
            }
        }
        else
        {
            agent.StateMachine.ChangeState(AiStateID.LookForTarget);
        }
    }


    public void Exit(AiAgent agent)
    {
        
    }
}
