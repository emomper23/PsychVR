using UnityEngine;
using System.Collections;

public class ImageLoader : MonoBehaviour {

    public string prefix;
    public int number;
    private Object[] list;
    public GameObject screen;

	// Use this for initialization
	void Start () {
        list = Resources.LoadAll("Textures");
    }
	
	// Update is called once per frame
	void Update ()
    { 
        if (Input.GetKeyDown(KeyCode.N))
        {
            number++;
        }
        screen.GetComponent<Renderer>().material.mainTexture = (Texture2D)list[number % list.Length];

    }
}
