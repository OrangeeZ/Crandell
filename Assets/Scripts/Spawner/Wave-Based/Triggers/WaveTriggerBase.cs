using UnityEngine;
using System.Collections;

public class WaveTriggerBase : ScriptableObject {

	public event System.Action OnTrigger;

	protected void NotifyTrigger() {

		Debug.Log( "Trigger!" );

		if ( OnTrigger != null ) {

			OnTrigger();
		}
	}

}