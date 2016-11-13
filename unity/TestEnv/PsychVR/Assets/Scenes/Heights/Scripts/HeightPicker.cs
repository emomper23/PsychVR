using UnityEngine;
using System.Collections;

public class HeightPicker : MonoBehaviour {

	public GameObject elevator;
	public GameObject top;

	public GameObject[] buildings;

	public float[] heights;

	// Use this for initialization
	void Start () {
		int num = UnityEngine.PlayerPrefs.GetInt("BuildingNum");

		elevator.transform.SetParent(buildings[num].transform);
		elevator.transform.localRotation = Quaternion.identity;
		elevator.transform.localPosition = Vector3.zero;
		elevator.transform.localScale = Vector3.one; 
		top.transform.localPosition = new Vector3(0f, heights[num], 0f);

	}
}
