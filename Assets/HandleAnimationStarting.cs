using UnityEngine;

public class HandleAnimationStarting : MonoBehaviour
{
    public Animator anim;
    private string boolName;

    public void StopAnimation(string newBoolName)
    {
        anim.SetBool(newBoolName, false);
    }
    
    public void PlayAnimation(float invokeTime, string newBoolName)
    {
        boolName = newBoolName;
        Invoke(nameof(Play), invokeTime);
    }

    void Play()
    {
        anim.SetBool(boolName, true);
    }
}

