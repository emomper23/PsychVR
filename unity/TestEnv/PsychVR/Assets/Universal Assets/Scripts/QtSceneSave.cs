using UnityEngine;
using System.Collections;
using SimpleJSON;
using System;
using System.IO;

public class QtSceneSave : MonoBehaviour {
    // Use this for initialization
    private string json_text;
    public GameObject heights;
	void Start ()
    {
        SaveHeights(7);
    }
    void save()
    {
        
    }
    public void SaveHeights(float max_height)
    {
        json_text = this.gameObject.GetComponent<QtSceneLoad>().getJSON();
        var data = JSON.Parse(json_text);
        Debug.Log(data[1]["Heights"]);
        int current_run = 0;
        float building_height = 0;
        building_height = heights.GetComponent<HeightPicker>().heights[UnityEngine.PlayerPrefs.GetInt("BuildingNum")];
        foreach (JSONNode run in data[1]["Heights"]["runs"].AsArray)
        {
            Debug.Log(run);
            current_run = Int16.Parse(run["run"]) - 1;
        }
        Debug.Log("CURRENT RUN IS " + current_run);
        data[1]["Heights"]["runs"][current_run].Add("maxHeight", new JSONData(building_height));
        data[1]["Heights"]["runs"][current_run].Add("height", new JSONData(max_height));
        data[1]["Heights"]["runs"][current_run].Add("time", new JSONData(Time.timeSinceLevelLoad));
        Debug.Log(data);

        var output_file = File.CreateText(this.gameObject.GetComponent<QtSceneLoad>().m_path);
        output_file.Write(data.ToString());
        output_file.Close();
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
