using UnityEngine;

public class DisableAnimator : MonoBehaviour
{
    public VisitorEntity visitorEntity;
    public void DisableAnim()
    {
        GetComponent<Animator>().enabled = false;
    }
}