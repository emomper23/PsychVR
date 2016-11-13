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
	public Boolean day;
    public string session_id;
    private string json_text;
	// Use this for initialization
	void Start ()
    {
        string text = System.IO.File.ReadAllText(m_path);
        var data = JSON.Parse(text);
       // Debug.Log(data[1]["Calm"]);
       // Debug.Log(data[1]["Heights"]);
       // Debug.Log(data[1]["Social"]);
        //Debug.Log(data[1]["Heights"]["Color"]);


        //do all write the same at once?
		PlayerPrefs.SetString("SkinColor", skin_color);//data[1]["Calm"]["Settings"]["Color"]);
        PlayerPrefs.SetInt("Day", Int16.Parse(data[1]["Heights"]["Settings"]["Day"]));        
        PlayerPrefs.SetInt("BuildingNum", Int16.Parse(data[1]["Heights"]["Settings"]["Building"]));
   

    }
    public string getJSON
        ()
    {
        return json_text;
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
