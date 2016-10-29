using UnityEngine;
using System.Collections;

public class copyRotation : MonoBehaviour {

	public GameObject sphere;
	public float speed;

	private Rigidbody rb;
	private Rigidbody rb2;
	private int count;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		count = 0;
		speed = 0.01f;
		rb2 = sphere.GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		Debug.Log(rb2.angularVelocity);
		float moveHorizontal = rb2.angularVelocity.x;
		float moveVertical = rb2.angularVelocity.z;
		if (moveHorizontal < 0.5f && moveVertical < 0.5f)
		{
			return;
		}
		float sign_x = 0;
		float sign_y = 0;

		if (moveHorizontal > 0)
		{
			sign_x = 1.0f;
		}
		else if (moveHorizontal < 0)
		{
			sign_x = -1.0f;
		}
		else
		{
			sign_x = 0.0f;
		}

		if (moveVertical > 0)
		{
			sign_y = 1.0f;
		}
		else if (moveVertical < 0)
		{
			sign_y = -1.0f;
		}
		else
		{
			sign_y = 0.0f;
		}

		Vector3 movement = new Vector3(sign_x, 0, sign_y);

		this.transform.Translate(-movement * speed);

	}
}
