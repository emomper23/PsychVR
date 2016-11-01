using UnityEngine;
using System.Collections;

public class ColorSetter : MonoBehaviour {

    public string hex_val;
    private Renderer skin;
	// Use this for initialization

	void Start ()
    {
        Color color = hex_str_to_RGB(hex_val);
        skin = this.gameObject.GetComponent<Renderer>();
        Texture tex = skin.material.mainTexture;
        Texture2D targetTexture = new Texture2D(tex.width, tex.height);
        for (int x = 0; x < targetTexture.width; x++)
        {
            for (int y = 0; y < targetTexture.height; y++)
            {
                targetTexture.SetPixel(x, y, color);
            }
        }
        skin.material.mainTexture = targetTexture;
       
	}

    private Color hex_str_to_RGB(string str)
    {
        Color ret = new Color(0, 0, 0);
        string rstr = str.Substring(0, 2);
        string gstr = str.Substring(2, 2);
        string bstr = str.Substring(4, 2);
     
        float r = uint.Parse(rstr, System.Globalization.NumberStyles.HexNumber);
        float g = uint.Parse(gstr,System.Globalization.NumberStyles.HexNumber);
        float b = uint.Parse(bstr, System.Globalization.NumberStyles.HexNumber);
        Debug.Log(r + " " + g + " " + b);
        ret.r = r;
        ret.g = g;
        ret.b = b;


        return ret;
    }
	// Update is called once per frame
	void Update () {
	
	}
}
