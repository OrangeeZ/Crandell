using UnityEngine;
using System.Collections;
using UniRx;

public class TimerWaveTrigger : WaveTriggerBase {

	private float _timeCurrent;
	public float Duration;
	private bool _didFire;

	private void OnEnable() {

		Observable.EveryUpdate().Subscribe( OnUpdate );
		//new PMonad().Add( Tick() ).Execute();
	}

	private void OnUpdate(long ticks) {

		if ( _didFire ) return;

		_timeCurrent += Time.deltaTime;

		if ( _timeCurrent >= Duration ) {
			NotifyTrigger();
			_didFire = true;
		}
	}

	private IEnumerable Tick() {
		_timeCurrent = 0f;
		yield return null;

		while ( true ) {

			Debug.Log( _timeCurrent );

			_timeCurrent += Time.deltaTime;

			if ( _timeCurrent >= Duration ) {
				NotifyTrigger();
				yield break;
			}
			yield return null;
		}
	}

}