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
        public List<MeshDraft> pregen_graphs = new List<MeshDraft>();
        public List<GameObject> pre_terrains = new List<GameObject>();


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
        private const int maxXSize = 200;
        private const int minYSize = 1;
        private const int maxYSize = 5;
        private const int minZSize = 10;
        private const int maxZSize = 200;
        private const float minCellSize = 0.3f;
        private const float maxCellSize = 2;
        private const int minNoiseScale = 1;
        private const int maxNoiseScale = 20;
        private int vert_x;
        private int vert_z;
        private Gradient grad;

        private List<ColorHSV> targetPalette = new List<ColorHSV>();
        private List<ColorHSV> currentPalette = new List<ColorHSV>();

        private void Awake()
        {

            RenderSettings.skybox = new Material(RenderSettings.skybox);
            meshCollider = terrain.GetComponents<MeshCollider>()[0];
           // meshCollider2 = terrain.GetComponents<MeshCollider>()[1];


            Generate();
            currentPalette.AddRange(targetPalette);

            for (int i = 0; i < 10; i++)
            {
                Vector3 terrainSize = new Vector3(terrainSizeX, terrainSizeY, terrainSizeZ);
                var temp_draft = LowPolyTerrainGenerator.TerrainDraft(terrainSize, cellSize, noiseScale, grad);
                temp_draft.Move(Vector3.left * terrainSizeX / 2 + Vector3.back * terrainSizeZ / 2);
                pregen_graphs.Add(temp_draft);
                GameObject temp = (GameObject)Instantiate(terrain, new Vector3(0, -100, 0), terrain.transform.rotation);
                UpdateDraft(temp, temp_draft);
                pre_terrains.Add(temp);
            }

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
            grad = gradient;

            var draft = LowPolyTerrainGenerator.TerrainDraft(terrainSize, cellSize, noiseScale, gradient);
            vert_x = (int)(terrainSizeX / cellSize);
            vert_z = (int)(terrainSizeZ / cellSize);

            for (int i = 0; i < 9; i++)
            {
                MeshDraft temp = LowPolyTerrainGenerator.TerrainDraft(terrainSize, cellSize, noiseScale, gradient);
                temp.Move(Vector3.left * terrainSizeX / 2 + Vector3.back * terrainSizeZ / 2);
                drafts.Add(temp);

            }
            draft.Move(Vector3.left * terrainSizeX / 2 + Vector3.back * terrainSizeZ / 2);


            meshFilter.mesh = draft.ToMesh();
            meshCollider.sharedMesh = draft.ToMesh();
            //meshCollider2.sharedMesh = draft.ToMesh();

            MorphFitRight(drafts[1], drafts[0]);
            MorphFitRight(drafts[2], drafts[1]);
            MorphFitRight(drafts[5], drafts[4]);
            MorphFitRight(drafts[7], drafts[6]);
            MorphFitRight(drafts[4], drafts[3]);
            MorphFitRight(drafts[8], drafts[7]);


            MorphFitDown(drafts[3], drafts[0]);
            MorphFitDown(drafts[4], drafts[1]);
            MorphFitDown(drafts[5], drafts[2]);
            MorphFitDown(drafts[6], drafts[3]);
            MorphFitDown(drafts[7], drafts[4]);
            MorphFitDown(drafts[8], drafts[5]);

            for (int i = 0; i < 9; i++)
            {
                meshes.Add(drafts[i].ToMesh());
            }


        }

        public void MorphFitRight(MeshDraft fromMesh, MeshDraft toMesh)
        {
            int length = toMesh.normals.Count;

            int limit = vert_x * 6;

            for (int i = 0; i < limit; i += 6)
            {
                int offset = (length - vert_x * 6);

                fromMesh.vertices[i] = (new Vector3(fromMesh.vertices[i].x, toMesh.vertices[offset + i + 1].y, fromMesh.vertices[i].z));
                fromMesh.vertices[i + 3] = (new Vector3(fromMesh.vertices[i + 3].x, toMesh.vertices[offset + i + 1].y, fromMesh.vertices[i + 3].z));
                fromMesh.vertices[i + 5] = (new Vector3(fromMesh.vertices[i + 5].x, toMesh.vertices[offset + i + 2].y, fromMesh.vertices[i + 5].z));

            }
        }


        public void MorphFitDown(MeshDraft fromMesh, MeshDraft toMesh)
        {
            int length = toMesh.normals.Count;
            int row = (vert_x * 6);

            for (int i = 0; i < length; i += row)
            {
                fromMesh.vertices[i] = (new Vector3(fromMesh.vertices[i].x, toMesh.vertices[i + row - 1].y, fromMesh.vertices[i].z));
                fromMesh.vertices[i + 1] = (new Vector3(fromMesh.vertices[i + 1].x, toMesh.vertices[i + row - 2].y, fromMesh.vertices[i + 1].z));
                fromMesh.vertices[i + 3] = (new Vector3(fromMesh.vertices[i + 3].x, toMesh.vertices[i + row - 1].y, fromMesh.vertices[i + 3].z));
            }
        }

        public void UpdateVerticies(GameObject terrain, int idx)
        {

            terrain.GetComponent<MeshFilter>().mesh = meshes[idx];
            terrain.GetComponents<MeshCollider>()[0].sharedMesh = meshes[idx];
            //terrain.GetComponents<MeshCollider>()[1].sharedMesh = meshes[idx];
            terrain.GetComponent<ChunkCollider>().terrain_draft = drafts[idx];
            //Debug.Log("update" + idx);
        }
        public void GetNewPlane(GameObject terrain, int idx)
        {
            int rand = Random.Range(1, 10);
            var temp = pregen_graphs[rand];
            terrain.GetComponent<ChunkCollider>().terrain_draft = temp;
            terrain.GetComponent<MeshFilter>().mesh = temp.ToMesh();
            terrain.GetComponents<MeshCollider>()[0].sharedMesh = temp.ToMesh();
            terrain.GetComponents<MeshCollider>()[1].sharedMesh = temp.ToMesh();
        }
        public GameObject GetPlane()
        {
            int rand = Random.Range(1, pre_terrains.Count);
            var temp = pre_terrains[rand];
            pre_terrains.Remove(temp);
            return temp;
        }
        public void SendPlane(GameObject out_p)
        {
            out_p.transform.position = pre_terrains[0].transform.position;
            pre_terrains.Add(out_p);
        }
        public void UpdateDraft(GameObject terrain, MeshDraft memberMesh)
        {
            terrain.GetComponent<ChunkCollider>().terrain_draft = memberMesh;
        }

        public void UpdateObject(GameObject terrain)
        {

            Vector3 [] verts = terrain.GetComponent<ChunkCollider>().terrain_draft.vertices.ToArray();

            terrain.GetComponent<MeshFilter>().mesh.vertices = verts;
            terrain.GetComponent<MeshCollider>().sharedMesh.vertices = verts;
           // terrain.GetComponents<MeshCollider>()[1].sharedMesh.vertices = verts;
            terrain.GetComponent<MeshFilter>().mesh.UploadMeshData(false);

        }
      
        

    }


}
 