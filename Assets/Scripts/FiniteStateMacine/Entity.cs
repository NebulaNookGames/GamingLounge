using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The base class for all entities that holds the references that all entities have in common.
/// </summary>
public class Entity : MonoBehaviour
{
    #region Serialized Variables
    
    [Tooltip("The animator of the entity.")]
    [SerializeField] protected Animator entityAnimator;

    public Animator EntityAnimator
    {
        get
        {
            return entityAnimator; 
        }
        set
        {
            entityAnimator = value;
        }
    }
    
    #endregion Serialized Variables

    #region Core References

    /// <summary>
    /// Manages and transitions from and to states.
    /// </summary>
    protected StateMachine stateMachine;

    public StateMachine StateMachine
    { get { return stateMachine; } }
    
    /// <summary>
    /// The NavMeshAgent on this gameObject.
    /// </summary>
    protected NavMeshAgent agent;

    public NavMeshAgent Agent
    { get { return agent; } }

    /// <summary>
    /// The state that this entity initially begins in.
    /// </summary>
    protected State initialState;

    #endregion Core References

    #region Unity Methods

    /// <summary>
    /// Calls the <see cref="Initialization"/> method.
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
    /// For all entities to function the <see cref="agent"/>, <see cref="worldTime"/> and <see cref="stateMachine"/>
    /// must be referenced or instanced.
    /// The speed and scale of the entity is also randomly set to simulate difference between entities.
    /// </summary>
    private void Initialization()
    {
        // Agent referencing and speed changing.
        if (GetComponent<NavMeshAgent>())
        {
            agent = GetComponent<NavMeshAgent>();
        }
        
        // State machine instancing.
        stateMachine = new StateMachine(initialState);
    }

    #endregion Initialization
}