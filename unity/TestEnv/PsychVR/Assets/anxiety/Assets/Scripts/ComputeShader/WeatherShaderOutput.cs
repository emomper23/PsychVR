using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Charles Will Code It/Compute Shader Scripts/WeatherShaderOutput")]
public class WeatherShaderOutput : MonoBehaviour
{

    #region Compute Shader Fields and Properties

    /// <summary>
    /// The Compute shader we will use
    /// </summary>
    public ComputeShader computeShader;

    /// <summary>
    /// The total number of verticies to calculate. We are going to have 32 * 32  work groups, each processing 16 * 16 * 1 points.
    /// </summary>
    public const int VertCount = 32 * 32 * 16 * 16 * 1;

    /// <summary>
    /// The initial start position for each of the points to be calculated.
    /// </summary>
    ComputeBuffer startPointBuffer;
    /// <summary>
    /// This buffer will store the calculated data resulting from the Compute shader.
    /// </summary>
    public ComputeBuffer outputBuffer;
    /// <summary>
    /// This buffer is used to hold constant values for each point, in this case a value for time.
    /// </summary>
    ComputeBuffer constantBuffer;

    ComputeBuffer modBuffer;

    public Shader PointShader;
    Material PointMaterial;

    [Tooltip("Speed weather is falling at, 1 would be slow like snow, 10, rain")]
    [Range(1,200)]
    public float Speed = 1;

    [Tooltip("Does the weather element wobble, like snow?")]
    public bool Wobble = false;

    [Tooltip("Wind Direction")]
    public Vector3 wind = Vector3.zero;

    [Tooltip("Distance between weather elements")]
    public float spacing = 5;

    /// <summary>
    /// A Reference to the CS Kernel we want to use.
    /// </summary>
    int CSKernel;

    #endregion

    void InitializeBuffers()
    {
        // Set start point compute buffer
        startPointBuffer = new ComputeBuffer(VertCount, 4); // create space in the buffer for 3 float per point (float = 4 bytes)

        // Set const compute buffer size
        constantBuffer = new ComputeBuffer(1, 4);

        modBuffer = new ComputeBuffer(VertCount, 8);

        // Set output buffer size.
        outputBuffer = new ComputeBuffer(VertCount, 12);

        // These values will be the starting y coords for each point.
        float[] values = new float[VertCount];
        Vector2[] mods = new Vector2[VertCount];

        for (int i = 0; i < VertCount; i++)
        {
            values[i] = Random.value * 2 * Mathf.PI;
            mods[i] = new Vector2(.1f + Random.value, .1f + Random.value);
        }

        modBuffer.SetData(mods);

        // Load starting valuse into the compute buffer
        startPointBuffer.SetData(values);

        // Only need to set the offsets once in the CS
        computeShader.SetBuffer(CSKernel, "startPointBuffer", startPointBuffer);
    }

    public void Dispatch()
    {
        constantBuffer.SetData(new[] { Time.time * .01f });

        computeShader.SetBuffer(CSKernel, "modBuffer", modBuffer);
        computeShader.SetBuffer(CSKernel, "constBuffer", constantBuffer);
        computeShader.SetBuffer(CSKernel, "outputBuffer", outputBuffer);
        computeShader.SetFloat("Speed", Speed);
        computeShader.SetInt("wobble", Wobble ? 1 : 0);
        computeShader.SetVector("wind", wind);
        computeShader.SetFloat("spacing", spacing);

        computeShader.Dispatch(CSKernel, 32, 32, 1);
    }

    void ReleaseBuffers()
    {
        modBuffer.Release();
        constantBuffer.Release();
        startPointBuffer.Release();
        outputBuffer.Release();
    }

    void Start()
    {
        CSKernel = computeShader.FindKernel("CSMain");

        PointMaterial = new Material(PointShader);

        InitializeBuffers();
    }

    void OnPostRender()
    {
        if (!SystemInfo.supportsComputeShaders)
        {
            Debug.LogWarning("Compute shaders not supported (not using DX11?)");
            return;
        }

        Dispatch();

        PointMaterial.SetPass(0);
        PointMaterial.SetBuffer("buf_Points", outputBuffer);

        Graphics.DrawProcedural(MeshTopology.Points, VertCount);
    }

    private void OnDisable()
    {
        ReleaseBuffers();
    }
}