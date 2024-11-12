using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAttackObjectiveState : AiState
{
    public AiStateID GetID()
    {
        return AiStateID.AttackObjective;
    }

    public void Enter(AiAgent agent)
    {
        agent.animatorManager.PlayTargetAnimation("PrimaryAttack", true);

    }

    public void Update(AiAgent agent)
    {
        if (agent.isInteracting) return;
        float distanceToObjective = Vector3.Distance(agent.transform.position, agent.objectiveTransform.position);
        float distanceToPlayer = Vector3.Distance(agent.transform.position, agent.playerTranform.position);
        if (distanceToObjective <= agent.config.maxDistance && distanceToPlayer > agent.config.maxSightDistance)
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
