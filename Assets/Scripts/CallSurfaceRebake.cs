using UnityEngine;

public class CallSurfaceRebake : MonoBehaviour
{
    public void CallRebake()
    {
        Invoke("ActionInvocation", .2f);
    }

    void ActionInvocation()
    {
       SurfaceReBake.instance.Rebake();
    }
}
