using UnityEngine;
using System.Collections;

public class ballToMovement : MonoBehaviour {

	public GameObject sphere;
	public Rigidbody thisRb;
	public Rigidbody sphereRb;
	private Vector3 movementVector;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		movementVector = normalize(sphereRb.angularVelocity);

		//difference between current x and 0 x, same for z, this is our movement speed in a vector direction
		//limit ball between -90 and 90 x and z
		//copy position of sphere to character

		thisRb.AddForce(new Vector3(movementVector.x, 0, movementVector.z));
		
	}

	Vector3 normalize(Vector3 angularVelocity)
	{
		float normal = angularVelocity.x + angularVelocity.y + angularVelocity.z;
		return angularVelocity / normal;
	}
}
