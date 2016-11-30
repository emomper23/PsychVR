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
    public int scene_idx = 1;
    public int user_id = 0;

	// Use this for initialization
	void Start ()
    {
        Debug.Log(m_path);
        string text = System.IO.File.ReadAllText(m_path);
        json_text = text;
        //Debug.Log(text);
        var data = JSON.Parse(json_text);

        
        //Debug.Log(data[user_id]["Calm"]);
        //Debug.Log(data[user_id]["Heights"]);
        //Debug.Log(data[user_id]["Social"]);
        //Debug.Log(data[user_id]["Heights"]["Color"]);

        if (scene_idx == 0)
        {
            Debug.Log("Loading heights Settings");
            PlayerPrefs.SetString("SkinColor", data[user_id]["Heights"]["Settings"]["Color"]);
            PlayerPrefs.SetInt("Day", Int16.Parse(data[user_id]["Heights"]["Settings"]["Day"]));
            PlayerPrefs.SetInt("BuildingNum", Int16.Parse(data[user_id]["Heights"]["Settings"]["Building"]));
        }
        else if (scene_idx == 1)
        {
            Debug.Log("Loading speech Settings");
            PlayerPrefs.SetInt("NumberStudents", Int16.Parse(data[user_id]["Social"]["Settings"]["Number Students"]));
            PlayerPrefs.SetString("SkinColor", data[user_id]["Social"]["Settings"]["Color"]);
            PlayerPrefs.SetString("Powerpoint", data[user_id]["Social"]["Settings"]["Powerpoint"]+"/");
            PlayerPrefs.SetString("Animations", data[user_id]["Social"]["Settings"]["Animations"].ToString());
        }
        else if (scene_idx == 2)
        {
            Debug.Log("Loading terrain Settings");
            PlayerPrefs.SetString("SkinColor", data[user_id]["Anxiety"]["Settings"]["Color"]);
            PlayerPrefs.SetString("Song", data[user_id]["Anxiety"]["Settings"]["Song"]);
            PlayerPrefs.SetString ("Tree", data[user_id]["Anxiety"]["Settings"]["Tree"] );
            PlayerPrefs.SetString("Rock", data[user_id]["Anxiety"]["Settings"]["Rock"]);
        }
    }
    public string getJSON()
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
