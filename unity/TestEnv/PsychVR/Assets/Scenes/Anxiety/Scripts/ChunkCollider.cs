using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChunkCollider : MonoBehaviour {

    public GameObject terrain_manager;
    public GameObject[] list = new GameObject[9];
    public ProceduralToolkit.MeshDraft terrain_draft;
    public Mesh terrain_mesh;
    public Vector3[] dir_list = { Vector3.right, -Vector3.right,Vector3.forward, -Vector3.forward,
        Vector3.right + Vector3.forward, Vector3.right - Vector3.forward, -Vector3.right + Vector3.forward , -Vector3.right - Vector3.forward };
    
	// Use this for initialization
	public void Load ()
    {
        list[0] = FindTerrain(this.transform.position + new Vector3(0, 20, 0), Vector3.right);
        list[1] = FindTerrain(this.transform.position + new Vector3(0, 20, 0), -Vector3.right);
        list[2] = FindTerrain(this.transform.position + new Vector3(0, 20, 0), Vector3.forward);
        list[3] = FindTerrain(this.transform.position + new Vector3(0, 20, 0), -Vector3.forward);

        list[4] = FindTerrain(this.transform.position + new Vector3(0, 20, 0), Vector3.right + Vector3.forward);
        list[5] = FindTerrain(this.transform.position + new Vector3(0, 20, 0), Vector3.right - Vector3.forward);
        list[6] = FindTerrain(this.transform.position + new Vector3(0, 20, 0), -Vector3.right + Vector3.forward);
        list[7] = FindTerrain(this.transform.position + new Vector3(0, 20, 0), -Vector3.right  - Vector3.forward);
    }

    private GameObject FindTerrain(Vector3 pos, Vector3 dir)
    {
        //Debug.Log("count");
        Ray check = new Ray(pos, dir);
        RaycastHit test;
        float mag = Mathf.Sqrt(100.0f * 100.0f + 100.0f * 100.0f);
        Physics.Raycast(check, out test, mag);
        if (test.collider == null)
        {
            //Debug.Log("nada " + this.transform.name + " " + dir+" "+pos + " "+dir*mag);

        }
        else
        {
            //Debug.Log(this.transform.name + " " + test.collider.transform.name + " " + dir);
            return test.collider.transform.parent.gameObject;
        }
        
        return null;
        
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggered" + gameObject.transform.name);
        //Debug.Log("moving to " + this.transform.position + this.gameObject.transform.name +  other.gameObject.transform.name);
        //terrain_manager.GetComponent<PlaneRepeat>().setPlane(this.gameObject);s
        if(this.gameObject.transform.name == "TerrainRenderer7")
            terrain_manager.GetComponent<PlaneRepeat>().SetGrid7();
        else if (this.gameObject.transform.name == "TerrainRenderer3")
            terrain_manager.GetComponent<PlaneRepeat>().SetGrid3();
        else if (this.gameObject.transform.name == "TerrainRenderer5")
            terrain_manager.GetComponent<PlaneRepeat>().SetGrid5();
        else if (this.gameObject.transform.name == "TerrainRenderer1")
            terrain_manager.GetComponent<PlaneRepeat>().SetGrid1();
        else if (this.gameObject.transform.name == "TerrainRenderer6")
            terrain_manager.GetComponent<PlaneRepeat>().SetGrid6();
        else if (this.gameObject.transform.name == "TerrainRenderer0")
            terrain_manager.GetComponent<PlaneRepeat>().SetGrid0();
        else if (this.gameObject.transform.name == "TerrainRenderer8")
            terrain_manager.GetComponent<PlaneRepeat>().SetGrid8();
        else if (this.gameObject.transform.name == "TerrainRenderer2")
            terrain_manager.GetComponent<PlaneRepeat>().SetGrid2();
    }

    // Update is called once per frame
    void Update ()
    {
        
	}
}
