using UnityEngine;
using System.Collections.Generic;
using ProceduralToolkit;

public class ObjectColorSetter : MonoBehaviour {

    // Use this for initialization


    public Gradient gradient1;
    public Gradient gradient2;

    public void Load()
    {
        Vector3[] verts = this.gameObject.GetComponent<MeshFilter>().mesh.vertices;
        
        Mesh mesh_temp = this.gameObject.GetComponent<MeshFilter>().mesh;
        int count = this.gameObject.GetComponent<MeshFilter>().mesh.vertexCount;
        Debug.Log(this.gameObject.transform.name + " "+count);
        Color[] temp = new Color[count];
        Gradient blend = BlendGradient(gradient1,gradient2);
        for(int i = 0; i < temp.Length; i++)
        {
            temp[i] = blend.Evaluate(verts[i].y);
        }
        mesh_temp.colors = temp;
        this.gameObject.GetComponent<MeshFilter>().mesh = mesh_temp;
    }
    public Gradient BlendGradient(Gradient terrain_gradient, Gradient object_gradient)
    {
        List<ColorHSV> targetPalette = new List<ColorHSV>();
        List<ColorHSV> currentPalette = new List<ColorHSV>();
        targetPalette = RandomE.TetradicPalette(0.25f, 0.75f);
        Debug.Log(targetPalette.Count);
        ColorHSV groundColor = new ColorHSV(terrain_gradient.Evaluate(0));
        ColorHSV newColor = new ColorHSV(object_gradient.Evaluate(1));
        targetPalette.Add(ColorHSV.Lerp(groundColor, newColor, 0.5f));
        var gradient = ColorE.Gradient(from: targetPalette[2].WithSV(0.8f, 0.8f),
            to: targetPalette[3].WithSV(0.8f, 0.8f));

        return object_gradient;
    } 
    // Update is called once per frame
    void Update () {
	
	}
}
