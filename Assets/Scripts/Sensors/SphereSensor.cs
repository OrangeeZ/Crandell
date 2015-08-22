using System;
using UniRx;
using UnityEngine;
using System.Collections;

public class SphereSensor : MonoBehaviour, IObservable<CharacterPawn> {

    private Subject<CharacterPawn> pawnSubject = new Subject<CharacterPawn>();

    public IDisposable Subscribe( IObserver<CharacterPawn> observer ) {

        return pawnSubject.Subscribe( observer );
    }

    private void OnTriggerEnter( Collider other ) {

        var otherPawn = other.gameObject.GetComponent<CharacterPawn>();

        if ( otherPawn != null ) {

            pawnSubject.OnNext( otherPawn );
        }
    }

}