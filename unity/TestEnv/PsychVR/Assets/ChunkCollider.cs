using UnityEngine;
using System.Collections;

public class ChunkCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
    }

    // Update is called once per frame
    void Update ()
    {
	}
}
