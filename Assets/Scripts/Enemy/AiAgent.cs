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
    [HideInInspector] public Transform objectiveTransform;
    [HideInInspector] public AnimatorManager animatorManager;

    void Start()
    {
        StateMachine = new AiStateMachine(this);
        GetAllReferences();
        GetAllStates();

        // Subscribe to the event
        PlayerManager playerManager = FindObjectOfType<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.OnObjectiveFailed += AutoDestruct;
        }
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
        objectiveTransform = GameObject.FindGameObjectWithTag("Objective").transform;

        animatorManager = GetComponent<AnimatorManager>();
        navMeshAgent.stoppingDistance = config.stoppingDistance;
    }

    private void GetAllStates()
    {
        StateMachine.RegisterState(new AiChasePlayer());
        StateMachine.RegisterState(new AiDeathState());
        StateMachine.RegisterState(new AiLookForTarget());
        StateMachine.RegisterState(new AiAttackState());
        StateMachine.RegisterState(new AiMoveToObjective());
        StateMachine.RegisterState(new AiAttackObjectiveState());

        StateMachine.ChangeState(initialState);
    }

    // AutoDestruct function when the event is triggered
    private void AutoDestruct()
    {
        // Example: Disable the AI agent or trigger death
        Debug.Log($"{name} has been destroyed because the objective failed.");
        Destroy(gameObject); // Destroys the AI object
    }

    // Unsubscribe from the event when destroyed
    private void OnDestroy()
    {
        PlayerManager playerManager = FindObjectOfType<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.OnObjectiveFailed -= AutoDestruct; // Unsubscribe to avoid memory leaks
        }
    }
}
