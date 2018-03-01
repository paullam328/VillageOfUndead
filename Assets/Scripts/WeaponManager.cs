using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

	public string activeWeapon = "none";
	public int ammoTotal;
	public int ammoPerReload;
	public int fireRate;
	public int reloadDelay;

	public GameObject gunSelection;
	public bool gunSwitched = false;
	public int akCount = 0;
	public int handgunCount = 0;

	//remove weapon if it's not equipped, and 

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		string postActiveWeapon = activeWeapon;
		SwitchingWeapon ();
		GetGun ();
		if (postActiveWeapon == activeWeapon) {
			gunSwitched = false;
		} else {
			//print (true);
			gunSwitched = true;
		}



		if (gunSwitched) {
			for (int i = 0; i < gunSelection.transform.childCount; i++) {
				if (gunSelection.transform.GetChild (i).gameObject.tag == activeWeapon) {
					if ((activeWeapon == "AK-47" && akCount < 1) || (activeWeapon == "Handgun" && handgunCount < 1)) {
						gunSelection.transform.GetChild (i).gameObject.GetComponent<GunController> ().activeWeapon = activeWeapon;
						gunSelection.transform.GetChild (i).gameObject.GetComponent<GunController> ().extraAmmoTotal = ammoTotal;
						gunSelection.transform.GetChild (i).gameObject.GetComponent<GunController> ().ammoPerReload = ammoPerReload;
						gunSelection.transform.GetChild (i).gameObject.GetComponent<GunController> ().fireRate = fireRate;
						gunSelection.transform.GetChild (i).gameObject.GetComponent<GunController> ().reloadDelay = reloadDelay;

						gunSelection.transform.GetChild (i).gameObject.GetComponent<GunController> ().extraAmmoTotalRemaining = ammoTotal;
						gunSelection.transform.GetChild (i).gameObject.GetComponent<GunController> ().ammoPerReloadRemaining = ammoPerReload;
						gunSelection.transform.GetChild (i).gameObject.GetComponent<GunController> ().unreloadable = false;

					}

					if (activeWeapon == "AK-47") {akCount++;}
					if (activeWeapon == "Handgun") {handgunCount++;}


				}
			}
		}
	}

	void SwitchingWeapon() {
		if (Input.GetKeyDown (KeyCode.Alpha1) || Input.GetKeyDown (KeyCode.Keypad1)) {
			for (int i = 0; i < gunSelection.transform.childCount; i++) {
				if (gunSelection.transform.GetChild (i).gameObject.tag == "AK-47") {
					gunSelection.transform.GetChild (i).gameObject.SetActive(true);
				}
				else {
					gunSelection.transform.GetChild (i).gameObject.SetActive(false);
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.Alpha2) || Input.GetKeyDown (KeyCode.Keypad2)) {
			for (int i = 0; i < gunSelection.transform.childCount; i++) {
				if (gunSelection.transform.GetChild (i).gameObject.tag == "Handgun") {
					gunSelection.transform.GetChild (i).gameObject.SetActive(true);
				}
				else {
					gunSelection.transform.GetChild (i).gameObject.SetActive(false);
				}
			}
		}
			
		if (Input.GetKeyDown (KeyCode.Alpha3) || Input.GetKeyDown (KeyCode.Keypad3)) {
			for (int i = 0; i < gunSelection.transform.childCount; i++) {
				if (gunSelection.transform.GetChild (i).gameObject.tag == "Knife") {
					gunSelection.transform.GetChild (i).gameObject.SetActive(true);
				}
				else {
					gunSelection.transform.GetChild (i).gameObject.SetActive(false);
				}
			}
		}

	}

	void GetGun() {
		gunSelection = GameObject.FindGameObjectWithTag ("GunSelection");
		for (int i = 0; i < gunSelection.transform.childCount; i++) {
			if (gunSelection.transform.GetChild (i).gameObject.activeSelf) {
				if (gunSelection.transform.GetChild (i).gameObject.tag == "AK-47") { //if it's checked and shown
					activeWeapon = "AK-47";
					ammoTotal = 150;
					ammoPerReload = 30;
					fireRate = 15;
					reloadDelay = 3;
				}
				if (gunSelection.transform.GetChild (i).gameObject.tag == "Handgun") {
					activeWeapon = "Handgun";
					ammoTotal = 21;
					ammoPerReload = 7;
					fireRate = 4;
					reloadDelay = 3;
				}
			}
		}

	}

	void SwitchGun() {

	}
}
