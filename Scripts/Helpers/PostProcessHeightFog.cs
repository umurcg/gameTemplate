using UnityEngine;

public class PostProcessHeightFog : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] public Material material;

    private void Start()
    {
        camera.depthTextureMode = camera.depthTextureMode | DepthTextureMode.Depth;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Matrix4x4 v = camera.worldToCameraMatrix;
        Matrix4x4 p = GL.GetGPUProjectionMatrix(camera.projectionMatrix, true);

        Matrix4x4 invVP = Matrix4x4.Inverse(p * v);
        material.SetMatrix("_Matrix_Inv_VP", invVP);
        Graphics.Blit(source, destination, material);
    }
}
