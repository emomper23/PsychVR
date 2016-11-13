using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlaneRepeat : MonoBehaviour
{

    // Use this for initialization
    public Transform char_position;
    public GameObject plane;
    public GameObject orig_plane;
    private GameObject[] plane_list = new GameObject[9];
    public GameObject player;
    public GameObject terrainGen;
    public float scale = 100;
    public float GRID_X = 3;
    public float GRID_Z = 3;
    private string old_name;
    int count = 9;
    private bool firstTime = true;
    float size_x;
    float size_z;
    enum directions
    {
        NORTH = 0,
        SOUTH,
        WEST,
        EAST,
        NW,
        NE,
        SW,
        SE
    }
    public Vector3[] dir_list = { Vector3.right, -Vector3.right,Vector3.forward, -Vector3.forward,
        Vector3.right + Vector3.forward, Vector3.right - Vector3.forward, -Vector3.right + Vector3.forward , -Vector3.right - Vector3.forward };
    void Start()
    {
        orig_plane = (GameObject)Instantiate(plane, new Vector3(0, -100, 0), plane.transform.rotation);
        //orig_plane.GetComponent<ObjectSpawner>().tree  = plane.GetComponent<ObjectSpawner>().tree;
        //orig_plane.GetComponent<ObjectSpawner>().rock = plane.GetComponent<ObjectSpawner>().rock;
        orig_plane.transform.name = "og";
        makePlanes();


    }
    void makePlanes()
    {

        //  Debug.Log("making planes for " + plane);   
        size_x = plane.GetComponent<Renderer>().bounds.size.x;
        size_z = plane.GetComponent<Renderer>().bounds.size.z;
        float pos_x = plane.transform.position.x;
        float pos_z = plane.transform.position.z;

        float gx = -1;
        float gz = -1;
        Quaternion rot = plane.transform.rotation;

        if (firstTime)
        {
            for (int i = 0; i < GRID_X; i++)
            {
                gz = -1;
                for (int j = 0; j < GRID_Z; j++)
                {
                    plane_list[i * 3 + j] = (GameObject)GameObject.Instantiate(plane, new Vector3(pos_x + size_x * gx, 0, pos_z + size_z * gz), rot);
                    ((GameObject)plane_list[i * 3 + j]).transform.name = "TerrainRenderer" + (i * 3 + j);
                    // plane_list[i * 3 + j].GetComponent<ObjectSpawner>().tree = plane.GetComponent<ObjectSpawner>().tree;
                    // oriplane_list[i * 3 + j]..GetComponent<ObjectSpawner>().rock = plane.GetComponent<ObjectSpawner>().rock;
                    terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().UpdateVerticies((GameObject)plane_list[i * 3 + j], i * 3 + j);
                    gz++;
                    //((GameObject)plane_list[i * 3 + j]).GetComponent<MeshFilter>().mesh.RecalculateNormals();
                    //((GameObject)plane_list[i * 3 + j]).GetComponent<MeshFilter>().mesh.RecalculateBounds();

                }
                gx++;


            }
            plane.transform.Translate(new Vector3(0, -100, 0));
            plane.GetComponent<ChunkCollider>().enabled = false;
            plane.GetComponent<ObjectSpawner>().enabled = false;

            plane = plane_list[4];
            for (int i = 0; i < 9; i++)
            {
                ((GameObject)plane_list[i]).GetComponent<ChunkCollider>().Load();
                ((GameObject)plane_list[i]).GetComponent<ObjectSpawner>().Load();
            }

            firstTime = false;
        }
    }

    public void setPlane(GameObject obj)
    {
        if (obj.name == plane.name)
            return;
        plane = obj;
        makePlanes();


    }
    public void SetGrid7()
    {
        List<GameObject> out_list = new List<GameObject>();
        Vector3 pos = plane_list[7].transform.position;

        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[0]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[1]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[2]);

        plane_list[0] = plane_list[3];
        plane_list[1] = plane_list[4];
        plane_list[2] = plane_list[5];

        plane_list[3] = plane_list[6];
        plane_list[4] = plane_list[7];
        plane_list[5] = plane_list[8];

        Vector3 dist0 = new Vector3(size_x, 0, size_z);
        Vector3 dist1 = new Vector3(size_x, 0, size_z);
        Vector3 dist2 = new Vector3(size_x, 0, size_z);
        dist0.Scale(dir_list[(int)directions.NORTH]);
        dist1.Scale(dir_list[(int)directions.NE]);
        dist2.Scale(dir_list[(int)directions.NW]);
        plane_list[7] = SpawnPlane(pos + dist0);
        plane_list[6] = SpawnPlane(pos + dist1);
        plane_list[8] = SpawnPlane(pos + dist2);
        renamePlanes();

        StitchRight(plane_list[7], plane_list[6]);
        StitchRight(plane_list[8], plane_list[7]);


        StitchDown(plane_list[8], plane_list[5]);
        StitchDown(plane_list[7], plane_list[4]);
        StitchDown(plane_list[6], plane_list[3]);

    }

    public void SetGrid1()
    {
        Debug.Log("1 callback");
        List<GameObject> out_list = new List<GameObject>();
        Vector3 pos = plane_list[1].transform.position;

        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[6]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[7]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[8]);

        plane_list[8] = plane_list[5];
        plane_list[7] = plane_list[4];
        plane_list[6] = plane_list[3];

        plane_list[5] = plane_list[2];
        plane_list[4] = plane_list[1];
        plane_list[3] = plane_list[0];

        Vector3 dist0 = new Vector3(size_x, 0, size_z);
        Vector3 dist1 = new Vector3(size_x, 0, size_z);
        Vector3 dist2 = new Vector3(size_x, 0, size_z);
        dist0.Scale(dir_list[(int)directions.SW]);
        dist1.Scale(dir_list[(int)directions.SOUTH]);
        dist2.Scale(dir_list[(int)directions.SE]);
        plane_list[2] = SpawnPlane(pos + dist0);
        plane_list[1] = SpawnPlane(pos + dist1);
        plane_list[0] = SpawnPlane(pos + dist2);
        renamePlanes();

        StitchRight(plane_list[2], plane_list[1]);
        StitchRight(plane_list[1], plane_list[0]);


        StitchDown(plane_list[5], plane_list[2]);
        StitchDown(plane_list[4], plane_list[1]);
        StitchDown(plane_list[3], plane_list[0]);

    }

    public void SetGrid3()
    {
        List<GameObject> out_list = new List<GameObject>();
        Vector3 pos = plane_list[3].transform.position;

        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[2]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[5]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[8]);

        plane_list[2] = plane_list[1];
        plane_list[5] = plane_list[4];
        plane_list[8] = plane_list[7];

        plane_list[1] = plane_list[0];
        plane_list[4] = plane_list[3];
        plane_list[7] = plane_list[6];

        Vector3 dist0 = new Vector3(size_x, 0, size_z);
        Vector3 dist1 = new Vector3(size_x, 0, size_z);
        Vector3 dist2 = new Vector3(size_x, 0, size_z);
        dist0.Scale(dir_list[(int)directions.SE]);
        dist1.Scale(dir_list[(int)directions.EAST]);
        dist2.Scale(dir_list[(int)directions.NE]);
        plane_list[0] = SpawnPlane(pos + dist0);
        plane_list[3] = SpawnPlane(pos + dist1);
        plane_list[6] = SpawnPlane(pos + dist2);
        renamePlanes();

        StitchRight(plane_list[7], plane_list[6]);
        StitchRight(plane_list[4], plane_list[3]);
        StitchRight(plane_list[1], plane_list[0]);

        StitchDown(plane_list[6], plane_list[3]);
        StitchDown(plane_list[3], plane_list[0]);

    }

    public void SetGrid5()
    {
        List<GameObject> out_list = new List<GameObject>();
        Vector3 pos = plane_list[5].transform.position;

        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[6]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[3]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[0]);

        plane_list[6] = plane_list[7];
        plane_list[0] = plane_list[1];
        plane_list[3] = plane_list[4];

        plane_list[7] = plane_list[8];
        plane_list[4] = plane_list[5];
        plane_list[1] = plane_list[2];



        Vector3 dist0 = new Vector3(size_x, 0, size_z);
        Vector3 dist1 = new Vector3(size_x, 0, size_z);
        Vector3 dist2 = new Vector3(size_x, 0, size_z);
        dist0.Scale(dir_list[(int)directions.NW]);
        dist1.Scale(dir_list[(int)directions.WEST]);
        dist2.Scale(dir_list[(int)directions.SW]);
        plane_list[8] = SpawnPlane(pos + dist0);
        plane_list[5] = SpawnPlane(pos + dist1);
        plane_list[2] = SpawnPlane(pos + dist2);
        renamePlanes();

        StitchRight(plane_list[8], plane_list[7]);
        StitchRight(plane_list[5], plane_list[4]);
        StitchRight(plane_list[2], plane_list[1]);

        StitchDown(plane_list[8], plane_list[5]);
        StitchDown(plane_list[5], plane_list[2]);

    }

    public void SetGrid6()
    {
        List<GameObject> out_list = new List<GameObject>();
        Vector3 pos = plane_list[6].transform.position;

        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[8]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[5]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[2]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[1]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[0]);

        plane_list[2] = plane_list[4];
        plane_list[4] = plane_list[6];
        plane_list[1] = plane_list[3];
        plane_list[5] = plane_list[7];



        Vector3 dist0 = new Vector3(size_x, 0, size_z);
        Vector3 dist1 = new Vector3(size_x, 0, size_z);
        Vector3 dist2 = new Vector3(size_x, 0, size_z);
        Vector3 dist3 = new Vector3(size_x, 0, size_z);
        Vector3 dist4 = new Vector3(size_x, 0, size_z);
        dist0.Scale(dir_list[(int)directions.NW]);
        dist1.Scale(dir_list[(int)directions.NORTH]);
        dist2.Scale(dir_list[(int)directions.NE]);
        dist3.Scale(dir_list[(int)directions.EAST]);
        dist4.Scale(dir_list[(int)directions.SE]);

        plane_list[8] = SpawnPlane(pos + dist0);
        plane_list[7] = SpawnPlane(pos + dist1);
        plane_list[6] = SpawnPlane(pos + dist2);
        plane_list[3] = SpawnPlane(pos + dist3);
        plane_list[0] = SpawnPlane(pos + dist4);
        renamePlanes();

        StitchRight(plane_list[8], plane_list[7]);
        StitchRight(plane_list[7], plane_list[6]);
        StitchRight(plane_list[4], plane_list[3]);
        StitchRight(plane_list[1], plane_list[0]);

        StitchDown(plane_list[8], plane_list[5]);
        StitchDown(plane_list[7], plane_list[4]);
        StitchDown(plane_list[6], plane_list[3]);
        StitchDown(plane_list[3], plane_list[0]);

    }

    public void SetGrid0()
    {
        List<GameObject> out_list = new List<GameObject>();
        Vector3 pos = plane_list[0].transform.position;

        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[2]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[5]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[8]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[7]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[6]);

        plane_list[8] = plane_list[4];
        plane_list[4] = plane_list[0];
        plane_list[5] = plane_list[1];
        plane_list[7] = plane_list[3];



        Vector3 dist0 = new Vector3(size_x, 0, size_z);
        Vector3 dist1 = new Vector3(size_x, 0, size_z);
        Vector3 dist2 = new Vector3(size_x, 0, size_z);
        Vector3 dist3 = new Vector3(size_x, 0, size_z);
        Vector3 dist4 = new Vector3(size_x, 0, size_z);
        dist0.Scale(dir_list[(int)directions.NE]);
        dist1.Scale(dir_list[(int)directions.EAST]);
        dist2.Scale(dir_list[(int)directions.SE]);
        dist3.Scale(dir_list[(int)directions.SOUTH]);
        dist4.Scale(dir_list[(int)directions.SW]);

        plane_list[6] = SpawnPlane(pos + dist0);
        plane_list[3] = SpawnPlane(pos + dist1);
        plane_list[0] = SpawnPlane(pos + dist2);
        plane_list[1] = SpawnPlane(pos + dist3);
        plane_list[2] = SpawnPlane(pos + dist4);
        renamePlanes();

        StitchRight(plane_list[7], plane_list[6]);
        StitchRight(plane_list[4], plane_list[3]);
        StitchRight(plane_list[1], plane_list[0]);
        StitchRight(plane_list[2], plane_list[1]);

        StitchDown(plane_list[6], plane_list[3]);
        StitchDown(plane_list[3], plane_list[0]);
        StitchDown(plane_list[4], plane_list[1]);
        StitchDown(plane_list[5], plane_list[2]);

    }
    public void SetGrid8()
    {
        List<GameObject> out_list = new List<GameObject>();
        Vector3 pos = plane_list[8].transform.position;

        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[6]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[3]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[0]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[1]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[2]);

        plane_list[0] = plane_list[4];
        plane_list[4] = plane_list[8];
        plane_list[1] = plane_list[5];
        plane_list[3] = plane_list[7];




        Vector3 dist0 = new Vector3(size_x, 0, size_z);
        Vector3 dist1 = new Vector3(size_x, 0, size_z);
        Vector3 dist2 = new Vector3(size_x, 0, size_z);
        Vector3 dist3 = new Vector3(size_x, 0, size_z);
        Vector3 dist4 = new Vector3(size_x, 0, size_z);
        dist0.Scale(dir_list[(int)directions.SW]);
        dist1.Scale(dir_list[(int)directions.WEST]);
        dist2.Scale(dir_list[(int)directions.NW]);
        dist3.Scale(dir_list[(int)directions.NORTH]);
        dist4.Scale(dir_list[(int)directions.NE]);

        plane_list[2] = SpawnPlane(pos + dist0);
        plane_list[5] = SpawnPlane(pos + dist1);
        plane_list[8] = SpawnPlane(pos + dist2);
        plane_list[7] = SpawnPlane(pos + dist3);
        plane_list[6] = SpawnPlane(pos + dist4);
        renamePlanes();

        StitchRight(plane_list[2], plane_list[1]);
        StitchRight(plane_list[5], plane_list[4]);
        StitchRight(plane_list[8], plane_list[7]);
        StitchRight(plane_list[7], plane_list[6]);

        StitchDown(plane_list[5], plane_list[2]);
        StitchDown(plane_list[8], plane_list[5]);
        StitchDown(plane_list[7], plane_list[4]);
        StitchDown(plane_list[6], plane_list[3]);

    }
    public void SetGrid2()
    {
        List<GameObject> out_list = new List<GameObject>();
        Vector3 pos = plane_list[2].transform.position;

        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[8]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[7]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[6]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[3]);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().SendPlane(plane_list[0]);

        plane_list[6] = plane_list[4];
        plane_list[4] = plane_list[2];
        plane_list[7] = plane_list[5];
        plane_list[3] = plane_list[1];



        Vector3 dist0 = new Vector3(size_x, 0, size_z);
        Vector3 dist1 = new Vector3(size_x, 0, size_z);
        Vector3 dist2 = new Vector3(size_x, 0, size_z);
        Vector3 dist3 = new Vector3(size_x, 0, size_z);
        Vector3 dist4 = new Vector3(size_x, 0, size_z);
        dist0.Scale(dir_list[(int)directions.NW]);
        dist1.Scale(dir_list[(int)directions.WEST]);
        dist2.Scale(dir_list[(int)directions.SW]);
        dist3.Scale(dir_list[(int)directions.SOUTH]);
        dist4.Scale(dir_list[(int)directions.SE]);

        plane_list[8] = SpawnPlane(pos + dist0);
        plane_list[5] = SpawnPlane(pos + dist1);
        plane_list[2] = SpawnPlane(pos + dist2);
        plane_list[1] = SpawnPlane(pos + dist3);
        plane_list[0] = SpawnPlane(pos + dist4);
        renamePlanes();

        StitchRight(plane_list[8], plane_list[7]);
        StitchRight(plane_list[5], plane_list[4]);
        StitchRight(plane_list[2], plane_list[1]);
        StitchRight(plane_list[1], plane_list[0]);

        StitchDown(plane_list[8], plane_list[5]);
        StitchDown(plane_list[5], plane_list[2]);
        StitchDown(plane_list[4], plane_list[1]);
        StitchDown(plane_list[3], plane_list[0]);

    }
    public GameObject SpawnPlane(Vector3 pos)
    {
        GameObject plane = terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().GetPlane();
        plane.transform.position = pos;
        return plane;
    }
    public void StitchRight(GameObject fromTerrain, GameObject toTerrain)
    {
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().MorphFitRight(fromTerrain.GetComponent<ChunkCollider>().terrain_draft, toTerrain.GetComponent<ChunkCollider>().terrain_draft);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().UpdateDraft(fromTerrain, fromTerrain.GetComponent<ChunkCollider>().terrain_draft);
    }

    public void StitchDown(GameObject fromTerrain, GameObject toTerrain)
    {
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().MorphFitDown(fromTerrain.GetComponent<ChunkCollider>().terrain_draft, toTerrain.GetComponent<ChunkCollider>().terrain_draft);
        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().UpdateDraft(fromTerrain, fromTerrain.GetComponent<ChunkCollider>().terrain_draft);
    }
    public void renamePlanes()
    {
        for (int i = 0; i < 9; i++)
        {
            plane_list[i].transform.name = "TerrainRenderer" + i;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
