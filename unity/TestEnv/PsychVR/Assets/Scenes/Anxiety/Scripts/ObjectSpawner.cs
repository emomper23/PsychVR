using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour {

    public GameObject[] tree_list;
    public GameObject rock;
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


        //((GameObject)temp).GetComponent<ColorSetter>().
        //((GameObject)temp).GetComponent<ColorSetter>().





        GameObject mnt = (GameObject)GameObject.Instantiate(mountain, this.transform.position - new Vector3(diff_x, -20, diff_z), this.transform.rotation);
        mnt.GetComponent<ObjectHeight>().parent_plane = this.gameObject;
        mnt.GetComponent<ObjectHeight>().enabled = true;
        Vector3 scale_vec = new Vector3(Random.Range(0.5f, 1f), Random.Range(0.3f, 3f), Random.Range(0.5f, 1f));
        Vector3 scale = ((GameObject)mnt).transform.GetChild(0).localScale;
        scale.Scale(scale_vec);
        mnt.GetComponentInChildren<ObjectColorSetter>().gradient1 = gradient1;
        mnt.GetComponentInChildren<ObjectColorSetter>().gradient2 = gradient2;
        mnt.GetComponentInChildren<ObjectColorSetter>().Load();
        ((GameObject)mnt).transform.GetChild(0).localScale = scale;
        //((GameObject)temp2).transform.Translate(new Vector3(0, -1, 0));
        obj_list.Add((GameObject)mnt);
        Random.Range(0, 10);
        makeForest(14);
        makeRocks(7);
    }
    private void makeForest(int num_trees)
    {

        for (int i = 0; i < num_trees; i++)
        {
            int idx = Random.Range(0, 4);
            float diff_x = Random.Range(-100, 100);
            float diff_z = Random.Range(-100, 100);
            GameObject temp = (GameObject)GameObject.Instantiate(tree_list[idx%2], this.transform.position - new Vector3(diff_x, -20, diff_z), this.transform.rotation);

            Vector3 scale_vec = new Vector3(Random.Range(1.7f, 1.2f), Random.Range(0.7f, 2f), Random.Range(.7f, 1.2f));
            Vector3 scale = ((GameObject)temp).transform.GetChild(0).localScale;
            scale.Scale(scale_vec);
            temp.GetComponentInChildren<MeshRenderer>().enabled = true;
            ((GameObject)temp).transform.GetChild(0).localScale = scale;
            temp.GetComponent<ObjectHeight>().parent_plane = this.gameObject;
            temp.GetComponent<ObjectHeight>().enabled = true;
            obj_list.Add(temp);
        }      
    }
    private void makeRocks(int num_trees)
    {

        for (int i = 0; i < num_trees; i++)
        {
            int idx = Random.Range(0, 100);
            float diff_x = Random.Range(-100, 100);
            float diff_z = Random.Range(-100, 100);
            GameObject temp1 = (GameObject)GameObject.Instantiate(rock, this.transform.position - new Vector3(diff_x, -20, diff_z), this.transform.rotation);
            temp1.GetComponent<MeshRenderer>().enabled = true;
            temp1.GetComponent<ObjectHeight>().parent_plane = this.gameObject;
            temp1.GetComponent<ObjectHeight>().enabled = true;
            obj_list.Add(temp1);

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
