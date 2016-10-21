using UnityEngine;
using System.Collections;

public class PlaneRepeat : MonoBehaviour {

    // Use this for initialization
    public Transform char_position;
    public GameObject plane;
    private GameObject[] plane_list;
    public GameObject player;
    public float scale = 10;
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
        Quaternion rot = plane.transform.rotation;
        for(int i = 0; i < GRID_X; i++)
        {
            gz = -1;
            for (int j = 0; j < GRID_Z; j++)
            {
                GameObject.Instantiate(plane, new Vector3(pos_x + size_x * gx, 0, pos_z + size_z * gz), rot);
                gz++;
            }
            gx++;
        }
        
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
