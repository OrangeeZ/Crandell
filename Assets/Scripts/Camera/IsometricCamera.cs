using UnityEngine;
using System.Collections;

public class IsometricCamera : CameraBehaviour {

	public float maxDistance = 5f;

	public float followTimeNormalized = 0.5f;

	protected override void UpdateCamera() {

		var offset = transform.position - target.position;
		var clampedOffset = Vector3.ClampMagnitude( offset, maxDistance );

		transform.position += clampedOffset - offset;

		transform.position = Vector3.Lerp( transform.position, target.position, followTimeNormalized * Time.deltaTime );
        transform.rotation = Quaternion.Lerp( transform.rotation, target.rotation, followTimeNormalized * Time.deltaTime );
	}
}
