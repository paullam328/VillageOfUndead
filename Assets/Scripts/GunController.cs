using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

	public float damage = 10f;
	public float range = 100f;
	//public float fireRate = 15f;
	//public float impactForce = 5f; no need to calc force rn

	public Camera tpsCam;
	public ParticleSystem muzzleFlash;
	public GameObject impactEffect;
	public GameObject gunHole;

	public float nextTimeToFire = 0f;

	public int extraAmmoTotal;
	public int ammoPerReload;
	public int extraAmmoTotalRemaining;
	public int ammoPerReloadRemaining;
	public int fireRate;
	public int reloadDelay;
	public string activeWeapon;

	public bool unreloadable = false;
	public bool isReloading = false;

	public GameObject swat;
	public GameObject gunShot;//sound
	private int framesBeforeNextShot = 1;
	private int currentShotFrame = 0;

	// Update is called once per frame
	void Update () {


		//each GunController script stores its own value for the ammo of respective gun, so don't worry about it!

		swat.GetComponent<Animator> ().SetBool ("WieldingKnife", false);

		if (GameObject.FindGameObjectWithTag ("WeaponManager").GetComponent<WeaponManager> ().gunSwitched) {
			/*print (GameObject.FindGameObjectWithTag ("WeaponManager").GetComponent<WeaponManager> ().gunSwitched);

			activeWeapon = GameObject.FindGameObjectWithTag ("WeaponManager").GetComponent<WeaponManager> ().activeWeapon;
			extraAmmoTotal = GameObject.FindGameObjectWithTag ("WeaponManager").GetComponent<WeaponManager> ().ammoTotal;
			ammoPerReload = GameObject.FindGameObjectWithTag ("WeaponManager").GetComponent<WeaponManager> ().ammoPerReload;
			fireRate = GameObject.FindGameObjectWithTag ("WeaponManager").GetComponent<WeaponManager> ().fireRate;
			reloadDelay = GameObject.FindGameObjectWithTag ("WeaponManager").GetComponent<WeaponManager> ().reloadDelay;*/

			/*extraAmmoTotalRemaining = extraAmmoTotal;
			ammoPerReloadRemaining = ammoPerReload;
			unreloadable = false;*/
		}

		bool fireTriggered;

		if (tag == "Handgun") {
			//activeWeapon == "Handgun"
			swat.GetComponent<Animator> ().SetBool ("WieldingPistol", true);
			fireTriggered = Input.GetButtonDown ("Fire1");
		} else {
			swat.GetComponent<Animator> ().SetBool ("WieldingPistol", false);
			fireTriggered = Input.GetButton ("Fire1");
		}





		if (Time.time >= nextTimeToFire) {
			isReloading = false;
			swat.GetComponent<Animator>().SetBool ("IsReloading", false);
		}

		if (!unreloadable && Input.GetKeyDown(KeyCode.R)) {
			Reload();
		}
			

		if (fireTriggered && Time.time >= nextTimeToFire) {//left mouse button

			nextTimeToFire = Time.time + 1f / fireRate;

			/*if (extraAmmoTotalRemaining + ammoPerReloadRemaining <= 30) {
				unreloadable = true;
			}*/

			if (extraAmmoTotalRemaining + ammoPerReloadRemaining <= 0) {
				print ("You've run out of bullets!");
			} else {
				if (ammoPerReloadRemaining > 0) {
					Shoot ();
					ammoPerReloadRemaining--;
					//ammoTotalRemaining--;
				} else {
					if (!unreloadable) {
						Reload ();
					} else {
						print("You've run out of extra bullets to reload!");
					}
				}
			}
		}
	}

	void Shoot() {

		muzzleFlash.Play ();

		if (tag != "Handgun") { 
			if (currentShotFrame == 0) {
				GameObject shot = Instantiate (gunShot, this.transform.position, this.transform.rotation)as GameObject;
				shot.transform.parent = this.transform;
				currentShotFrame = framesBeforeNextShot;
			} else {
				currentShotFrame--;
			}
		} else {
			GameObject shot = Instantiate (gunShot, this.transform.position, this.transform.rotation)as GameObject;
			shot.transform.parent = this.transform;
		}



		RaycastHit hit;

		bool isTarget = false;

		if (Physics.Raycast (tpsCam.transform.position, tpsCam.transform.forward, out hit, range)) {
			//Debug.Log (hit.transform.name);

			ZombieController target = hit.transform.GetComponent<ZombieController> (); //find the script called target from hit

			if (target != null) {
				target.TakeDamage (damage);
				isTarget = true;
			}

			/*if (hit.rigidbody != null) {
				hit.rigidbody.AddForce (-hit.normal*impactForce);
			}*/
		}

		GameObject impactGO = Instantiate (impactEffect, hit.point, Quaternion.LookRotation (hit.normal));
		impactEffect.GetComponent<ParticleSystem> ().Play ();//play it
		Destroy(impactGO,2f);

		if (!isTarget) {
			GameObject gunHoleGO = Instantiate (gunHole, hit.point, Quaternion.LookRotation (hit.normal));
			gunHoleGO.transform.parent = GameObject.Find ("GunHoles").transform;
		}

		//effect, point of particle, and direction of ray hit

		//ray shot from camera position, direction of camera

	}

	void Reload() {

		isReloading = true;
		swat.GetComponent<Animator>().SetBool ("IsReloading", true);
		nextTimeToFire += reloadDelay;

		//if (extraAmmoTotalRemaining > ammoPerReloadRemaining) {
			//ammoTotalRemaining -= ammoPerReload - ammoPerReloadRemaining;
		if (extraAmmoTotalRemaining + ammoPerReloadRemaining > ammoPerReload) {
			print (" ammoPerReload - ammoPerReloadRemaining: " + (ammoPerReload - ammoPerReloadRemaining));
			extraAmmoTotalRemaining -= ammoPerReload - ammoPerReloadRemaining;
			ammoPerReloadRemaining = ammoPerReload;
		} else {
			//ammoTotalRemaining -= ammoPerReload - ammoPerReloadRemaining;
			//print (" ammoPerReload - ammoPerReloadRemaining: " + (ammoPerReload - ammoPerReloadRemaining));
			ammoPerReloadRemaining += extraAmmoTotalRemaining;
			extraAmmoTotalRemaining = 0;
			unreloadable = true;
		}
		//set animation
	}
}
