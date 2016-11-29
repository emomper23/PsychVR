using UnityEngine;
using System.Collections.Generic;
using ProceduralToolkit;

public class ObjectColorSetter : MonoBehaviour {

    // Use this for initialization


    public Gradient gradient1;
    public Gradient gradient2;

    public void LoadMountain()
    {
        
       float rand = Random.Range(0, 100);
        this.gameObject.GetComponent<MeshRenderer>().materials[0].color = gradient1.Evaluate(rand);
        this.gameObject.GetComponent<MeshRenderer>().materials[1].color = gradient2.Evaluate(rand);
        this.gameObject.GetComponent<MeshRenderer>().materials[2].color = gradient2.Evaluate(rand);
        this.gameObject.GetComponent<MeshRenderer>().materials[3].color = gradient1.Evaluate(rand);
        this.gameObject.GetComponent<MeshRenderer>().materials[4].color = gradient1.Evaluate(rand);

        // this.gameObject.GetComponents<Material>()[0].set
    }

    public void LoadTree(int selection)
    {

      
        float rand = Random.Range(0, 100);
        float rand1 = Random.Range(0, 100);
        float rand2= Random.Range(0, 100);
        if (selection == 0)
        {
            this.gameObject.GetComponent<MeshRenderer>().materials[0].color = gradient2.Evaluate(rand);
            this.gameObject.GetComponent<MeshRenderer>().materials[1].color = gradient1.Evaluate(rand1);
            this.gameObject.GetComponent<MeshRenderer>().materials[2].color = gradient2.Evaluate(rand2);
            this.gameObject.GetComponent<MeshRenderer>().materials[3].color = gradient1.Evaluate(rand);
            this.gameObject.GetComponent<MeshRenderer>().materials[4].color = gradient1.Evaluate(rand);
            this.gameObject.GetComponent<MeshRenderer>().materials[5].color = gradient2.Evaluate(rand2);
            this.gameObject.GetComponent<MeshRenderer>().materials[6].color = gradient2.Evaluate(rand1);
            this.gameObject.GetComponent<MeshRenderer>().materials[7].color = gradient1.Evaluate(rand);
            this.gameObject.GetComponent<MeshRenderer>().materials[8].color = gradient1.Evaluate(rand2);
        }
        else
        {
            this.gameObject.GetComponent<MeshRenderer>().materials[0].color = gradient1.Evaluate(rand);
            this.gameObject.GetComponent<MeshRenderer>().materials[1].color = gradient2.Evaluate(rand1);
        }

        



        // this.gameObject.GetComponents<Material>()[0].set
    }
    public void LoadRock()
    {
        float rand = Random.Range(0, 100);
        int sel = (int)rand;
        if (sel % 2 == 0)
        {
            this.gameObject.GetComponent<MeshRenderer>().materials[0].color = gradient1.Evaluate(rand);
        }
        else
        {
            this.gameObject.GetComponent<MeshRenderer>().materials[0].color = gradient2.Evaluate(rand);
        }
        
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
