using UnityEngine;
using System.Collections;

public class CrowdElement : MonoBehaviour {

	public SimpleSphereCollider sphereCollider;

	public Vector3 spatialHash;

	private PlanetSurfaceTransform _planetTransform;
	private readonly Transform _transform;

	public CrowdElement( Transform transform ) {

		_transform = transform;
	}
	
	public void Move( Vector3 direction, float speed ) {

		_planetTransform.Move( _transform, direction, speed );
	}

	public bool CompareHash( CrowdElement otherElement ) {

		var result = otherElement.spatialHash - spatialHash;
		return result.x.Abs() < 2 && result.y.Abs() < 2 && result.z.Abs() < 2;
	}

	private void Update() {

		spatialHash = transform.position;
		spatialHash.Scale( new Vector3( .5f / sphereCollider.radius, .5f / sphereCollider.radius, .5f / sphereCollider.radius ) );
	}

}