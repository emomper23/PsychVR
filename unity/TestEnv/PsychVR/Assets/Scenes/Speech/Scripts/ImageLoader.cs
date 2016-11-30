using UnityEngine;
using System.Collections;
using System.IO;

public class ImageLoader : MonoBehaviour {

    public string prefix;
    private string path; 
    public int number;
    public int total;
    public int loaded;
    private bool first = true;
    private Object[] list;
    public GameObject screen;

    // Use this for initialization
    void Start()
    {
        path = PlayerPrefs.GetString("Powerpoint");
        if (path == "")
        {
            Debug.Log("default ppt");
            list = Resources.LoadAll("Textures");

        }
        else
        {
            string url = "file:///" + path;
            Debug.Log(path);
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] info = dir.GetFiles("*.JPG");
            total = info.Length;
            list = new Object[total];
            foreach (FileInfo f in info)
            {
                WWW www = new WWW(url + f.Name);
                StartCoroutine(WaitForRequest(www));
            }

        }
    }
    IEnumerator WaitForRequest(WWW www)
    {

        yield return www;



        // check for errors
        if (www.error == null)
        {
            //Debug.Log("WWW Ok!: "+ www.url);
            Texture temp =  www.texture;
            temp.name = Path.GetFileName(www.url);
            list[loaded] = temp;
            loaded++;
        }
        else
        {
            //Debug.Log(www.url);
            Debug.Log("WWW Error: " + www.error);
        }
    }
    void sort()
    {
        Object[] temp = new Object[list.Length];
        for (int i = 1; i < list.Length + 1; i++)
        {
            foreach (Object o in list)
            {
                if (o.name == ("Slide" + i  + ".JPG"))
                {
                    temp[i-1] = o;
                }
                else
                {
                    //Debug.Log(o.name+ "=="+("Slide" + i + ".JPG"));
                }
            }
        }

        list = temp;
        Debug.Log("done!!" + total);
        foreach (Object o in list)
        {
            //Debug.Log(o);
        }
        screen.GetComponent<Renderer>().material.mainTexture = (Texture2D)list[number % list.Length];
        //Debug.Log(list[number % list.Length].name);
    }
    // Update is called once per frame
    void Update ()
    {
        if (first && loaded != 0 && loaded == total)
        {
            first = false;
            sort();
            
        }
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
