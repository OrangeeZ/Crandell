﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CollisionSystem : MonoBehaviour {
	
	public class CollisionGroup {
		
		public readonly IList<SimpleSphereCollider> Colliders;
		private readonly float _radius;
		private readonly Vector3 _center;
		
		public CollisionGroup(IList<SimpleSphereCollider> colliders) {
			
			Colliders = colliders;
			
			_center = colliders.Aggregate(Vector3.zero, (total, each) => (total + each.transform.position)) / colliders.Count;
			
			_radius = colliders.Select( _=> (_.transform.position - _center).magnitude + _radius ).Max();
		}
		
		public bool Intersects(SimpleSphereCollider collider) {
			
			return (collider.transform.position - _center).sqrMagnitude <= (_radius + collider.radius).Pow(2);
		}
		
	}
	
	public static CollisionSystem Instance { get; private set; }
	
	public IList<CollisionGroup> Groups { get; private set; }
	
	void Awake() {
		
		Instance = this;
		Groups = new List<CollisionGroup>();
		
		SphereLevelGenerator.Completed += OnLevelGenerated;
	}
	
	private void OnLevelGenerated() {
				
		var groups = new Utility.Collisions.RecursiveDimensionalClustering().GetGroups(Building.instances.Select( _ => _.sphereCollider ).ToList());
		var collisionGroups = new List<CollisionGroup>();
		
		foreach (var each in groups) {
			
			if (each.Count > 0) {
				
				collisionGroups.Add( new CollisionGroup(each) );
			}
		}
		
		Groups = collisionGroups;
		
		SphereLevelGenerator.Completed -= OnLevelGenerated;
	}
}
