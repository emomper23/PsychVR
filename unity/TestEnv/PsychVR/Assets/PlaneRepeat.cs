using UnityEngine;
using System.Collections;

public class PlaneRepeat : MonoBehaviour {

    // Use this for initialization
    public Transform char_position;
    public GameObject plane;
    private GameObject[] plane_list = new GameObject[9];
    public GameObject player;
    public float scale = 100;
    public float GRID_X = 3;
    public float GRID_Z = 3;
    private string old_name;
 
	void Start ()
    {
        makePlanes();

    }
    void makePlanes()
    {

      //  Debug.Log("making planes for " + plane);   
        float size_x = plane.GetComponent<Renderer>().bounds.size.x;
        float size_z = plane.GetComponent<Renderer>().bounds.size.z;
        float pos_x = plane.transform.position.x;
        float pos_z = plane.transform.position.z;
        
        float gx = -1;
        float gz = -1;
        Quaternion rot = plane.transform.rotation;
        

        for (int i = 0; i < GRID_X; i++)
        {
            gz = -1;
            for (int j = 0; j < GRID_Z; j++)
            {                   
                plane_list[i * 3 + j] = (GameObject)GameObject.Instantiate(plane, new Vector3(pos_x + size_x * gx, 0, pos_z + size_z * gz), rot);
                ((GameObject)plane_list[i * 3 + j]).transform.name = "TerrainRenderer"+(i*3+j);

                gz++;
            }
            gx++;
            

        }
        GameObject.Destroy(plane);
    }
    public void setPlane(GameObject obj)
    {
        if (obj.transform.name == "First Person Controller")
        {
            return;
        }
       foreach (GameObject p in plane_list)
        {
            if (p && (p.transform.name != obj.transform.name))
            {
                GameObject.Destroy(p);
            }
        }
        plane = obj;
        makePlanes();
          
       
    }
	// Update is called once per frame
	void Update ()
    {
	
	}
}
