using UnityEngine;
using System.Collections;

public class ChunkCollider : MonoBehaviour {

    public GameObject terrain_manager;
	// Use this for initialization
	void Start () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name + " from"+ this.transform.position);
       terrain_manager.GetComponent<PlaneRepeat>().setPlane(this.gameObject);
    }

    // Update is called once per frame
    void Update ()
    {
	}
}
