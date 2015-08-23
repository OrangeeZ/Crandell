using UnityEngine;
using System.Collections;

public class CharacterPlanetPawn : CharacterPawn {

    public PlanetSurfaceTransform planetTransform;

    public bool canFollowDestination;

    public GameObject turret;
    
    [SerializeField]
    private float _startingHeight = 5;

    [SerializeField]
    private float _rotationToDirectionSpeed = 100;

    private Vector3? _destination;

    protected override void Start() {

        planetTransform = new PlanetSurfaceTransform( Planet.instance );

        planetTransform.SetHeight( _startingHeight );

        planetTransform.SetPosition( transform );
    }

    private void Update() {

        if ( _destination.HasValue && canFollowDestination ) {

            var direction = planetTransform.GetDirectionTo( _destination.Value );

            planetTransform.Move( transform, Vector3.forward, speed * Time.deltaTime );

            Debug.DrawRay( transform.position, planetTransform.rotation * Vector3.forward );
            Debug.DrawRay( transform.position, transform.rotation * direction.Set( y: 0 ).normalized );

            transform.rotation = Quaternion.RotateTowards( transform.rotation, transform.rotation * Quaternion.FromToRotation( Vector3.forward, direction.Set( y: 0 ) ), _rotationToDirectionSpeed * Time.deltaTime );

            //transform.rotation *= Quaternion.RotateTowards( transform.rotation, transform.rotation * Quaternion.FromToRotation( Vector3.forward, direction.Set( y: 0 ).normalized ), _rotationToDirectionSpeed * Time.deltaTime );
            //transform.rotation *= Quaternion.AngleAxis( _rotationToDirectionSpeed * Time.deltaTime, Vector3.up );
        }
    }

    public override void MoveDirection( Vector3 direction ) {

        planetTransform.Move( transform, direction, speed * Time.deltaTime );
    }

    public override void SetDestination( Vector3 destination ) {

        _destination = destination;
    }

    public override float GetDistanceToDestination() {

        return _destination.HasValue ? planetTransform.GetDistanceTo( _destination.Value ) : float.NaN;
    }

    public override Vector3 GetDirectionTo( CharacterPawn otherPawn ) {

        return planetTransform.GetDirectionTo( otherPawn.position );
    }

    public void ClearDestination() {

        _destination = null;
    }

    public void SetColor( Color baseColor ) {

        GetComponentInChildren<Renderer>().material.SetColor( "_Color", baseColor );
    }

}