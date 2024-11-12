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

        if (distanceToObjective <= agent.config.maxDistance)
        {
            if (!agent.isInteracting)
            {
                agent.animatorManager.PlayTargetAnimation("PrimaryAttack", true);
            }
        }
        else
        {
            agent.StateMachine.ChangeState(AiStateID.MoveToObjective);
        }
    }

    public void Exit(AiAgent agent)
    {
    }
}
