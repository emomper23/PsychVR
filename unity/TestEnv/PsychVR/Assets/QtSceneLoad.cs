using UnityEngine;
using System.Collections;
using System;
using System.IO;
using SimpleJSON;

public class QtSceneLoad : MonoBehaviour {

    public string m_path;
    public int m_day_flag;
    public int m_config_id;
    public string skin_color;
	// Use this for initialization
	void Start ()
    {
        string text = System.IO.File.ReadAllText(m_path);
        var data = JSON.Parse(text);
        Debug.Log(data[0]["Calm"]);
        Debug.Log(data[0]["Heights"]);
        Debug.Log(data[0]["Social"]);


    }
    public string getSkinColor()
    {
        return skin_color;
    }
	// Update is called once per frame
	void Update ()
    {
	
	}
}
