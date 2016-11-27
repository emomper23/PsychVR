using UnityEngine;
using System.Collections;

public class MusicLoader : MonoBehaviour {

    //public string url = ;
    public AudioSource source;
    public string url;
    // Use this for initialization
    void Start () {
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
            source = transform.GetChild(0).GetComponent<AudioSource>();
            source.clip = www.GetAudioClip(true, false, AudioType.WAV);
            if (transform.GetChild(0) !=  null)
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

        if (source && !source.isPlaying)
        {
            //Debug.Log("loaded!!");
        }
            
    }
}
