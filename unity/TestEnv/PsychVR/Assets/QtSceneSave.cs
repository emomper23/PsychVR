using UnityEngine;
using System.Collections;
using SimpleJSON;

public class QtSceneSave : MonoBehaviour {

    // Use this for initialization
    private string json_text;
	void Start ()
    {
        json_text = this.gameObject.GetComponent<QtSceneLoad>().getJSON();
	}
    void save()
    {
        var data = JSON.Parse(json_text);
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
