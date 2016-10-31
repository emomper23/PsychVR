using UnityEngine;
using System.Collections;

public class HumanController : MonoBehaviour {

	public GameObject[] seats;

	public GameObject human01Prefab;

	public Vector3 zeroVector = new Vector3( 0, 0, 0 );
	public Vector3 oneVector = new Vector3(1, 1, 1);
	public Quaternion zeroQuaternion = new Quaternion( 0f, 0f, 0f, 0f );

	public Vector3[] animTransforms;

	public Vector3[] animRotations;

	//7.64f
	// Use this for initialization
	void Start () {
		//if (seats == null)
			seats = GameObject.FindGameObjectsWithTag("position");

		foreach (GameObject seat in seats)
		{
			int animInt = Random.Range(1, 9);
			Debug.Log(animRotations[animInt-1] + " " + animInt + " " + seat.transform.rotation);
			GameObject human = (GameObject)Instantiate(human01Prefab, zeroVector, zeroQuaternion);
			human.transform.parent = seat.transform;
			human.transform.localScale = oneVector;
			human.transform.localEulerAngles = animRotations[animInt - 1]; 
			human.transform.localPosition = animTransforms[animInt-1];
			human.GetComponent<Animator>().SetInteger("position", animInt);
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
