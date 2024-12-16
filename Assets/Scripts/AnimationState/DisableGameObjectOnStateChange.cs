using UnityEngine;

public class DisableGameObjectOnStateChange : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.gameObject.GetComponent<GameObjectActivation>())
            animator.gameObject.GetComponent<GameObjectActivation>().Invert();
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        if(animator.gameObject.GetComponent<GameObjectActivation>())
            animator.gameObject.GetComponent<GameObjectActivation>().Invert();
    }
}
