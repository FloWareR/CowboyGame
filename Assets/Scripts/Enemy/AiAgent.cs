using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    public AiStateID initialState;
    public AiStateMachine StateMachine;
    public AiAgentConfig config;
    public bool isInteracting;
    
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Ragdoll ragdoll;
    [HideInInspector] public UIHealthBar healthBar;
    [HideInInspector] public SkinnedMeshRenderer skinnedMeshRenderer;
    [HideInInspector] public Transform playerTranform;
    [HideInInspector] public AnimatorManager animatorManager;

    
    void Start()
    {
        StateMachine = new AiStateMachine(this);
        GetAllReferences();
        GetAllStates();
        
    }

    void Update()
    {
        StateMachine.Update();
        isInteracting = animatorManager.AnimatorComponent.GetBool("isInteracting");
    }


    private void GetAllReferences()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        ragdoll = GetComponent<Ragdoll>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        healthBar = GetComponentInChildren<UIHealthBar>();
        playerTranform = GameObject.FindGameObjectWithTag("Player").transform;
        animatorManager = GetComponent<AnimatorManager>();
        navMeshAgent.stoppingDistance = config.stoppingDistance;
    }

    private void GetAllStates()
    {
        StateMachine.RegisterState(new AiChasePlayer());
        StateMachine.RegisterState(new AiDeathState());
        StateMachine.RegisterState(new AiIdleState());
        StateMachine.RegisterState(new AiAttackState());
        StateMachine.ChangeState(initialState);
    }
}
