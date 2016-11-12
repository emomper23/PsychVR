using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
    public class LowPolyTerrainGeneratorUI : UIBase
    {
        public MeshFilter meshFilter;
        public GameObject terrain;
        public RectTransform leftPanel;
        private MeshCollider meshCollider;
        private MeshCollider meshCollider2;
        public List<Mesh> meshes = new List<Mesh>();
        public List<MeshDraft> drafts = new List<MeshDraft>();


        [Space]
        [Range(minXSize, maxXSize)]
        public int terrainSizeX = 20;
        [Range(minYSize, maxYSize)]
        public int terrainSizeY = 1;
        [Range(minZSize, maxZSize)]
        public int terrainSizeZ = 20;
        [Range(minCellSize, maxCellSize)]
        public float cellSize = 1;
        [Range(minNoiseScale, maxNoiseScale)]
        public int noiseScale = 5;
        private const int minXSize = 10;
        private const int maxXSize = 100;
        private const int minYSize = 1;
        private const int maxYSize = 5;
        private const int minZSize = 10;
        private const int maxZSize = 100;
        private const float minCellSize = 0.3f;
        private const float maxCellSize = 2;
        private const int minNoiseScale = 1;
        private const int maxNoiseScale = 20;
        private int vert_x;
        private int vert_z;


        private List<ColorHSV> targetPalette = new List<ColorHSV>();
        private List<ColorHSV> currentPalette = new List<ColorHSV>();

        private void Awake()
        {

            RenderSettings.skybox = new Material(RenderSettings.skybox);
            meshCollider = terrain.GetComponents<MeshCollider>()[0];
            meshCollider2 = terrain.GetComponents<MeshCollider>()[1];


            Generate();
            currentPalette.AddRange(targetPalette);
        }

        private void Update()
        {
            SkyBoxGenerator.LerpSkybox(RenderSettings.skybox, currentPalette, targetPalette, 0, 1, 4, Time.deltaTime);
        }

        public void Generate()
        {
            Vector3 terrainSize = new Vector3(terrainSizeX, terrainSizeY, terrainSizeZ);

            targetPalette = RandomE.TetradicPalette(0.25f, 0.75f);
            targetPalette.Add(ColorHSV.Lerp(targetPalette[0], targetPalette[1], 0.5f));

            var gradient = ColorE.Gradient(from: targetPalette[2].WithSV(0.8f, 0.8f),
                to: targetPalette[3].WithSV(0.8f, 0.8f));

            var draft = LowPolyTerrainGenerator.TerrainDraft(terrainSize, cellSize, noiseScale, gradient);
            vert_x = (int)(terrainSizeX/cellSize);
            vert_z = (int)(terrainSizeZ / cellSize);
            Debug.Log(vert_x + " " + vert_z);
            for (int i = 0; i < 9; i++)
            {
                MeshDraft temp = LowPolyTerrainGenerator.TerrainDraft(terrainSize, cellSize, noiseScale, gradient);
                temp.Move(Vector3.left * terrainSizeX / 2 + Vector3.back * terrainSizeZ / 2);
                drafts.Add(temp);
                
            }
            draft.Move(Vector3.left*terrainSizeX/2 + Vector3.back*terrainSizeZ/2);
           
            meshFilter.mesh = draft.ToMesh();
            meshCollider.sharedMesh = draft.ToMesh();
            meshCollider2.sharedMesh = draft.ToMesh();
            ConnectMesh( drafts[4],drafts[5]);

            for (int i = 0; i < 9; i++)
            {
                meshes.Add(drafts[i].ToMesh());
            }


        }

        public void ConnectMesh(MeshDraft center, MeshDraft draft)
        {

            Debug.Log(draft.triangles.ToArray().Length);
            Debug.Log(draft.vertices.ToArray().Length);
            Debug.Log(draft.normals.ToArray().Length);
            int length = draft.normals.ToArray().Length;
            Vector3[] vert = center.vertices.ToArray();
            Vector3[] norm = center.normals.ToArray();
            int[] tri = center.triangles.ToArray();
            center.colors.RemoveRange(0, length);
            center.vertices.RemoveRange(0, length);
            center.normals.RemoveRange(0, length);
            center.triangles.RemoveRange(0, length);
            
            for (int i = 0; i < length ; i++)
              {
                if (i > vert_x * 6)
                {
                    center.colors.Add(new Color(0, 255, 0));
                    center.vertices.Add(vert[i]);
                    center.triangles.Add(tri[i]);
                    center.normals.Add(norm[i]);
                }
                else
                {
                  
                    center.colors.Add(new Color(255, 0, 0));
                    center.vertices.Add(draft.vertices[i]);
                    center.triangles.Add(draft.triangles[i]);
                    center.normals.Add(draft.normals[i ]);   
                    //center.vertices.Add(vert[i]);
                    //center.triangles.Add(tri[i]);
                    //center.normals.Add(norm[i]);
                }
              }
            //center.length.
              Debug.Log("connected");
             
     

        }
        public void UpdateVerticies(GameObject terrain, int idx)
        {
            terrain.GetComponent<MeshFilter>().mesh = meshes[idx];
            terrain.GetComponents<MeshCollider>()[0].sharedMesh = meshes[idx];
            terrain.GetComponents<MeshCollider>()[1].sharedMesh = meshes[idx];
            //Debug.Log("update" + idx);
        }
    }
    
}