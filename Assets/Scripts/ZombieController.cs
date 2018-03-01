using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour {

	public float health = 50f;
	UnityEngine.AI.NavMeshAgent nav;
	Transform playerTrans;
	Animator controller;
	int pointValue = 10;

	public GameObject gm;
	bool isDead = false;
	public float timeToAttack;
	public float attackTimer;

	int damage = 10;

	public GameObject zombieSound;
	GameObject sound;

	void Awake() {
		nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
		playerTrans = GameObject.FindGameObjectWithTag ("Player").transform;
		controller = GetComponentInParent<Animator> ();
		sound = Instantiate (zombieSound, this.transform.position, this.transform.rotation) as GameObject;
	}
	void Update () {
		
		if (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> () != null) {
			nav.SetDestination (playerTrans.position);
			controller.SetFloat ("speed", Mathf.Abs (nav.velocity.x + nav.velocity.z));
		}

		if (!isDead) {
			float distance = Vector3.Distance (transform.position,playerTrans.position);//distance between two points
			if (distance < 2.25) {
				nav.isStopped = true;
				controller.SetFloat("speed",0);
				attackTimer += Time.deltaTime;
			} else if (attackTimer > 0) { //if distance >= 3 and attackTimer > 0)
				nav.isStopped = false;
				controller.SetFloat("speed",Mathf.Abs (nav.velocity.x + nav.velocity.z));
				controller.SetBool("IsAttacking",false);
				attackTimer -= Time.deltaTime * 2;
			} else {
				nav.isStopped = false;
				attackTimer = 0;
				controller.SetFloat("speed",Mathf.Abs (nav.velocity.x + nav.velocity.z));
			}
		}
	
	}

	bool Attack() {
		if (attackTimer > timeToAttack) {
			//nav.isStopped = true;
			controller.SetBool("IsAttacking",true);
			attackTimer = 0;
			return true;
		}
		return false;
	}

	public void TakeDamage(float amount) {
		health -= amount;
		if (!isDead) {
			GameObject.FindWithTag ("GameManager").GetComponent<GameManager> ().AddPoints (pointValue);
			if (health <= 0f) {
				Die ();
				isDead = true;
			}
		}
	}

	void OnCollisionStay(Collision collisionInfo) {
		if (collisionInfo.gameObject.tag == "Player") { // if collide with a gameObject called player
			//print("collided");
			if (Attack()) { //if it's ready to attack
				collisionInfo.collider.SendMessageUpwards ("PlayerDamage", damage, SendMessageOptions.RequireReceiver);
			}
		}//have to have the playerdamage function, or else it will return an error
	}

	void Die() {
		//Destroy (gameObject);
		GameManager.zombiesLeftInRound -= 1;
		Destroy (sound);
		controller.SetBool ("IsDying", true);
		Destroy (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>());
		enabled = false;
		Destroy (gameObject.GetComponent<Rigidbody>());
		Destroy (gameObject,15f);
	}
}
