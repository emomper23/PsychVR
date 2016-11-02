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
        Object[] temp = new Object[list.Length];
        for (int i = 0; i < list.Length; i++)
        {
            foreach (Object o in list)
            {
                if (o.name == ("img-" + i))
                {
                    temp[i] = o;
                }
            }
        }

        list = temp;
		screen.GetComponent<Renderer>().material.mainTexture = (Texture2D)list[number % list.Length];
        Debug.Log(list[number % list.Length].name);
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
        Debug.Log(list[number % list.Length].name);

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
