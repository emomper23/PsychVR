using UnityEngine;
using System.Collections;
using System;
using System.IO;


public class QtSceneLoad : MonoBehaviour {

    public string m_path;
    public int m_day_flag;
    public int m_config_id;
    public string skin_color;
	// Use this for initialization
	void Start ()
    {
        /*
        string args = System.Environment.CommandLine;
        INIParser parser = new INIParser();
        parser.Open(m_path);
        m_day_flag = parser.ReadValue("%General", "day", 0);
        m_config_id = parser.ReadValue("%General", "config_id", 0);
        if (m_day_flag == 1)
        {
            //TODO Day night rotation here, I can't find it.
        }
        if (m_config_id == 0)
        {
            //config 0 positions
        }
        else if (m_config_id == 1)
        {
            //config 1 positions

        }*/
        
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
