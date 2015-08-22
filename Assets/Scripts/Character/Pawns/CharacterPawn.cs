using UniRx;
using UnityEngine;
using System.Collections;

public class CharacterPawn : AObject {

	protected float speed;
    
	[SerializeField]
	private SphereSensor sensor;

	[SerializeField]
	private CharacterComplexAnimationController animatedViewPrefab;

	public CharacterComplexAnimationController animatedView { get; private set; }

	public Character character { get; private set; }

	protected virtual void Start() {

		if ( animatedViewPrefab != null ) {

			animatedView = Instantiate( animatedViewPrefab );
			animatedView.transform.SetParent( transform, worldPositionStays: false );
		}
	}

	public SphereSensor GetSphereSensor() {

		return sensor;
	}

	public void SetCharacter( Character character ) {

		this.character = character;
	}

	public void SetSpeed( float newSpeed ) {

		speed = newSpeed;
	}

	public virtual void MoveDirection( Vector3 direction ) {

		transform.position += speed * direction * Time.deltaTime;
	}

	public virtual void SetDestination( Vector3 destination ) {

		//navMeshAgent.destination = destination;
	}

    public virtual float GetDistanceToDestination() {

        return float.NaN;
    }

    public virtual Vector3 GetDirectionTo( CharacterPawn otherPawn ) {

        return Vector3.forward;
    }
}
