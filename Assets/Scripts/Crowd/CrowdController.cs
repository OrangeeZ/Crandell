using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CrowdController : MonoBehaviour {

	[SerializeField]
	private Transform _target;

	[SerializeField]
	private List<CrowdElement> _crowdElements = new List<CrowdElement>();

	private void Reset() {

		//this.GetComponentsInChildren( out _crowdElements );
	}

	public void AddElement( CrowdElement element ) {
		
		_crowdElements.Add( element );
	}

	private void Update() {

		foreach ( var each in _crowdElements ) {

			var direction = ( _target.position - each.transform.position ).ClampMagnitude( 1f );

			each.transform.position += direction * Time.deltaTime * 5f;
		}

		foreach ( var each in _crowdElements ) {

			ApplyPunishingForce( each );
		}
	}

	private void ApplyPunishingForce( CrowdElement crowdElement ) {

		var force = _crowdElements
			.Where( _ => _ != crowdElement )
			.Where( _=>_.CompareHash( crowdElement ) )
			.Select( _ => _.sphereCollider )
			.Concat( Building.instances.Select( _ => _.sphereCollider ) )
			.Select( _ => _.CalculatePunishingForce( crowdElement.sphereCollider ) )
			.Aggregate( Vector3.zero, ( total, each ) => total + each );

		crowdElement.transform.position += force.Set( y: 0 );
	}

	private void Advance( PlanetSurfaceTransform crowdElement, Vector3 delta ) {

		//crowdElement.Move(  );
	}

}