using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour {

    public GameObject tree;
    public GameObject rock;
	// Use this for initialization
	void Awake ()
    {
        float diff_x = Random.Range(-50, 50);
        float diff_z = Random.Range(-50, 50);
        float diff_x1 = Random.Range(-50, 50);
        float diff_z1 = Random.Range(-50, 50);
        Object temp = GameObject.Instantiate(tree,this.transform.position - new Vector3(diff_x,-20,diff_z),this.transform.rotation);
        ((GameObject)temp).GetComponentInChildren<MeshRenderer>().enabled = true;
        ((GameObject)temp).GetComponent<ObjectHeight>().enabled = true;

        Object temp1 = GameObject.Instantiate(rock, this.transform.position - new Vector3(diff_x1, -20, diff_z1), this.transform.rotation);
        ((GameObject)temp1).GetComponent<MeshRenderer>().enabled = true;
        ((GameObject)temp).GetComponent<ObjectHeight>().enabled = true;

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
