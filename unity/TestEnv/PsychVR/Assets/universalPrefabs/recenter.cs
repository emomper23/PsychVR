using UnityEngine;
using System.Collections;

public class ReCenter : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        UnityEngine.VR.InputTracking.Recenter();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F12))
        {
            UnityEngine.VR.InputTracking.Recenter();
        }
    }
}
