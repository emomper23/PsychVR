using UnityEngine;
using System.Collections;

public class VoxelComputeShaderOutput : MonoBehaviour
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
    int VertCount;

    public int Seed;

    /// <summary>
    /// This buffer will store the calculated data resulting from the Compute shader.
    /// </summary>
    public ComputeBuffer outputBuffer;
    public ComputeBuffer mapBuffer;

    public Shader PointShader;
    Material PointMaterial;

    public bool DebugRender = false;

    public int cubeMultiplier = 5;

    /// <summary>
    /// A Reference to the CS Kernel we want to use.
    /// </summary>
    int CSKernel;

    #endregion

    void InitializeBuffers()
    {
        VertCount = 10 * 10 * 10 * cubeMultiplier * cubeMultiplier * cubeMultiplier;

        // Set output buffer size.
        outputBuffer = new ComputeBuffer(VertCount, (sizeof(float) * 3) + (sizeof(int) * 6));
        mapBuffer = new ComputeBuffer(VertCount, sizeof(int));

        int width = 10 * cubeMultiplier;
        int height = 10 * cubeMultiplier;
        int depth = 10 * cubeMultiplier;

        PerlinNoise.Seed = Seed;
        float[][] fmap = PerlinNoise.GeneratePerlinNoise(width, height, 8);
        //fmap = PerlinNoise.GeneratePerlinNoise(fmap, 8);

        int[] map = new int[VertCount];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    int idx = x + (y * 10 * cubeMultiplier) + (z * 10 * cubeMultiplier * 10 * cubeMultiplier);

                    if (fmap[x][z] >= y / (float)height)
                        map[idx] = 1;
                    else
                        map[idx] = 0;
                }
            }
        }

        mapBuffer.SetData(map);

        computeShader.SetBuffer(CSKernel, "outputBuffer", outputBuffer);
        computeShader.SetBuffer(CSKernel, "mapBuffer", mapBuffer);

        computeShader.SetVector("group_size", new Vector3(cubeMultiplier, cubeMultiplier, cubeMultiplier));

        if (DebugRender)
            PointMaterial.SetBuffer("buf_Points", outputBuffer);

        transform.position -= (Vector3.one * 10 * cubeMultiplier) *.5f;
    }


    public void Dispatch()
    {
        if (!SystemInfo.supportsComputeShaders)
        {
            Debug.LogWarning("Compute shaders not supported (not using DX11?)");
            return;
        }

        computeShader.Dispatch(CSKernel, cubeMultiplier, cubeMultiplier, cubeMultiplier);
    }

    void ReleaseBuffers()
    {
        outputBuffer.Release();
        mapBuffer.Release();
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