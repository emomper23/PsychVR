using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour {

    public GameObject[] tree_list; 
    public GameObject[] rock_list;
    public GameObject mountain;
    public List<GameObject> obj_list;
    public Gradient gradient1;
    public Gradient gradient2;
    // Use this for initialization
    public void Start()
    {

    }

	public void Load ()
    {
        float diff_x = Random.Range(-100, 100);
        float diff_z = Random.Range(-100, 100);
        GameObject temp = (GameObject)GameObject.Instantiate(mountain, this.transform.position - new Vector3(diff_x, -20, diff_z), this.transform.rotation);
        Vector3 scale_vec = new Vector3(Random.Range(0.5f, 1f), Random.Range(0.3f, 3f), Random.Range(0.5f, 1f));
        Vector3 scale = ((GameObject)temp).transform.GetChild(0).localScale;
        scale.Scale(scale_vec);
       // temp.GetComponent<ObjectHeight>().height_diff = temp.GetComponent<ObjectHeight>(;
        temp.GetComponent<ObjectHeight>().parent_plane = this.gameObject;
        temp.GetComponent<ObjectHeight>().enabled = true;
       
        temp.GetComponentInChildren<ObjectColorSetter>().gradient1 = gradient1;
        temp.GetComponentInChildren<ObjectColorSetter>().gradient2 = gradient2;
        temp.GetComponentInChildren<ObjectColorSetter>().LoadMountain();
        ((GameObject)temp).transform.GetChild(0).localScale = scale;
        //((GameObject)temp2).transform.Translate(new Vector3(0, -1, 0));
        obj_list.Add((GameObject)temp);
        Random.Range(0, 10);
        makeForest(14);
        makeRocks(7);
    }
    private void makeForest(int num_trees)
    {

        for (int i = 0; i < num_trees; i++)
        {
            int idx = Random.Range(0, 100);
            float diff_x = Random.Range(-100, 100);
            float diff_z = Random.Range(-100, 100);
            GameObject temp = (GameObject)GameObject.Instantiate(tree_list[idx%tree_list.Length], this.transform.position - new Vector3(diff_x, -20, diff_z), this.transform.rotation);
            Vector3 scale_vec = new Vector3(Random.Range(.5f, 3f), Random.Range(0.7f, 2f), Random.Range(.5f, 3f));
            Vector3 scale = ((GameObject)temp).transform.localScale;
            scale.Scale(scale_vec);
            temp.transform.localScale = scale;
            temp.GetComponent<ObjectHeight>().parent_plane = this.gameObject;
            temp.GetComponent<ObjectHeight>().enabled = true;
            temp.GetComponentInChildren<ObjectColorSetter>().gradient1 = gradient1;
            temp.GetComponentInChildren<ObjectColorSetter>().gradient2 = gradient2;
            temp.GetComponentInChildren<ObjectColorSetter>().LoadTree(idx%2);
            obj_list.Add(temp);
        }      
    }
    private void makeRocks(int rocks)
    {

        for (int i = 0; i < rocks; i++)
        {
            int idx = Random.Range(0, 100);
            float diff_x = Random.Range(-100, 100);
            float diff_z = Random.Range(-100, 100);
            GameObject temp = (GameObject)GameObject.Instantiate(rock_list[idx%rock_list.Length], this.transform.position - new Vector3(diff_x, -20, diff_z), this.transform.rotation);
            temp.GetComponent<MeshRenderer>().enabled = true;
            temp.GetComponent<ObjectHeight>().parent_plane = this.gameObject;
            temp.GetComponent<ObjectHeight>().enabled = true;
            Vector3 scale_vec = new Vector3(Random.Range(.2f, 4f), Random.Range(0.4f, 2f), Random.Range(.2f, 2f));
            Vector3 scale = ((GameObject)temp).transform.localScale;
            scale.Scale(scale_vec);
            temp.transform.localScale = scale;
            temp.GetComponent<ObjectColorSetter>().gradient1 = gradient1;
            temp.GetComponent<ObjectColorSetter>().gradient2 = gradient2;
            temp.GetComponent<ObjectColorSetter>().LoadRock();
            obj_list.Add(temp);

        }
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
