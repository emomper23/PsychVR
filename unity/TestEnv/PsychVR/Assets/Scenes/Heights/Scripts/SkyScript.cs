using UnityEngine;
using System.Collections;

public class SkyScript : MonoBehaviour {

	public GameObject sun;
	public GameObject stars;
	public GameObject moon;

	// Use this for initialization
	void Start () {
		if( PlayerPrefs.GetInt("Day") != 1 ){
			this.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(1, 1);
			stars.SetActive(false);
			moon.SetActive(false);
		}
		else 
		{
			sun.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
