using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

    // Use this for initialization
    public List<GameObject> ground_list;
    public List<GameObject> wind_list;
    public GameObject elevator;

    void Start ()
    {
        Transform child = transform.GetChild(0);
        int count = child.childCount;
        for (int i = 0; i < count; i++)
        {
            child.GetChild(i).gameObject.SetActive(true);
            child.GetChild(i).GetComponent<AudioSource>().enabled = true;
            child.GetChild(i).GetComponent<OSPAudioSource>().enabled = true;
            ground_list.Add(child.GetChild(i).gameObject);


        }
        child = transform.GetChild(1);
        count = child.childCount;
        for (int i = 0; i < count; i++)
        {
            child.GetChild(i).gameObject.SetActive(true);
            child.GetChild(i).GetComponent<AudioSource>().enabled = true;
            child.GetChild(i).GetComponent<OSPAudioSource>().enabled = true;
            wind_list.Add(child.GetChild(i).gameObject);
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
        foreach (GameObject a in ground_list)
        {
            a.GetComponent<AudioSource>().volume = elevator.GetComponent<ElevatorScript>().getGround();
        }
        foreach (GameObject a in wind_list)
        {
            a.GetComponent<AudioSource>().volume = elevator.GetComponent<ElevatorScript>().getWind();
        }
    }
}
