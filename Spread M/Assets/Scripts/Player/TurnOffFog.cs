using UnityEngine;
using UnityEngine.Rendering;

public class TurnOffFog : MonoBehaviour
{
    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += BeginRender;
        RenderPipelineManager.endCameraRendering += EndRender;
    }

    private void OnDisable()
    {

        RenderPipelineManager.beginCameraRendering -= BeginRender;
        RenderPipelineManager.endCameraRendering -= EndRender;
    }


    void BeginRender(ScriptableRenderContext context, Camera camera)
    {
        if (camera.name == gameObject.GetComponent<Camera>().name)
        {
            RenderSettings.fog = false;
        }
    }
    void EndRender(ScriptableRenderContext context, Camera camera)
    {
        if (camera.name == gameObject.GetComponent<Camera>().name)
        {
            RenderSettings.fog = true;
        }
    }
}

