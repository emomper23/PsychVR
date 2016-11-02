using UnityEngine;
using System.Collections;

public class ObjectHeight : MonoBehaviour {

	// Use this for initialization
	void Awake ()
    {
        Vector3 tree_pos = this.transform.position;
        Ray check = new Ray(tree_pos, Vector3.down);
        RaycastHit test;
        Physics.Raycast(check, out test,100.0f);
        Debug.Log(test.collider.gameObject.transform.name);

        this.transform.position = tree_pos - new Vector3(0, test.distance, 0);

    }

    // Update is called once per frame
    void Update ()
    {
	
	}
}
