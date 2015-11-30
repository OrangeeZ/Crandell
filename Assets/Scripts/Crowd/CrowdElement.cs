using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrowdElement : MonoBehaviour {

	public SimpleSphereCollider sphereCollider;

	public Vector3 spatialHash;

	private PlanetSurfaceTransform _planetTransform;
	private readonly Transform _transform;

	private HashSet<CrowdElement> _testedElements;

	void Awake() {

		_testedElements = new HashSet<CrowdElement>();
	}

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

	public Vector3 CalculatePunishingForce( CrowdElement other ) {

		//other._testedElements.Add( this );

		return sphereCollider.CalculatePunishingForce( other.sphereCollider );
	}

	public bool CanBeTested( CrowdElement other ) {

		return !_testedElements.Contains( other );
	}

	public void Reset() {
		
		_testedElements.Clear();
	}

	private void Update() {

		spatialHash = transform.position;
		spatialHash.Scale( new Vector3( .5f / sphereCollider.radius, .5f / sphereCollider.radius, .5f / sphereCollider.radius ) );
	}

}