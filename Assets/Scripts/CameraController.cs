using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	// Use this for initialization

	public bool lockCursor;

	public float lookSensitivity = 5f;
	public float xRotation;
	public float yRotation; //x-rotation
	public float currentXRotation;
	public float currentYRotation;
	public float xRotationV;
	public float yRotationV;
	public float lookSmoothDamp = 0.1f;
	public Transform soldier;
	public Transform target;

	public float dstFromTarget = 2;

	void Start () {
		if (lockCursor) { 
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		xRotation -= Input.GetAxis ("Mouse Y") * lookSensitivity;
		yRotation += Input.GetAxis ("Mouse X") * lookSensitivity;

		xRotation = Mathf.Clamp (xRotation, -90, 90);

		currentXRotation = Mathf.SmoothDamp (currentXRotation, xRotation, ref xRotationV, lookSmoothDamp);
		currentYRotation = Mathf.SmoothDamp (currentYRotation, yRotation, ref yRotationV, lookSmoothDamp);

		transform.rotation = Quaternion.Euler (currentXRotation, currentYRotation,0);//used to represent rotation

		//transform.position = new Vector3(soldier.position.x,2,soldier.position.z); FPS
		transform.position = target.position - transform.forward * dstFromTarget;
	}
}
