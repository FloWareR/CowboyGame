using UnityEngine;

public class AiStateMachine
{
    public AiState[] States;
    public AiAgent Agent;
    public AiStateID CurrentState;

    public AiStateMachine(AiAgent agent)
    {
        this.Agent = agent;
        var numStates = System.Enum.GetNames(typeof(AiStateID)).Length;
        States = new AiState[numStates];
    }

    public void RegisterState(AiState state)
    {
        var index = (int)state.GetID();
        States[index] = state;
    }

    public AiState GetState(AiStateID stateID)
    {
        var index = (int)stateID;
        return States[index];
    }
    
    public void Update()
    {
        GetState(CurrentState)?.Update(Agent);
    }

    public void ChangeState(AiStateID newState)
    {
        GetState(CurrentState)?.Exit(Agent);
        CurrentState = newState;
        GetState(CurrentState)?.Enter(Agent);
    }
}
