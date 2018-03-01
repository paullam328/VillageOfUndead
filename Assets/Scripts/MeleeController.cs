using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour {

	public GameObject swat;
	public float damage = 50f;
	public float distance = 3f;
	public Camera tpsCam;

	public float nextTimeToFire = 0f;
	public int fireRate;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		swat.GetComponent<Animator> ().SetBool ("WieldingPistol", false);
		swat.GetComponent<Animator> ().SetBool ("WieldingKnife", true);
		//fireTriggered = Input.GetButtonDown ("Fire1");
		swat.GetComponent<Animator> ().SetBool ("IsAttacking", false);
		if (Input.GetButtonDown ("Fire1") && Time.time >= nextTimeToFire) {
			nextTimeToFire = Time.time + 1f / fireRate;
			Stab ();
		}
	}

	void Stab() {
		swat.GetComponent<Animator> ().SetBool ("IsAttacking", true);
		RaycastHit hit;

		bool isTarget = false;

		if (Physics.Raycast (tpsCam.transform.position, tpsCam.transform.forward, out hit,distance)) {
			//Debug.Log (hit.transform.name);

			//distance = hit.distance;

			ZombieController target = hit.transform.GetComponent<ZombieController> (); //find the script called target from hit

			if (target != null) {
				target.TakeDamage (damage);
				isTarget = true;
			}

			/*if (hit.rigidbody != null) {
				hit.rigidbody.AddForce (-hit.normal*impactForce);
			}*/
		}
	}
		
}
