using UnityEngine;

/// <summary>
/// This class is responsible for calling the rebake action on the SurfaceReBake instance.
/// It triggers the rebake process with a delay using Unity's Invoke method.
/// </summary>
public class CallSurfaceRebake : MonoBehaviour
{
    /// <summary>
    /// Calls the Rebake method with a delay of 0.2 seconds.
    /// This method triggers the Invoke method to schedule the ActionInvocation method.
    /// </summary>
    public void CallRebake()
    {
        // Invoke ActionInvocation method after a delay of 0.2 seconds.
        Invoke("ActionInvocation", 0.2f);
    }

    /// <summary>
    /// Executes the rebake process by calling the Rebake method on the SurfaceReBake instance.
    /// </summary>
    void ActionInvocation()
    {
        // Call the Rebake method on the static instance of SurfaceReBake.
        SurfaceReBake.instance.Rebake();
    }
}