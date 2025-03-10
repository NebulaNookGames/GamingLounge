using System;

public class StateMachine
{
    #region Variables

    /// <summary>
    /// The state currently active in the state machine.
    /// </summary>
    private State currentState;

    #endregion Variables

    #region Constructor

    public StateMachine(State firstState)
    {
        if (firstState == null)
        {
            throw new ArgumentNullException(nameof(firstState), "The first state cannot be null.");
        }

        currentState = firstState;
        currentState.EnterState(); // Initialize the first state
    }

    #endregion Constructor

    #region Methods

    public void Update()
    {
        // If the state has an update function, execute it
        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }

    /// <summary>
    /// Sets the current state to the new state.
    /// </summary>
    /// <param name="newState">The state to transition to.</param>
    public void SetState(State newState)
    {
        if (newState == null)
        {
            Console.WriteLine("Attempted to transition to a null state.");
            return;
        }

        // Check if we are trying to transition to the same state
        if (newState == currentState)
        {
            Console.WriteLine("Already in the target state.");
            return;
        }

        // Exit the current state
        currentState.ExitState();

        // Enter the new state
        newState.EnterState();
        
        // Log the state transition for debugging purposes
        Console.WriteLine($"Transitioning from {currentState.GetType().Name} to {newState.GetType().Name}");
        
        // Update the current state to the new one
        currentState = newState;
    }

    #endregion Methods
}