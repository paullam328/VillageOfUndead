using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : MonoBehaviour {

	public float walkSpeed = 4;
	public float runSpeed = 6;
	float turnSmoothVelocity;
	float speedSmoothVelocity;
	public float turnSmoothTime = 0.2f;
	public float speedSmoothTime = 0.1f;
	Animator animator;
	float currentSpeed;
	public GameObject cam;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		/*Vector2 input = new Vector2 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		Vector2 inputDir = input.normalized;//change them to less than 1, so it can be used for atan calc

		transform.rotation = Quaternion.Euler(0, cam.GetComponent<CameraController>().currentYRotation, 0);
		//never rotate along x-axis, unless u want the soldier to sleep

		bool running = Input.GetKey (KeyCode.LeftShift);
		float targetSpeed = (running) ? runSpeed : walkSpeed * inputDir.magnitude;
		//returns 0 if idle, and x if moving
		currentSpeed = Mathf.SmoothDamp(currentSpeed,targetSpeed,ref speedSmoothVelocity,speedSmoothTime);

		transform.Translate (transform.forward * currentSpeed * Time.deltaTime, Space.World);

		float animatorSpeedPercent = (running) ? 1 : 0.7f * inputDir.magnitude;

		animator.SetFloat ("speedPercent", animatorSpeedPercent,speedSmoothTime,Time.deltaTime);*/



		Vector2 input = new Vector2 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		//Vector2 inputDir = input.normalized;//change them to less than 1, so it can be used for atan calc
		Vector2 inputDir = input.normalized;

		//print ("Input.GetAxisRaw:" + Input.GetAxisRaw ("Vertical"));
		//print ("InputDir:" + inputDir);
		transform.rotation = Quaternion.Euler(0, cam.GetComponent<CameraController>().currentYRotation, 0);
		//never rotate along x-axis, unless u want the soldier to sleep
		bool running = Input.GetKey (KeyCode.LeftShift);
		float targetSpeed = (running) ? runSpeed * inputDir.y : walkSpeed * inputDir.y;
		//returns 0 if idle, and x if moving
		currentSpeed = Mathf.SmoothDamp(currentSpeed,targetSpeed,ref speedSmoothVelocity,speedSmoothTime);

		transform.Translate (transform.forward * currentSpeed * Time.deltaTime, Space.World);
		//inputdir.magnitude

		float animatorSpeedPercent = (running) ? 1 * inputDir.y: 0.7f * inputDir.y * inputDir.magnitude;
		animator.SetFloat ("verticalSpeedPercent", animatorSpeedPercent, speedSmoothTime, Time.deltaTime);

		/*if (Input.GetAxisRaw("Vertical") == -1) {
			print ("currentSpeed" + currentSpeed);
			print ("animatorSpeedPercent" + animatorSpeedPercent);
		}*/

	}
}

//alert: Rotation is not based on wasd now, it's based on mouse for first person shooter!

/*if (inputDir != Vector2.zero) {
			float targetRotation = Mathf.Atan2 (inputDir.x, inputDir.y) * Mathf.Rad2Deg;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y,targetRotation,ref turnSmoothVelocity,turnSmoothTime);
		}*/
