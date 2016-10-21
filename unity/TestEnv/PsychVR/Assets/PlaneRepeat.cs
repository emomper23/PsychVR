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

	void Start ()
    {
        float size_x = plane.GetComponent<Renderer>().bounds.size.x;
        float size_z = plane.GetComponent<Renderer>().bounds.size.z;
        float pos_x = plane.transform.position.x;
        float pos_z = plane.transform.position.z;

        float gx = -1;
        float gz = -1;
        foreach(GameObject p in plane_list)
        {
            GameObject.Destroy(p);
        }
        Quaternion rot = plane.transform.rotation;
        for(int i = 0; i < GRID_X; i++)
        {
            gz = -1;
            for (int j = 0; j < GRID_Z; j++)
            {
                plane.transform.name = "plane";
                plane_list[i * 3 + j] = (GameObject)GameObject.Instantiate(plane, new Vector3(pos_x + size_x * gx, 0, pos_z + size_z * gz), rot);
                
                gz++;
            }
            gx++;
        }
        
	}
    public void setPlane(GameObject obj)
    {
        plane = obj;
        Start();
    }
	// Update is called once per frame
	void Update ()
    {
	
	}
}
