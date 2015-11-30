using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Utility.Collisions;

public class CrowdController : MonoBehaviour {

	[SerializeField]
	private Transform _target;

	[SerializeField]
	private List<CrowdElement> _crowdElements = new List<CrowdElement>();

	private RecursiveDimensionalClustering _clustering = new RecursiveDimensionalClustering();
	
	private void Reset() {

		//this.GetComponentsInChildren( out _crowdElements );
	}

	public void AddElement( CrowdElement element ) {

		_crowdElements.Add( element );
	}

	private void Update() {

		foreach ( var each in _crowdElements ) {

			var direction = ( _target.position - each.transform.position ).ClampMagnitude( 1f );

			each.transform.position += direction * Time.deltaTime * 15f;
		}

		_clustering = new RecursiveDimensionalClustering();
		var pairs = _clustering.Clusterize( _crowdElements.Select( _ => _.sphereCollider ).ToList() );

		//var buildingColliders = Building.instances
		//	.Select( _ => _.sphereCollider )
		//	.ToList();

		foreach ( var each in pairs ) {

			var punishingForce = each.a.CalculatePunishingForce( each.b ).Set( y: 0 ) * 0.5f;
			each.b.transform.position += punishingForce;
			each.a.transform.position -= punishingForce;
		}

		//foreach ( var each in _crowdElements ) {
			
		//	//var buildingsPunishingForces = buildingColliders
		//	//	.Select( _ => _.CalculatePunishingForce( each.sphereCollider ) )
		//	//	.Aggregate( Vector3.zero, ( total, _ ) => total + _ );

		//	buildingColliders.Add( each.sphereCollider );

		//	foreach ( var pair in _clustering.Clusterize( buildingColliders ) ) {

		//		if ( pair.a != each || pair.b != each ) {

		//			continue;
		//		}

		//		var self = pair.a == each ? pair.a : pair.b;
		//		var other = pair.b != each ? pair.b : pair.a;

		//		self.transform.position += other.CalculatePunishingForce( self ).Set( y: 0 );
		//	}

		//	buildingColliders.RemoveAt( buildingColliders.Count - 1 );

		//	//each.transform.position += buildingsPunishingForces.Set( y: 0 );

		//	//var cluster = clusters.FirstOrDefault( which => which.Contains( each.sphereCollider ) );

		//	//ApplyPunishingForce( each, cluster );
		//}
		
		//foreach ( var each in _crowdElements ) {

		//	each.Reset();
		//}
	}

	private void ApplyPunishingForce( CrowdElement crowdElement, IList<SimpleSphereCollider> otherElements ) {

		var buildingsPunishingForces = Building.instances
			.Select( _ => _.sphereCollider )
			.Select( _ => _.CalculatePunishingForce( crowdElement.sphereCollider ) );

		var force = otherElements
			//.Skip( elementIndex )
			//.Where( crowdElement.CanBeTested )
			//.Where( _ => 1f.Random() > 0.95f )
			//.Where( crowdElement.CompareHash )
			.Select( _ => _.CalculatePunishingForce( crowdElement.sphereCollider ) )
			.Concat( buildingsPunishingForces )
			.Aggregate( Vector3.zero, ( total, each ) => total + each );

		crowdElement.transform.position += force.Set( y: 0 );
	}

}