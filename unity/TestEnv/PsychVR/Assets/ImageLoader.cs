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
		screen.GetComponent<Renderer>().material.mainTexture = (Texture2D)list[number % list.Length];
    }
	
	// Update is called once per frame
	void Update ()
    { 
        if (Input.GetKeyDown(KeyCode.RightArrow ))
        {
			Next();
        }
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			Previous();
		}
    }

	public void Next ()
	{
		if (number != list.Length - 1)
		{
			number++;
		}
		else
		{
			number = 0;
		}
		screen.GetComponent<Renderer>().material.mainTexture = (Texture2D)list[number % list.Length];

	}

	public void Previous()
	{
		if (number != 0)
		{
			number--;
		}
		else
		{
			number = list.Length - 1;
		}
		screen.GetComponent<Renderer>().material.mainTexture = (Texture2D)list[number % list.Length];

	}
}
