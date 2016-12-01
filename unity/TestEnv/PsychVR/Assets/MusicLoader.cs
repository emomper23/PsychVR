using UnityEngine;
using System.Collections;

public class MusicLoader : MonoBehaviour {

    //public string url = ;
    public AudioSource source;
    public string url;
    private bool global = false;
    // Use this for initialization
    void Awake ()
    {
        Debug.Log(PlayerPrefs.GetString("Song"));
        if (PlayerPrefs.GetString("Song") == "")
        {
            global = true;
            source = transform.GetComponent<AudioSource>();
            Debug.Log("Default");
            return;
        }
        url = "file:///"+PlayerPrefs.GetString("Song");
        WWW www = new WWW(url);

        StartCoroutine(WaitForRequest(www));
       
        
    }
    IEnumerator WaitForRequest(WWW www)
    {

        yield return www;
        
        

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: ");
            if (transform.childCount > 0)
            {

                source = transform.GetChild(0).GetComponent<AudioSource>();
            }

            else
            {
                source = transform.GetComponent<AudioSource>();
                global = true;
            }
                

            source.clip = www.GetAudioClip(true, false, AudioType.WAV);

            if (transform.childCount > 0)
            {
                transform.GetChild(0).GetComponent<AudioSource>().clip = source.clip;
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(0).GetComponent<AudioSource>().enabled = true;
                transform.GetChild(0).GetComponent<OSPAudioSource>().enabled = true;
            }
        }
        else
        {
            Debug.Log(www.url);
            Debug.Log("WWW Error: " + www.error);
        }
    }

        // Update is called once per frame
        void Update () {

        if (global && source && !source.isPlaying)
        {
            source.Play();
        }
            
    }
}
