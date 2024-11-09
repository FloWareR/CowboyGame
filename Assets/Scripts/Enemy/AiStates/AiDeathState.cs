using UnityEngine;

public class AiDeathState : AiState
{
    public Vector3 direction;
    public AiStateID GetID()
    {
        return AiStateID.Death;
    }

    public void Enter(AiAgent agent)
    {
        agent.ragdoll.ActivateRagdoll();
        direction.y = 1;
        agent.ragdoll.ApplyForce(direction * agent.config.dieForce);
        agent.healthBar.gameObject.SetActive(false);
        agent.skinnedMeshRenderer.updateWhenOffscreen = true;
    }

    public void Update(AiAgent agent)
    {
        
    }

    public void Exit(AiAgent agent)
    {
        
    }
}
