using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;

public class Soundtrack : MonoBehaviour {

    private AudioSource fxSound;
    public AudioClip soundtrack;

    public AudioClip endgameSoundtrack;

    // Use this for initialization
    private void Start() {

        fxSound = GetComponent<AudioSource>();
        fxSound.Play();

        EventSystem.Events.SubscribeOfType<BossDeadStateInfo.Died>( OnBossDie );
    }

    private void OnBossDie( BossDeadStateInfo.Died obj ) {

        fxSound = GetComponent<AudioSource>();
        fxSound.Stop();
        fxSound.clip = endgameSoundtrack;
        fxSound.Play();
    }

}