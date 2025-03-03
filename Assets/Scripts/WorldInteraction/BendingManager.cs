using UnityEngine;
using UnityEngine.Rendering;

public class BendingManager : MonoBehaviour
{
    public static float near = 0.001f;
    public static float far = 99;
    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }

    private static void OnBeginCameraRendering(ScriptableRenderContext ctx, Camera cam)
    {
        cam.cullingMatrix = Matrix4x4.Ortho(-99, 99, -99, 99, near, far) * cam.worldToCameraMatrix;           
    }

    private static void OnEndCameraRendering(ScriptableRenderContext ctx, Camera cam)
    {
        cam.ResetCullingMatrix();
    }
}
