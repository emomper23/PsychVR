using UnityEngine;
using System.Collections;

public class ObjectHeight : MonoBehaviour {

    public GameObject parent_plane;
    public float height_diff = 0;
    // Use this for initialization
    void Awake ()
    {
      
        Vector3 tree_pos = this.transform.position;
        Ray check = new Ray(tree_pos, Vector3.down);
        RaycastHit test;
        Physics.Raycast(check, out test,100.0f);
        //Mathf.Exp
        if (test.collider == null)
        {
           // Debug.Log("No collider"+ this.gameObject.transform.position);
            //GameObject.Destroy(this.gameObject);
            return;
        }

        //  Debug.Log(test.collider.gameObject.transform.name);
        //Physics.IgnoreCollision(parent_plane.GetComponent<BoxCollider>());
        //test.collider.
        this.transform.position = tree_pos - new Vector3(0, test.distance - height_diff, 0);

    }

    // Update is called once per frame
    void Update ()
    {
	
	}
}
