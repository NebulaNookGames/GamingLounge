using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The base class for all entities that holds the references that all entities have in common.
/// </summary>
public class Entity : MonoBehaviour
{
    #region Serialized Variables
    
    [Header("Entity Components")]
    [Tooltip("The animator of the entity.")]
    [SerializeField] protected Animator entityAnimator;

    /// <summary>
    /// Public accessor for the entity's Animator.
    /// </summary>
    public Animator EntityAnimator
    {
        get { return entityAnimator; }
        set { entityAnimator = value; }
    }
    
    #endregion Serialized Variables

    #region Core References

    [Header("Core References")]
    [Tooltip("Manages and transitions from and to states.")]
    protected StateMachine stateMachine;

    /// <summary>
    /// Public accessor for the entity's StateMachine.
    /// </summary>
    public StateMachine StateMachine => stateMachine;
    
    [Tooltip("The NavMeshAgent on this gameObject.")]
    protected NavMeshAgent agent;

    /// <summary>
    /// Public accessor for the entity's NavMeshAgent.
    /// </summary>
    public NavMeshAgent Agent => agent;

    [Tooltip("The state that this entity initially begins in.")]
    protected State initialState;

    #endregion Core References

    #region Unity Methods

    /// <summary>
    /// Calls the Initialization method.
    /// </summary>
    protected void Start()
    {
        Initialization();
    }

    /// <summary>
    /// Updates the state machine every frame.
    /// </summary>
    private void Update()
    {
        stateMachine.Update();
    }

    #endregion Unity Methods

    #region Initialization

    /// <summary>
    /// Initializes core components such as the NavMeshAgent and StateMachine.
    /// The speed and scale of the entity may be randomly set to simulate differences between entities.
    /// </summary>
    private void Initialization()
    {
        // Assign the NavMeshAgent if present on the GameObject.
        agent = GetComponent<NavMeshAgent>();
        
        // Instantiate the state machine with the initial state.
        stateMachine = new StateMachine(initialState);
    }

    #endregion Initialization
}