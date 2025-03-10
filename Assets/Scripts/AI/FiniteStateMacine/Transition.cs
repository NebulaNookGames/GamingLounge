using System;

public struct Transition
{
    #region Variables

    /// <summary>
    /// The function declaring if a State can transition or not.
    /// </summary>
    public Func<bool> transition;

    /// <summary>
    /// The state that should be transitioned to, if the func returns true.
    /// </summary>
    public State state;

    #endregion Variables

    #region Constructor

    public Transition(Func<bool> transition, State state)
    {
        this.transition = transition ?? throw new ArgumentNullException(nameof(transition), "Transition condition cannot be null.");
        this.state = state ?? throw new ArgumentNullException(nameof(state), "State cannot be null.");
    }

    #endregion Constructor
}