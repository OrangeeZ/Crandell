using UnityEngine;
using System.Collections;

public class CrowdGenerator : MonoBehaviour {

	public CrowdController crowdController;

	public CrowdElement prefab;

	public int amount;
	public float radius;

	// Use this for initialization
	private void Start() {

		for ( var i = 0; i < amount; ++i ) {

			var instance = Instantiate( prefab );
			instance.transform.position = Random.insideUnitCircle.ToXZ() * radius + transform.position;

			crowdController.AddElement( instance );
		}
	}

}