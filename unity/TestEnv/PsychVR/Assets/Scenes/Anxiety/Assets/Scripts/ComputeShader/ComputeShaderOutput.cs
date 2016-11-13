using UnityEngine;
using System.Collections;
using System.Linq;

[AddComponentMenu("Scripts/Charles Will Code It/Compute Shader Scripts/ComputeShaderOutPut")]
public class ComputeShaderOutput : MonoBehaviour
{

    #region Compute Shader Fields and Properties

    /// <summary>
    /// The Compute shader we will use
    /// </summary>
    public ComputeShader computeShader;

    /// <summary>
    /// The total number of verticies to calculate.
    /// 10 * 10 * 10 block rendered in 10 * 10 * 10 threads in 1 * 1 * 1 groups
    /// </summary>
    public const int VertCount = 10 * 10 * 10 * 10 * 10 * 10;

    /// <summary>
    /// This buffer will store the calculated data resulting from the Compute shader.
    /// </summary>
    public ComputeBuffer outputBuffer;

    public Shader PointShader;
    Material PointMaterial;

    public bool DebugRender = false;

    /// <summary>
    /// A Reference to the CS Kernel we want to use.
    /// </summary>
    int CSKernel;

    #endregion

    void InitializeBuffers()
    {
        // Set output buffer size.
        outputBuffer = new ComputeBuffer(VertCount, (sizeof(float) * 3) + (sizeof(int) * 6));

        computeShader.SetBuffer(CSKernel, "outputBuffer", outputBuffer);

        if (DebugRender)
            PointMaterial.SetBuffer("buf_Points", outputBuffer);
    }


    public void Dispatch()
    {
        if (!SystemInfo.supportsComputeShaders)
        {
            Debug.LogWarning("Compute shaders not supported (not using DX11?)");
            return;
        }

        computeShader.Dispatch(CSKernel, 10, 10, 10);
    }

    void ReleaseBuffers()
    {
        outputBuffer.Release();
    }

    void Start()
    {
        CSKernel = computeShader.FindKernel("CSMain");

        if (DebugRender)
        {
            PointMaterial = new Material(PointShader);
            PointMaterial.SetVector("_worldPos", transform.position);
        }

        InitializeBuffers();
    }
    
    void OnRenderObject()
    {
        if (DebugRender)
        {            
            Dispatch();        
            PointMaterial.SetPass(0);
            PointMaterial.SetVector("_worldPos", transform.position);

            Graphics.DrawProcedural(MeshTopology.Points, VertCount);
        }
    }

    private void OnDisable()
    {
        ReleaseBuffers();
    }
}