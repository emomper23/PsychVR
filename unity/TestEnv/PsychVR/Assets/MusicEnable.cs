using UnityEngine;
using System.Collections;

public class MusicEnable : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        play();
	}
    public void play()
    {
        GetComponentInChildren<MusicLoader>().enabled = true;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
