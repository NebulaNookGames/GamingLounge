using UnityEngine;

public class DisableGameObjectOnStateChange : StateMachineBehaviour
{
    /// <summary>
    /// Called when the state machine enters a state.
    /// Inverts the state of GameObjectActivation on entry.
    /// </summary>
    /// <param name="animator">The Animator component of the object.</param>
    /// <param name="stateInfo">Information about the current state.</param>
    /// <param name="layerIndex">The layer index of the state machine.</param>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Check if the GameObject has the GameObjectActivation component
        if (animator.gameObject.GetComponent<GameObjectActivation>())
        {
            // Invert the active state of the GameObject using GameObjectActivation
            animator.gameObject.GetComponent<GameObjectActivation>().Invert();
        }
    }

    /// <summary>
    /// Called when the state machine exits a state.
    /// Inverts the state of GameObjectActivation on exit.
    /// </summary>
    /// <param name="animator">The Animator component of the object.</param>
    /// <param name="stateInfo">Information about the current state.</param>
    /// <param name="layerIndex">The layer index of the state machine.</param>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        // Check if the GameObject has the GameObjectActivation component
        if (animator.gameObject.GetComponent<GameObjectActivation>())
        {
            // Invert the active state of the GameObject using GameObjectActivation
            animator.gameObject.GetComponent<GameObjectActivation>().Invert();
        }
    }
}