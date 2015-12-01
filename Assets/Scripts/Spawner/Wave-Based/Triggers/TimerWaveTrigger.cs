using UnityEngine;
using System.Collections;
using UniRx;

public class TimerWaveTrigger : WaveTriggerBase {

	private float _timeCurrent;
	public float Duration;
	private bool _didFire;

	public override void Initialize() {

		new PMonad().Add( Tick() ).Execute();
	}

	private IEnumerable Tick() {
		_timeCurrent = 0f;
		yield return null;

		while ( true ) {

			_timeCurrent += Time.deltaTime;

			if ( _timeCurrent >= Duration ) {
				NotifyTrigger();
				yield break;
			}
			yield return null;
		}
	}

}