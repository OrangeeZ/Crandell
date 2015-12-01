using UnityEngine;
using System.Collections;

public class WaveTriggerBase : ScriptableObject {

	public event System.Action OnTrigger;

	public virtual void Initialize() {
		

	}

	protected void NotifyTrigger() {
		
		if ( OnTrigger != null ) {

			OnTrigger();
		}
	}

}