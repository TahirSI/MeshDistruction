using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
	/*
		FEATURES
			WASD/Arrows:    Movement
					  Q:    Climb
					  E:    Drop
						  Shift:    Move faster
						Control:    Move slower
							End:    Toggle cursor locking to screen (you can also press Ctrl+P to toggle play mode on and off).
		*/

	public float cameraSensitivity = 90;
	public float climbSpeed = 4;
	public float normalMoveSpeed = 10;
	public float slowMoveFactor = 0.25f;
	public float fastMoveFactor = 3;

	public float rotationX = 0.0f;
	public float rotationY = 0.0f;

	void Start()
	{

	}

	void Update()
	{
		int xrot = 0;
		int yrot = 0;

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			xrot = -1;
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			xrot = 1;
		}

		if (Input.GetKey(KeyCode.DownArrow))
		{
			yrot = -1;
		}
		else if (Input.GetKey(KeyCode.UpArrow))
		{
			yrot = 1;
		}

		rotationX += xrot * cameraSensitivity * Time.deltaTime;
		rotationY += yrot * cameraSensitivity * Time.deltaTime;
		rotationY = Mathf.Clamp(rotationY, -90, 90);

		transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
		transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

		int xspeed = 0;
		int yspeed = 0;

		if (Input.GetKey(KeyCode.A))
		{
			xspeed = -1;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			xspeed = 1;
		}

		if (Input.GetKey(KeyCode.S))
		{
			yspeed = -1;
		}
		else if (Input.GetKey(KeyCode.W))
		{
			yspeed = 1;
		}

		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			transform.position += transform.forward * (normalMoveSpeed * fastMoveFactor) * yspeed * Time.deltaTime;
			transform.position += transform.right * (normalMoveSpeed * fastMoveFactor) * xspeed * Time.deltaTime;
		}
		else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
		{
			transform.position += transform.forward * (normalMoveSpeed * slowMoveFactor) * yspeed * Time.deltaTime;
			transform.position += transform.right * (normalMoveSpeed * slowMoveFactor) * xspeed * Time.deltaTime;
		}
		else
		{

			transform.position += transform.forward * normalMoveSpeed * yspeed * Time.deltaTime;
			transform.position += transform.right * normalMoveSpeed * xspeed * Time.deltaTime;
		}


		if (Input.GetKey(KeyCode.Q)) { transform.position += transform.up * climbSpeed * Time.deltaTime; }
		if (Input.GetKey(KeyCode.E)) { transform.position -= transform.up * climbSpeed * Time.deltaTime; }
	}
}
