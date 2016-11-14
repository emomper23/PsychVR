using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour {

    public GameObject tree;
    public GameObject rock;
    public List<GameObject> obj_list;
    public Gradient gradient1;
    public Gradient gradient2;
    // Use this for initialization
    public void Start()
    {

    }

	public void Load ()
    {
        float diff_x = Random.Range(-50, 50);
        float diff_z = Random.Range(-50, 50);
        float diff_x1 = Random.Range(-50, 50);
        float diff_z1 = Random.Range(-50, 50);
        
        Object temp = GameObject.Instantiate(tree,this.transform.position - new Vector3(diff_x,-20,diff_z),this.transform.rotation);
        ((GameObject)temp).GetComponentInChildren<MeshRenderer>().enabled = true;
        ((GameObject)temp).GetComponent<ObjectHeight>().enabled = true;
        ((GameObject)temp).GetComponent<ObjectHeight>().parent_plane = this.gameObject;
        //((GameObject)temp).GetComponent<ColorSetter>().
        //((GameObject)temp).GetComponent<ColorSetter>().

        Object temp1 = GameObject.Instantiate(rock, this.transform.position - new Vector3(diff_x1, -20, diff_z1), this.transform.rotation);
        ((GameObject)temp1).GetComponent<MeshRenderer>().enabled = true;
        ((GameObject)temp).GetComponent<ObjectHeight>().enabled = true;
        ((GameObject)temp1).GetComponent<ObjectColorSetter>().gradient1 = gradient1;
        ((GameObject)temp1).GetComponent<ObjectColorSetter>().gradient2 = gradient2;
        ((GameObject)temp1).GetComponent<ObjectColorSetter>().Load();
        obj_list.Add((GameObject)temp);
        obj_list.Add((GameObject)temp1);


    }

    public void Burn()
    {
        foreach (GameObject g in obj_list)
        {
            GameObject.Destroy(g);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
