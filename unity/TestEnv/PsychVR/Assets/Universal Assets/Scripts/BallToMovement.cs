using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using CnControls;

public class BallToMovement : MonoBehaviour
{
	public Rigidbody thisRb;
    public float speed;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void FixedUpdate()
	{

		//movementVector = normalize(sphereRb.angularVelocity);

		//difference between current x and 0 x, same for z, this is our movement speed in a vector direction
		//limit ball between -90 and 90 x and z
		//copy position of sphere to character

		//Debug.Log(CnInputManager.GetAxis("Horizontal"));

		//Quaternion.LookRotation(camera.transform.eulerAngles, Vector3.up);
		//camera.transform.
		//Debug.Log(test);
		
		//thisRb.transform.Rotate(new Vector3(0, camera.transform.rotation.y,0),; // = new Vector3(0f, camera.transform.rotation.y, 0f);

		//if (CnInputManager.GetAxis("Horizontal") != 0 || CnInputManager.GetAxis("Vertical") != 0)
		//	thisRb.AddForce(new Vector3(CnInputManager.GetAxis("Horizontal"), 0f, CnInputManager.GetAxis("Vertical")));

		if (CnInputManager.GetAxis("Horizontal") != 0 || CnInputManager.GetAxis("Vertical") != 0)
		{
            if (thisRb.velocity.magnitude < 13)
            {
                Vector3 test = Camera.main.worldToCameraMatrix * new Vector3(CnInputManager.GetAxis("Horizontal"), 0, -CnInputManager.GetAxis("Vertical"));
                test.Scale(new Vector3(speed, speed, speed));
                thisRb.AddForce(test);
            }
			
		}
	}

	/*Vector3 normalize(Vector3 angularVelocity)
	{
		float normal = angularVelocity.x + angularVelocity.y + angularVelocity.z;
		return angularVelocity / normal;
	}*/
}