using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour {

    public GameObject tree;
	// Use this for initialization
	void Awake ()
    {
        float diff_x = Random.Range(-50, 50);
        float diff_z = Random.Range(-50, 50);
        GameObject.Instantiate(tree,this.transform.position - new Vector3(diff_x,-20,diff_z),this.transform.rotation);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
