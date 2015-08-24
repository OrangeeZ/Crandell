using UnityEngine;
using System.Collections;



public class Soundtrack : MonoBehaviour {

	AudioSource fxSound;
	public AudioClip soundtrack;

	// Use this for initialization
	void Start () {
		fxSound = GetComponent<AudioSource> ();
		fxSound.Play ();
	}
}
