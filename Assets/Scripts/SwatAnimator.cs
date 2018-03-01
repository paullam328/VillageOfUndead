using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatAnimator : MonoBehaviour {

	public float crouchSpeed = 2;
	public float walkSpeed = 4;
	public float runSpeed = 6;
	float turnSmoothVelocity;
	float speedSmoothHorizontalVelocity;
	float speedSmoothVerticalVelocity;
	public float turnSmoothTime = 0.2f;
	public float speedSmoothTime = 0.1f;
	Animator animator;
	float currentVerticalSpeed;
	float currentHorizontalSpeed;
	public GameObject cam;

	public bool isCrouched = false;

	private Rigidbody rb;
	public LayerMask groundLayers;
	public float jumpForce = 7;

	Transform transform;
	private bool isJumping = false;

	int health = 100;
	bool isDead = false;

	bool isReloading = false;


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		transform = GetComponent<Transform> ();
	}

	// Update is called once per frame
	void Update () {

		if (isDead) {
			enabled = false;
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}

		Vector2 input = new Vector2 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		Vector2 inputDir = input.normalized;

		transform.rotation = Quaternion.Euler(0, cam.GetComponent<CameraController>().currentYRotation, 0);
		bool running = Input.GetKey (KeyCode.LeftShift);


		float animatorIdleSpeedPercent = 0;
		animator.SetFloat ("Vertical", animatorIdleSpeedPercent, speedSmoothTime, Time.deltaTime);
		animator.SetFloat ("Horizontal", animatorIdleSpeedPercent, speedSmoothTime, Time.deltaTime);

		//Optimizing speed for diagonal movement

		if ((inputDir.x > 0 && inputDir.x < 1)&&(inputDir.y > 0 && inputDir.y < 1)) {
			inputDir.x = 1;
			inputDir.y = 1;
		}

		if ((inputDir.x > -1 && inputDir.x < 0)&&(inputDir.y > -1 && inputDir.y < 0)) {
			inputDir.x = -1;
			inputDir.y = -1;
		}

		if ((inputDir.x > 0 && inputDir.x < 1)&&(inputDir.y > -1 && inputDir.y < 0)) {
			inputDir.x = 1;
			inputDir.y = -1;
		}

		if ((inputDir.x > -1 && inputDir.x < 0)&&(inputDir.y > 0 && inputDir.y < 1)) {
			inputDir.x = -1;
			inputDir.y = 1;
		}
	
		//crouching

		if (Input.GetKeyDown (KeyCode.C)) {
			if (isCrouched || Input.GetKey (KeyCode.LeftShift)|| Input.GetKeyDown (KeyCode.LeftShift)) {
				isCrouched = false;
			} else {
				isCrouched = true;
			}
			animator.SetBool ("IsCrouched", isCrouched);
		}

		//jumping

		if (transform.position.y <= 0) {
			isJumping = false;
		} else {
			isJumping = true;
		}

		if (Input.GetKeyDown (KeyCode.Space) && !isJumping && !isCrouched) {
			rb.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
		}

		float jumpingVelocity = rb.velocity.y;
		float jumpingVelocityMagnitude = jumpingVelocity / jumpForce;

		if (isJumping) {
			animator.SetBool ("IsJumping", true);
			animator.SetFloat("JumpHeight",jumpingVelocityMagnitude* rb.mass);
		} else {
			animator.SetBool ("IsJumping", false);
		} 

		//Reloading TODO: REWRITE THIS ANIMATION FOR WEAPON OPTIONS IN THE FUTURE
		//print (GameObject.FindGameObjectWithTag("AK-47").GetComponent<GunController>());
		//isReloading = GameObject.FindGameObjectWithTag("AK-47").GetComponent<GunController>().isReloading;
		//animator.SetBool ("IsReloading", isReloading);
		if (inputDir.x == 0 && inputDir.y == 0) {
			animator.SetBool ("IsRunning", false);
		} else {
			animator.SetBool ("IsRunning", true);
		}


		//Horizontal

		if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D)) {
			float targetHorizontalSpeed = (isCrouched) ? crouchSpeed * inputDir.x : (running) ? runSpeed * inputDir.x : walkSpeed * inputDir.x;
			//returns 0 if idle, and x if moving
			currentHorizontalSpeed = Mathf.SmoothDamp(currentHorizontalSpeed,targetHorizontalSpeed,ref speedSmoothHorizontalVelocity,speedSmoothTime);

			transform.Translate (transform.right * currentHorizontalSpeed * Time.deltaTime, Space.World);


			float animatorHorizontalSpeedPercent = (running) ? 2 * inputDir.x : (running) ? 2 * inputDir.x: 1.4f * inputDir.x;
			animator.SetFloat ("Horizontal", animatorHorizontalSpeedPercent, speedSmoothTime, Time.deltaTime);
		} 

		//Vertical

		if (Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.S)){
			float targetVerticalSpeed = (isCrouched) ? crouchSpeed * inputDir.y : (running) ? runSpeed * inputDir.y : walkSpeed * inputDir.y;
			//returns 0 if idle, and x if moving
			currentVerticalSpeed = Mathf.SmoothDamp(currentVerticalSpeed,targetVerticalSpeed,ref speedSmoothVerticalVelocity,speedSmoothTime);
	
			transform.Translate (transform.forward * currentVerticalSpeed * Time.deltaTime, Space.World);


			float animatorVerticalSpeedPercent = (running) ? 2 * inputDir.y : (running) ? 2 * inputDir.y: 1.4f * inputDir.y;
			animator.SetFloat ("Vertical", animatorVerticalSpeedPercent, speedSmoothTime, Time.deltaTime);

		}


	}

	void PlayerDamage(int damage) {
		if (!isDead) {
			health -= damage;
			print ("Current health: " + health);
			if (health <= 0) {
				Die ();
			}
		}
	}

	void Die() {
		animator.SetBool ("IsDead", true);
		isDead = true;
		Destroy (gameObject.GetComponent<Rigidbody> ());
	}
}
