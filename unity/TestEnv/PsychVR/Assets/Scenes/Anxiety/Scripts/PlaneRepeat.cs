using UnityEngine;
using System.Collections;

public class PlaneRepeat : MonoBehaviour {

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
    enum directions
    {
           NORTH = 0,
           SOUTH,
           EAST,
           WEST,
           NE,
           NW,
           SE,
           SW
    }
	void Start ()
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
        float size_x = plane.GetComponent<Renderer>().bounds.size.x ;
        float size_z = plane.GetComponent<Renderer>().bounds.size.z;
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
        else
        {
            ChunkCollider chunk = plane.GetComponent<ChunkCollider>();
            GameObject[] temp_list = new GameObject[8];
            for (int i = 0; i < 8; i++)
            {
                if (chunk.list[i] == null)
                {
                    Vector3 dist = new Vector3(size_x, 0, size_z);
                    dist.Scale(chunk.dir_list[i]);

                    temp_list[i] = (GameObject)Instantiate(orig_plane, plane.transform.position + dist, plane.transform.rotation);
                    terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().GetNewPlane(temp_list[i], i);
                    chunk.list[i] = temp_list[i];
                    temp_list[i].transform.name = "TerrainRenderer" + count;
                    count++;


                }
            }


            for (int i = 0; i < 8; i++)
            {
                if (temp_list[i] != null)
                {
                    temp_list[i].GetComponent<ChunkCollider>().Load();
                    temp_list[i].GetComponent<ObjectSpawner>().Load();

                }
            }
            //NORTH CASE DONE
            if (temp_list[(int)directions.NORTH] != null && temp_list[(int)directions.NE] != null && temp_list[(int)directions.NW] != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (temp_list[i] != null)
                    {
                        ChunkCollider temp_obj = temp_list[i].GetComponent<ChunkCollider>();
                     //   Debug.Log("List at" + i + " " + temp_obj.list[1] + " " + temp_obj);
                        

                            terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().MorphFitDown(temp_obj.terrain_draft,
                                                                                                                             temp_obj.list[(int)directions.SOUTH].GetComponent<ChunkCollider>().terrain_draft);
                            terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().UpdateDraft(temp_list[i], temp_obj.terrain_draft);
                        
                    }
                    //Debug.Log("new plane"+ plane.transform.name);   
                }
                for (int i = 0; i < 8; i++)
                {
                    if (temp_list[i] != null)
                    {
                        ChunkCollider temp_obj = temp_list[i].GetComponent<ChunkCollider>();
                      //  Debug.Log("List at" + i + " " + temp_obj.list[1] + " " + temp_obj);
                        if (i != 5 )
                        {
                            terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().MorphFitRight(temp_obj.terrain_draft,
                                                                                                                             temp_obj.list[(int)directions.WEST].GetComponent<ChunkCollider>().terrain_draft);
                            terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().UpdateDraft(temp_list[i], temp_obj.terrain_draft);
                        }
                        
                    }
                    //Debug.Log("new plane"+ plane.transform.name);   
                }
            }
            //SOUTH CASE DONE
            if (temp_list[(int)directions.SOUTH] != null && temp_list[(int)directions.SE] != null && temp_list[(int)directions.SW] != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (temp_list[i] != null)
                    {
                        ChunkCollider temp_obj = temp_list[i].GetComponent<ChunkCollider>();
                        //Debug.Log("List at" + i + " " + temp_obj.list[1] + " " + temp_obj);
                       
                        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().MorphFitDown(temp_obj.list[(int)directions.NORTH].GetComponent<ChunkCollider>().terrain_draft, temp_obj.terrain_draft);
                        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().UpdateDraft(temp_obj.list[(int)directions.NORTH], temp_obj.list[(int)directions.NORTH].GetComponent<ChunkCollider>().terrain_draft);

                    }
                    //Debug.Log("new plane"+ plane.transform.name);   
                }
                for (int i = 0; i < 8; i++)
                {
                    if (temp_list[i] != null)
                    {
                        ChunkCollider temp_obj = temp_list[i].GetComponent<ChunkCollider>();
                       // Debug.Log("List at" + i + " " + temp_obj.list[1] + " " + temp_obj);
                        if (i != 6)
                        {
                            terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().MorphFitRight(temp_obj.list[(int)directions.EAST].GetComponent<ChunkCollider>().terrain_draft,temp_obj.terrain_draft
                                                                                                                             );
                            terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().UpdateDraft(temp_obj.list[(int)directions.EAST], temp_obj.list[(int)directions.EAST].GetComponent<ChunkCollider>().terrain_draft);
                        }

                    }
                    //Debug.Log("new plane"+ plane.transform.name);   
                }
            }
            //East Case DONE
            if (temp_list[(int)directions.EAST] != null && temp_list[(int)directions.NE] != null && temp_list[(int)directions.SE] != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (temp_list[i] != null)
                    {
                        ChunkCollider temp_obj = temp_list[i].GetComponent<ChunkCollider>();
                      //  Debug.Log("List at" + i + " " + temp_obj.list[1] + " " + temp_obj);
                        if (i != (int)directions.SE)
                        {

                            terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().MorphFitDown(temp_obj.terrain_draft,
                                                                                                                             temp_obj.list[(int)directions.SOUTH].GetComponent<ChunkCollider>().terrain_draft);
                            terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().UpdateDraft(temp_list[i], temp_obj.terrain_draft);
                        }
                        
                    }
                    //Debug.Log("new plane"+ plane.transform.name);   
                }
                for (int i = 0; i < 8; i++)
                {
                    if (temp_list[i] != null)
                    {
                        ChunkCollider temp_obj = temp_list[i].GetComponent<ChunkCollider>();
                       // Debug.Log("List at" + i + " " + temp_obj.list[1] + " " + temp_obj);


                        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().MorphFitRight(temp_obj.terrain_draft, temp_obj.list[(int)directions.WEST].GetComponent<ChunkCollider>().terrain_draft);
                            terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().UpdateDraft(temp_list[i], temp_obj.terrain_draft);
                        

                    }
                    //Debug.Log("new plane"+ plane.transform.name);   
                }
            }
            //West Case TODO
            if (temp_list[(int)directions.WEST] != null && temp_list[(int)directions.NW] != null && temp_list[(int)directions.SW] != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (temp_list[i] != null)
                    {
                        ChunkCollider temp_obj = temp_list[i].GetComponent<ChunkCollider>();
                      //  Debug.Log("List at" + i + " " + temp_obj.list[1] + " " + temp_obj);
                        if (i != (int)directions.SW)
                        {

                            terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().MorphFitDown(temp_obj.terrain_draft,
                                                                                                                             temp_obj.list[(int)directions.SOUTH].GetComponent<ChunkCollider>().terrain_draft);
                            terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().UpdateDraft(temp_list[i], temp_obj.terrain_draft);
                        }

                    }
                    //Debug.Log("new plane"+ plane.transform.name);   
                }
                for (int i = 0; i < 8; i++)
                {
                    if (temp_list[i] != null)
                    {
                        ChunkCollider temp_obj = temp_list[i].GetComponent<ChunkCollider>();
                      //  Debug.Log("List at" + i + " " + temp_obj.list[1] + " " + temp_obj);


                        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().MorphFitRight(temp_obj.terrain_draft, temp_obj.list[(int)directions.EAST].GetComponent<ChunkCollider>().terrain_draft);
                        terrainGen.GetComponent<ProceduralToolkit.Examples.UI.LowPolyTerrainGeneratorUI>().UpdateDraft(temp_list[i], temp_obj.terrain_draft);


                    }
                    //Debug.Log("new plane"+ plane.transform.name);   
                }
            }

        }
       
    }
    public void setPlane(GameObject obj)
    {
        if (obj.name == plane.name)
            return;
        plane = obj;
        makePlanes();
          
       
    }
	// Update is called once per frame
	void Update ()
    {
	
	}
}
