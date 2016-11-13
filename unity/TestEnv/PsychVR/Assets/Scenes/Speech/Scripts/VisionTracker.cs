using UnityEngine;
using System.Collections;

public class VisionTracker : MonoBehaviour {

	public float boardTime = 0f;
	public float eyeContactTime = 0f;
	public float floorTime = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		RaycastHit hit;

		Physics.Raycast(ray, out hit, 30f);

		if (hit.collider.gameObject.tag == "human")
		{
			//Debug.Log(hit.collider.gameObject.name);
			hit.collider.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.GetComponent<Animator>().SetBool("sitUp", true);
			eyeContactTime += Time.deltaTime;
		}
		else if (hit.collider.gameObject.tag == "nearFloor")
		{
			floorTime += Time.deltaTime;
		}
		else if (hit.collider.gameObject.tag == "board")
		{
			boardTime += Time.deltaTime;
		}


		//Debug.Log(eyeContactTime);
		//Debug.Log(floorTime);
		//Debug.Log(boardTime);

	}
}
