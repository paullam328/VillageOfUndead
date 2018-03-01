using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatSounds : MonoBehaviour {

	// Use this for initialization
	[SerializeField]
	private AudioClip[] stepClips;
	[SerializeField]
	private AudioClip reloadClip;

	private AudioSource audioSource;

	void Awake () {
		audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Step () {
		AudioClip clip = stepClips [UnityEngine.Random.Range (0, stepClips.Length)];
		audioSource.PlayOneShot (clip);
	}

	void Reload() {
		AudioClip clip = reloadClip;
		audioSource.PlayOneShot (clip);
	}

}
