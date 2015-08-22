using UnityEngine;
using System.Collections;

public class CharacterPlanetPawn : CharacterPawn {

    public PlanetSurfaceTransform planetTransform;

    [SerializeField]
    private float _startingHeight = 5;

    private Vector3? _destination;
    public bool canFollowDestination;

    private void Awake() {

        planetTransform = new PlanetSurfaceTransform( FindObjectOfType<Planet>() );

        planetTransform.SetHeight( _startingHeight );
    }

    private void Update() {

        if ( _destination.HasValue && canFollowDestination ) {

            var direction = planetTransform.GetDirectionTo( _destination.Value );

            planetTransform.Move( transform, direction.Set( y: 0 ).normalized, speed * Time.deltaTime );
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