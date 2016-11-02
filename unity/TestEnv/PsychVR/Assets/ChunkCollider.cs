using UnityEngine;
using System.Collections;

public class ChunkCollider : MonoBehaviour {

    public GameObject terrain_manager;
    public GameObject name;
	// Use this for initialization
	void Start () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("moving to " + this.transform.position + this.gameObject.transform.name +  other.gameObject.transform.name);
        terrain_manager.GetComponent<PlaneRepeat>().setPlane(this.gameObject);
    }

    // Update is called once per frame
    void Update ()
    {
        
	}
}
