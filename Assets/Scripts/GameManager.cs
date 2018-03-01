using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Determine when to spawn, where to spawn, basically manage the scene

public class GameManager : MonoBehaviour {
	public int round = 1;
	int zombiesInRound = 10;
	int zombiesSpawnedInRound = 0;
	float zombieSpawnTimer = 0;

	public static int zombiesLeftInRound = 10;

	public Transform[] zombieSpawnPoints;
	public GameObject zombieEnemy;
	public GameObject gunSelection;
	public Font font;

	float countdown = 0;
	static int playerScore = 0;
	static int playerCash = 0; //don't have to use instance to access these variables

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (zombiesSpawnedInRound < zombiesInRound && countdown == 0) {
			if (zombieSpawnTimer > 30) {
				SpawnZombie ();
				zombieSpawnTimer = 0;
			} else {
				zombieSpawnTimer++;
			}
		} else if (zombiesLeftInRound == 0) {
			StartNextRound ();
		}

		if (countdown > 0) {
			countdown -= Time.deltaTime;
		} else {
			countdown = 0;
		}
	}

	public void AddPoints(int pointValue) {
		playerScore += pointValue;
		playerCash += pointValue;
	}

	void SpawnZombie() {
		Vector3 randomSpawnPoint = zombieSpawnPoints [Random.Range (0, zombieSpawnPoints.Length)].position;
		Instantiate (zombieEnemy, randomSpawnPoint, Quaternion.identity);//Don't need to care about rotation
		zombiesSpawnedInRound++;
	}

	void StartNextRound() {
		zombiesInRound = zombiesLeftInRound = round * 10;
		zombiesSpawnedInRound = 0;
		countdown = 15;
		round++;
	}

	void OnGUI() { //this function automatically draws what I put on the screen
		//automatically called from unity

		GUIStyle mystyle = new GUIStyle ();
		mystyle.font = font;
		mystyle.normal.textColor = Color.white;
		GUI.Label (new Rect (40, Screen.height - 80, 100, 60), "SCORE: ",mystyle);
		GUI.Label (new Rect (110, Screen.height - 80, 160, 60), "" + playerScore,mystyle);

		GUI.Label (new Rect (40, Screen.height - 110, 100, 60), "$: ",mystyle);
		GUI.Label (new Rect (110, Screen.height - 110, 160, 60), "" + playerCash,mystyle);

		if (countdown != 0) {
			GUI.Label (new Rect (Screen.width/2 - 50, Screen.height/2 - 80, 100, 60), "NextRound: ",mystyle);
			GUI.Label (new Rect (Screen.width/2 + 50, Screen.height/2 - 80, 160, 60), "" + Mathf.RoundToInt(countdown),mystyle);
		}

		for (int i = 0; i < gunSelection.transform.childCount; i++) {
			if (gunSelection.transform.GetChild (i).gameObject.activeSelf) {
				//print(gunSelection.transform.GetChild(i).gameObject.tag);
				GUI.Label (new Rect (Screen.width - 110, Screen.height - 95, 160, 60), 
					gunSelection.transform.GetChild (i).gameObject.GetComponent<GunController> ().ammoPerReloadRemaining 
					+ "/"
					+ gunSelection.transform.GetChild (i).gameObject.GetComponent<GunController> ().extraAmmoTotalRemaining, mystyle);
			}
		}
	}
}
