using UnityEngine;
using System.Collections;

public class ColorSetter : MonoBehaviour {

    public GameObject settings;
    private Renderer skin;
	// Use this for initialization

	void Start ()
    {
        string hex_val = PlayerPrefs.GetString("SkinColor");
        //Debug.Log(hex_val);
        Color color = hex_str_to_RGB(hex_val.Substring(1));
        skin = this.gameObject.GetComponent<Renderer>();
        skin.sharedMaterial.color = color;
        //Debug.Log(color); 
	}

    private Color hex_str_to_RGB(string str)
    {
        Color ret = new Color(0, 0, 0,1);
        string rstr = str.Substring(0, 2);
        string gstr = str.Substring(2, 2);
        string bstr = str.Substring(4, 2);
     
        float r = uint.Parse(rstr, System.Globalization.NumberStyles.HexNumber);
        float g = uint.Parse(gstr,System.Globalization.NumberStyles.HexNumber);
        float b = uint.Parse(bstr, System.Globalization.NumberStyles.HexNumber);
        //Debug.Log(r + " " + g + " " + b);
        ret.r = r / 255.0f;
        ret.g = g / 255.0f;
        ret.b = b / 255.0f;


        return ret;
    }
	// Update is called once per frame
	void Update ()
    {
        //Start();

    }
}
